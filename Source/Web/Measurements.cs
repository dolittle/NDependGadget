using System.Linq;
using Bifrost.Read;
using Web.NDepend;

namespace Web
{
    public class Measurements : IQueryFor<Measurement>
    {
        IResults _results;

        public Measurements(IResults results)
        {
            _results = results;
        }

        public BuildIdentifier BuildIdentifier { get; set; }

        public IQueryable<Measurement> Query
        {
            get
            {
                return _results.GetAll(BuildIdentifier).AsQueryable();
            }
        }
    }
}
