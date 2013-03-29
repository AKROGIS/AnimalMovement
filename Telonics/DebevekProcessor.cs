using System.Collections.Generic;

namespace Telonics
{
    public class DebevekProcessor : IProcessor
    {
        public IEnumerable<string> ProcessTransmissions(IEnumerable<ArgosTransmission> transmissions, ArgosFile file)
        {
            string newHeader = "PlatformId" + file.Header.Substring(8); //case insensitive replace of "CollarId"
            yield return newHeader;
            foreach (var transmission in transmissions)
            {
                yield return transmission.ToString();
            }
        }
    }
}
