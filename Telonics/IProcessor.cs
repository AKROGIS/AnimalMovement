using System.Collections.Generic;

namespace Telonics
{
    public interface IProcessor
    {
        IEnumerable<string> ProcessTransmissions(IEnumerable<ArgosTransmission> transmissions, ArgosFile file);
    }
}
