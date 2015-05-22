using System.Collections.Generic;
using System.IO;

namespace Web.NDepend
{
    public interface IResultParser
    {
        Measurement GetLastFrom(Stream stream);
        IEnumerable<Measurement> GetAllFrom(Stream stream);
    }
}
