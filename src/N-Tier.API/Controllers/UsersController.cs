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
        public async Task<IActionResult> RegisterAsync(CreateUserModel createUserModel)
        {
            return Ok(ApiResult<CreateUserResponseModel>.Success(await _userService.CreateAsync(createUserModel)));
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginUserModel loginUserModel)
        {
            return Ok(ApiResult<LoginResponseModel>.Success(await _userService.LoginAsync(loginUserModel)));
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
        {
            return Ok(ApiResult<ConfirmEmailResponseModel>.Success(await _userService.ConfirmEmailAsync(confirmEmailModel)));
        }

        [HttpPut("{id:guid}/changePassword")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordModel changePasswordModel)
        {
            return Ok(ApiResult<BaseResponseModel>.Success(await _userService.ChangePasswordAsync(id, changePasswordModel)));
        }
    }
}
