using System.Collections.Generic;
using System.IO;

namespace Web.NDepend
{
    public interface IResults
    {
        void MergeWith(BuildIdentifier buildIdentifier, Stream stream);
        IEnumerable<Measurement> GetAll(BuildIdentifier buildIdentifier);
    }
}
