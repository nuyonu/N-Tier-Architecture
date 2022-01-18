using System.Runtime.Serialization;

namespace N_Tier.Core.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() { }

        public ResourceNotFoundException(Type type) : base($"{type} is missing") { }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ResourceNotFoundException(string? message) : base(message) { }

        public ResourceNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
