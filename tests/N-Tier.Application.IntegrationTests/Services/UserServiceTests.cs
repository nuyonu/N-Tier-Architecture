using Microsoft.AspNetCore.Identity;
using N_Tier.Application.Services.Impl;
using N_Tier.DataAccess.Identity;
using NSubstitute;

namespace N_Tier.Application.IntegrationTests.Services
{
    public class UserServiceTests : BaseServiceTest
    {
        private readonly UserService _sut;

        public UserServiceTests()
        {
            var userManager = new UserManager<ApplicationUser>()
            _sut = new UserService(_mapper, _userManager, _signInManager, _configuration, _templateService, _emailService);
        }
    }
}
