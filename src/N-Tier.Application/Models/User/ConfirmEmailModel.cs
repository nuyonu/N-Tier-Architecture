namespace N_Tier.Application.Models.User;

public class ConfirmEmailModel
{
    public string UserId { get; set; }

    public string Token { get; set; }
}

public class ConfirmEmailResponseModel
{
    public bool Confirmed { get; set; }
}
