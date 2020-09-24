using Microsoft.AspNetCore.Http;
using N_Tier.Infrastructure.Services;
using System.Security.Claims;

namespace N_Tier.Application.Services.Impl
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        public string GetClaim(string key)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(key);
        }
    }
}
