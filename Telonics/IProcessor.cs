using System.Collections.Generic;

namespace Telonics
{
    public interface IProcessor
    {
        IEnumerable<string> ProcessAws(string fileContents);
        IEnumerable<string> ProcessEmail(IEnumerable<ArgosTransmission> transmissions);
    }
}
