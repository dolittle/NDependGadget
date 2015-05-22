using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Web.NDepend
{
    public class Results : IResults
    {
        IResultParser _parser;
        IRepository _repository;

        public Results(IResultParser parser, IRepository repository)
        {
            _parser = parser;
            _repository = repository;
        }

        public void MergeWith(BuildIdentifier buildIdentifier, Stream stream)
        {
            var result = _parser.GetAllFrom(stream);
            var existing = _repository.GetAll(buildIdentifier);

            var newMeasurements = result.Where(measurement => !existing.Contains(measurement));
            _repository.Save(buildIdentifier, newMeasurements);
        }
        
        public IEnumerable<Measurement> GetAll(BuildIdentifier buildIdentifier)
        {
            return _repository.GetAll(buildIdentifier);
        }
    }
}
