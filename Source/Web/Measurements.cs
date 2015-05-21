using System;
using System.Linq;
using Bifrost.Read;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Web
{
    public class Measurements : IQueryFor<Measurement>
    {
        public Measurements()
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ndependvisualizer;AccountKey=Mdf7RkUa9luB43JBwhRdzyiMfskltIVHz8Vw+u1gelB+r7Iacqo3xACrjkPrrSoL5SzUuW43le5GDYexMSzkuA==");

            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference("measurements");
            
        }

        public IQueryable<Measurement> Query
        {
            get
            {
                return new[] {
                    new Measurement("", DateTime.Now) { NumberOfRulesViolations = 42, AverageCyclomaticComplexityForMethods = 70, },
                    new Measurement("", DateTime.Now.AddDays(1)) { NumberOfRulesViolations = 44, AverageCyclomaticComplexityForMethods = 73, },
                    new Measurement("", DateTime.Now.AddDays(2)) { NumberOfRulesViolations = 49, AverageCyclomaticComplexityForMethods = 77, },
                    new Measurement("", DateTime.Now.AddDays(3)) { NumberOfRulesViolations = 41, AverageCyclomaticComplexityForMethods = 90, }
                }.AsQueryable();
            }
        }
    }
}
