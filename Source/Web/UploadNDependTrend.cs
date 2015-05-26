using System.Web;
using System.Xml.Linq;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System;
using System.Text;
using System.Reflection;
using Microsoft.AspNet.SignalR;

namespace Web
{
    public class UploadNDependTrend : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        float[] GetValuesFrom(XElement element)
        {
            var values = element.Attribute("V");
            if (values == null) return new float[0];

            return values.Value.Split('|').Select(v => float.Parse(v)).ToArray();
        }

        string GetPropertyNameFromRuleName(string ruleName)
        {

            var words = ruleName.Split(' ');

            var propertyName = new StringBuilder();
            foreach (var word in words)
            {
                var correctedWord = ReplaceAndExpandStrings(word);
                propertyName.AppendFormat("{0}{1}", correctedWord.Substring(0, 1).ToUpperInvariant(), correctedWord.Substring(1));
            }

            var result = propertyName.ToString();
            result = result.Replace("OfOf", "Of");
            return result;
        }

        string ReplaceAndExpandStrings(string word)
        {
            var replacements = new[] {
                "(", string.Empty,
                ")", string.Empty,
                "-", string.Empty,
                "#", "NumberOf"
            };

            var propertyName = word;
            for (var replacementIndex = 0; replacementIndex < replacements.Length; replacementIndex += 2)
            {
                propertyName = propertyName.Replace(replacements[replacementIndex], replacements[replacementIndex + 1]);
            }

            return propertyName;
        }


        Measurement ParseAndGetLastMeasurement(string buildIdentifier, Stream stream)
        {
            var trendData = XDocument.Load(stream);

            var root = trendData.FirstNode as XElement;

            var ruleNames = root.Elements()
                                    .Where(e => e.Name.LocalName == "MetricIndex")
                                        .Elements()
                                            .Where(e => e.Name.LocalName == "Metric")
                                            .Select(e => e.Attribute("Name").Value).ToArray();

            var mElement = root.Elements().Where(e => e.Name.LocalName == "M").SingleOrDefault();
            if (mElement != null)
            {
                var rElementsQuery = mElement.Elements().Where(e => e.Name.LocalName == "R");
                var last = rElementsQuery.LastOrDefault();
                if (last != null)
                {
                    var values = GetValuesFrom(last);

                    var time = DateTime.Now;
                    var dateAttribute = last.Attribute("D");
                    if (dateAttribute != null) time = DateTime.Parse(dateAttribute.Value);

                    var measurement = new Measurement(buildIdentifier, time);
                    var type = typeof(Measurement);
                    for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
                    {
                        var ruleName = ruleNames[valueIndex];
                        var propertyName = GetPropertyNameFromRuleName(ruleName);

                        var property = type.GetProperty(propertyName);
                        if (property != null)
                            property.SetValue(measurement, Convert.ChangeType(values[valueIndex], property.PropertyType));
                    }

                    return measurement;
                }
            }
            return null;
        }

        EntityProperty GetEntityPropertyFromProperty(Measurement measurement, PropertyInfo property)
        {
            var value = property.GetValue(measurement);
            var constructor = typeof(EntityProperty).GetConstructor(new Type[] { property.PropertyType });
            if (constructor != null) return constructor.Invoke(new[] { value }) as EntityProperty;
            return new EntityProperty(value.ToString());
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count == 1)
            {
                var file = context.Request.Files[0];

                var buildIdentifier = context.Request.Form["BuildIdentifier"];
                if (string.IsNullOrEmpty(buildIdentifier))
                {
                    buildIdentifier = "Default";
                }

                var measurement = ParseAndGetLastMeasurement(buildIdentifier, file.InputStream);
                if (measurement != null)
                {
                    var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ndependvisualizer;AccountKey=Mdf7RkUa9luB43JBwhRdzyiMfskltIVHz8Vw+u1gelB+r7Iacqo3xACrjkPrrSoL5SzUuW43le5GDYexMSzkuA==");

                    var tableClient = account.CreateCloudTableClient();
                    var table = tableClient.GetTableReference("measurements");
                    table.CreateIfNotExists();

                    var tableEntityValues = measurement.GetType().GetProperties().ToDictionary(p=>p.Name, p => GetEntityPropertyFromProperty(measurement,p));
                    var time = measurement.Time.ToString("yyyyMMddhhmm");
                    var tableEntity = new DynamicTableEntity(buildIdentifier, time)
                    {
                        ETag = "*",
                        Properties = tableEntityValues
                    };

                    var operation = TableOperation.InsertOrReplace(tableEntity);
                    table.Execute(operation);
                }

                var trendHub = GlobalHost.ConnectionManager.GetHubContext<TrendHub>();
                trendHub.Clients.All.trendUpdated();
                
                context.Response.Write("You uploaded " + file.FileName+" with "+file.ContentLength+" bytes");

            } else {
                context.Response.Write("OK");
            }
        }
    }
}
