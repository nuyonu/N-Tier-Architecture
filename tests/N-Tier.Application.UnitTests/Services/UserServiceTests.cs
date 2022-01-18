using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MockQueryable.NSubstitute;
using N_Tier.Application.Common.Email;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.User;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;
using N_Tier.DataAccess.Identity;
using NSubstitute;
using Xunit;

namespace N_Tier.Application.UnitTests.Services;

public class UserServiceTests : BaseServiceTestConfiguration
{
    private readonly IEmailService _emailService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserService _sut;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserServiceTests()
    {
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        _userManager =
            Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManager = Substitute.For<SignInManager<ApplicationUser>>(_userManager, contextAccessor,
            userPrincipalFactory, null, null, null, null);
        _templateService = Substitute.For<ITemplateService>();
        _emailService = Substitute.For<IEmailService>();

        _sut = new UserService(Mapper, _userManager, _signInManager, Configuration, _templateService,
            _emailService);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_User_To_Database_And_Send_Confirmation_Email()
    {
        // Arrange
        var createUserModel = Builder<CreateUserModel>.CreateNew().Build();
        var applicationUser = Builder<ApplicationUser>.CreateNew().Build();
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _userManager.FindByNameAsync(Arg.Any<string>()).Returns(applicationUser);

        // Act
        var result = await _sut.CreateAsync(createUserModel);

        // Assert
        result.Id.Should().Be(applicationUser.Id);
        await _userManager.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        await _userManager.Received(1).GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>());
        await _templateService.Received(1).GetTemplateAsync(Arg.Any<string>());
        await _emailService.Received(1).SendEmailAsync(Arg.Any<EmailMessage>());
        await _userManager.Received(1).FindByNameAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_Exception_If_CreateAsync_Return_Failure()
    {
        // Arrange
        var createUserModel = Builder<CreateUserModel>.CreateNew().Build();
        var identityErrors = Builder<IdentityError>.CreateListOfSize(5).Build().ToArray();
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(identityErrors));

        // Act
        Func<Task> callCreateAsync = async () => await _sut.CreateAsync(createUserModel);

        // Assert
        await callCreateAsync.Should().ThrowAsync<BadRequestException>()
            .WithMessage(identityErrors.FirstOrDefault()?.Description);
        await _userManager.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_Exception_If_User_Does_Not_Exist()
    {
        // Arrange
        var loginUserModel = Builder<LoginUserModel>.CreateNew().Build();
        var emptyUserListQueryable = new List<ApplicationUser>().AsQueryable().BuildMock();
        _userManager.Users.Returns(emptyUserListQueryable);

        // Act
        Func<Task> callCreateAsync = async () => await _sut.LoginAsync(loginUserModel);

        // Assert
        await callCreateAsync.Should().ThrowAsync<NotFoundException>().WithMessage("Username or password is incorrect");
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_Exception_If_User_Does_Not_Provide_Good_Credentials()
    {
        // Arrange
        var loginUserModel = Builder<LoginUserModel>.CreateNew().Build();
        var usersListQueryable = Builder<ApplicationUser>.CreateListOfSize(1)
            .All().With(u => u.UserName = loginUserModel.Username)
            .Build().AsQueryable().BuildMock();
        _userManager.Users.Returns(usersListQueryable);
        _signInManager
            .PasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(SignInResult.Failed);

        // Act
        Func<Task> callCreateAsync = async () => await _sut.LoginAsync(loginUserModel);

        // Assert
        await callCreateAsync.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Username or password is incorrect");
        await _signInManager.Received(1).PasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(),
            Arg.Any<bool>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Login_Response_If_Credentials_Are_Good()
    {
        // Arrange
        var loginUserModel = Builder<LoginUserModel>.CreateNew().Build();
        var usersList = Builder<ApplicationUser>.CreateListOfSize(1).All()
            .With(u => u.UserName = loginUserModel.Username).Build();
        var usersListQueryable = usersList.AsQueryable().BuildMock();
        _userManager.Users.Returns(usersListQueryable);
        _signInManager
            .PasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(SignInResult.Success);

        // Act
        var result = await _sut.LoginAsync(loginUserModel);

        // Assert
        result.Username.Should().Be(usersList[0].UserName);
        result.Email.Should().Be(usersList[0].Email);
        result.Token.Should().NotBeNullOrEmpty();
        await _signInManager.Received(1).PasswordSignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(),
            Arg.Any<bool>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task ConfirmEmailAsync_Should_Throw_Exception_If_Token_Or_UserId_Are_Not_Ok()
    {
        // Arrange
        var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew().Build();
        var identityErrors = Builder<IdentityError>.CreateListOfSize(5).Build().ToArray();
        _userManager.ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(identityErrors));

        // Act
        Func<Task> callConfirmEmailAsync = async () => await _sut.ConfirmEmailAsync(confirmEmailModel);

        // Assert
        await callConfirmEmailAsync.Should().ThrowAsync<UnprocessableRequestException>()
            .WithMessage("Your verification link has expired");
        await _userManager.Received(1).FindByIdAsync(Arg.Any<string>());
        await _userManager.Received(1).ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ConfirmEmailAsync_Should_Return_Success_True_If_Token_And_UserId_Are_Ok()
    {
        // Arrange
        var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew().Build();
        _userManager.ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);

        // Act
        var result = await _sut.ConfirmEmailAsync(confirmEmailModel);

        // Assert
        result.Should().NotBeNull();
        result.Confirmed.Should().BeTrue();
        await _userManager.Received(1).FindByIdAsync(Arg.Any<string>());
        await _userManager.Received(1).ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
    }
}
