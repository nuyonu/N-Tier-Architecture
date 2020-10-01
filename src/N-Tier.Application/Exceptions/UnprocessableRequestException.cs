using System;

namespace N_Tier.Application.Exceptions
{
    public class UnprocessableRequestException : Exception
    {
        public UnprocessableRequestException()
        { }

        public UnprocessableRequestException(string message) : base(message)
        { }
    }
}
