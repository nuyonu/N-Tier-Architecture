using System.Runtime.Serialization;

namespace N_Tier.Application.Exceptions;

[Serializable]
public class UnprocessableRequestException : Exception
{
    public UnprocessableRequestException(string message) : base(message) { }
    protected UnprocessableRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
