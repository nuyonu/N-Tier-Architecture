﻿using N_Tier.Application.Models.User;
using System;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface IUserService
    {
        Task<Guid> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);
        Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);

        Task<Guid> CreateAsync(CreateUserModel createUserModel);

        Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel);
    }
}
