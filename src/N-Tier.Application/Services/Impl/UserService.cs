using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using N_Tier.Application.Common.Email;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Helpers;
using N_Tier.Application.Models.User;
using N_Tier.Application.Templates;
using N_Tier.DataAccess.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITemplateService _templateService;
        private readonly IEmailService _emailService;

        public UserService(IMapper mapper,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IConfiguration configuration,
                           ITemplateService templateService,
                           IEmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _templateService = templateService;
            _emailService = emailService;
        }

        public async Task<Guid> CreateAsync(CreateUserModel createUserModel)
        {
            var user = _mapper.Map<ApplicationUser>(createUserModel);

            var result = await _userManager.CreateAsync(user, createUserModel.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

            var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
                                                               new Dictionary<string, string> { { "{UserId}", user.Id }, { "{Token}", token } });

            await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "[N-Tier]Confirm your email"));

            return Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id);
        }

        public async Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.Username);

            if (user == null)
                throw new NotFoundException("Username or password is incorrect");

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);

            if (!signInResult.Succeeded)
                throw new BadRequestException("Username or password is incorrect");

            var token = JwtHelper.GenerateToken(user, _configuration);

            return new LoginResponseModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);

            if (user == null)
                throw new UnprocessableRequestException("Your verification link is incorrect");

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);

            if (!result.Succeeded)
                throw new UnprocessableRequestException("Your verification link has expired");

            return new ConfirmEmailResponseModel
            {
                Confirmed = true
            };
        }
    }
}
