namespace N_Tier.Infrastructure.Services
{
    public interface IClaimService
    {
        string GetUserId();
        string GetClaim(string key);
    }
}
