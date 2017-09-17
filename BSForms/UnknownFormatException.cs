using System;
using System.Runtime.Serialization;

namespace BSForms
{
    [Serializable]
    internal class UnknownFormatException : Exception
    {
        public UnknownFormatException()
        {
        }

        public UnknownFormatException(string message) : base(message)
        {
        }

        public UnknownFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}