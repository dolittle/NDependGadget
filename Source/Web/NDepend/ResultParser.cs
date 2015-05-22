using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web.NDepend
{
    public class ResultParser : IResultParser
    {
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


        IEnumerable<Measurement> Parse(Stream stream)
        {
            var measurements = new List<Measurement>();

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
                foreach (var element in rElementsQuery)
                {
                    var values = GetValuesFrom(element);

                    var time = DateTime.Now;
                    var dateAttribute = element.Attribute("D");
                    if (dateAttribute != null) time = DateTime.Parse(dateAttribute.Value);

                    var measurement = new Measurement { Time = time };
                    var type = typeof(Measurement);
                    for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
                    {
                        var ruleName = ruleNames[valueIndex];
                        var propertyName = GetPropertyNameFromRuleName(ruleName);

                        var property = type.GetProperty(propertyName);
                        if (property != null)
                            property.SetValue(measurement, Convert.ChangeType(values[valueIndex], property.PropertyType));
                    }

                    measurements.Add(measurement);
                }
            }
            return measurements;
        }

        EntityProperty GetEntityPropertyFromProperty(Measurement measurement, PropertyInfo property)
        {
            var value = property.GetValue(measurement);
            var constructor = typeof(EntityProperty).GetConstructor(new Type[] { property.PropertyType });
            if (constructor != null) return constructor.Invoke(new[] { value }) as EntityProperty;
            return new EntityProperty(value.ToString());
        }


        public Measurement GetLastFrom(Stream stream)
        {
            var measurements = Parse(stream);
            return measurements.LastOrDefault();
        }

        public IEnumerable<Measurement> GetAllFrom(Stream stream)
        {
            var measurements = Parse(stream);
            return measurements;
        }
    }
}