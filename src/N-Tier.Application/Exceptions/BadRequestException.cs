namespace N_Tier.Application.Exceptions;

[Serializable]
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
