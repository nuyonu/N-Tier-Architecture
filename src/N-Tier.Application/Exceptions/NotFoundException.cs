using System;
using System.Runtime.Serialization;

namespace N_Tier.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        { }

        public NotFoundException(string message) : base(message)
        { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
