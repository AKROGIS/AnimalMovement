using System.Collections.Generic;

namespace Telonics
{
    public interface IProcessor
    {
        IEnumerable<string> Process(IEnumerable<ArgosTransmission> transmissions);
    }
}
