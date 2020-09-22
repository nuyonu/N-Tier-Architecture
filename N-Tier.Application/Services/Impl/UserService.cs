using AutoMapper;
using Microsoft.AspNetCore.Identity;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.User;
using N_Tier.Infrastructure.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Guid> CreateAsync(CreateUserModel createUserModel)
        {
            var user = _mapper.Map<ApplicationUser>(createUserModel);

            var result = await _userManager.CreateAsync(user, createUserModel.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description);
            }

            return Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id);
        }
    }
}
