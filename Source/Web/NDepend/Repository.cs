using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Bifrost.Execution;
using Bifrost.Serialization;

namespace Web.NDepend
{
    [Singleton]
    public class Repository : IRepository
    {
        ISerializer _serializer;
        Dictionary<BuildIdentifier, IEnumerable<Measurement>> _measurementsPerBuild = new Dictionary<BuildIdentifier, IEnumerable<Measurement>>();

        public Repository(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<Measurement> GetAll(BuildIdentifier buildIdentifier)
        {
            if (_measurementsPerBuild.ContainsKey(buildIdentifier)) return _measurementsPerBuild[buildIdentifier];
            var fileName = GetFileNameFor(buildIdentifier);
            if (File.Exists(fileName))
            {
                return ReadFromDiskAndDeserialize(buildIdentifier, fileName);
            }
            else
            {
                return new Measurement[0];
            }
        }

        private IEnumerable<Measurement> ReadFromDiskAndDeserialize(BuildIdentifier buildIdentifier, string fileName)
        {
            var measurementsAsJson = File.ReadAllText(fileName);

            var measurements = _serializer.FromJson<IEnumerable<Measurement>>(measurementsAsJson);
            _measurementsPerBuild[buildIdentifier] = measurements;
            return measurements;
        }

        public void Save(BuildIdentifier buildIdentifier, IEnumerable<Measurement> measurements)
        {
            var all = MergeWithExisting(buildIdentifier, measurements);
            SerializeAndWriteToDisk(buildIdentifier, all);
        }

        IEnumerable<Measurement> MergeWithExisting(BuildIdentifier buildIdentifier, IEnumerable<Measurement> measurements)
        {
            var all = new List<Measurement>(GetAll(buildIdentifier));
            var newMeasurements = measurements.Where(measurement => !all.Contains(measurement));
            all.AddRange(newMeasurements);
            return all;
        }

        string GetFileNameFor(BuildIdentifier buildIdentifier)
        {
            var fileName = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/{0}.json", buildIdentifier));
            return fileName;
        }

        void SerializeAndWriteToDisk(BuildIdentifier buildIdentifier, IEnumerable<Measurement> all)
        {
            var measuresmentsAsJson = _serializer.ToJson(all);
            var fileName = GetFileNameFor(buildIdentifier);
            File.WriteAllText(fileName, measuresmentsAsJson);
        }
    }
}
