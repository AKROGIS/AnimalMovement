using System;
using System.Runtime.Serialization;

namespace Telonics
{
    public class NoMessagesException : Exception
    {
        public NoMessagesException()
        {
        }

        public NoMessagesException(string message) : base(message)
        {
        }

        public NoMessagesException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected NoMessagesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
