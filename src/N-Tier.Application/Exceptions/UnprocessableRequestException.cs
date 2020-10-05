using System;
using System.Runtime.Serialization;

namespace N_Tier.Application.Exceptions
{
    [Serializable]
    public class UnprocessableRequestException : Exception
    {
        public UnprocessableRequestException()
        { }

        public UnprocessableRequestException(string message) : base(message)
        { }

        public UnprocessableRequestException(string message, Exception innerException) : base(message, innerException)
        { }

        protected UnprocessableRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
