using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models.User;
using N_Tier.Application.Services;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Guid>> RegisterAsync(CreateUserModel createUserModel)
        {
            return Ok(await _userService.CreateAsync(createUserModel));
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> LoginAsync(LoginUserModel loginUserModel)
        {
            return Ok(await _userService.LoginAsync(loginUserModel));
        }
    }
}
