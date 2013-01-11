using System;
using System.Text;

namespace Telonics
{
    public class ArgosWebResult
    {
        private readonly string _text;

        public ArgosWebResult(string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");
            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }

        public Byte[] ToBytes()
        {
            var e = new UTF8Encoding();
            return e.GetBytes(_text);
        }
    }
}
