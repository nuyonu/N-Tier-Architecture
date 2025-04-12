namespace N_Tier.Application.Exceptions;

[Serializable]
public class BadRequestException(string message) : Exception(message);
