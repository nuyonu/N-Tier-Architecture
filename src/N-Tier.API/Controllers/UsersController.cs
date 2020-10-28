using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
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
        public async Task<ActionResult> RegisterAsync(CreateUserModel createUserModel)
        {
            return Ok(ApiResult<Guid>.Success(201, await _userService.CreateAsync(createUserModel)));
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(LoginUserModel loginUserModel)
        {
            return Ok(ApiResult<LoginResponseModel>.Success200(await _userService.LoginAsync(loginUserModel)));
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
        {
            return Ok(ApiResult<ConfirmEmailResponseModel>.Success200(await _userService.ConfirmEmailAsync(confirmEmailModel)));
        }

        [HttpPut("{id}/changePassword")]
        public async Task<ActionResult> ChangePassword(Guid id, ChangePasswordModel changePasswordModel)
        {
            return Ok(ApiResult<Guid>.Success200(await _userService.ChangePasswordAsync(id, changePasswordModel)));
        }
    }
}
