using System.Collections.Generic;

namespace Web.NDepend
{
    public interface IRepository
    {
        IEnumerable<Measurement> GetAll(BuildIdentifier buildIdentifier);
        void Save(BuildIdentifier buildIdentifier, IEnumerable<Measurement> measurements);
    }
}
