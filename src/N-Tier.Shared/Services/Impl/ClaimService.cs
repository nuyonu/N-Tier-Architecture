using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace N_Tier.Shared.Services.Impl
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId() 
            => GetClaim(ClaimTypes.NameIdentifier);

        public string GetClaim(string key) 
            => _httpContextAccessor.HttpContext?.User?.FindFirst(key).Value;
    }
}
