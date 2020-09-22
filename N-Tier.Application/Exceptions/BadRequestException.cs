using System;

namespace N_Tier.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        { }

        public BadRequestException(string message) : base(message)
        { }
    }
}
