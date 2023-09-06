using System;
using System.Net;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using N_Tier.Api.IntegrationTests.Config;
using N_Tier.Api.IntegrationTests.Config.Constants;
using N_Tier.Api.IntegrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.User;
using N_Tier.DataAccess.Identity;
using N_Tier.DataAccess.Persistence;
using Xunit;

namespace N_Tier.Api.IntegrationTests.Tests;

public class UsersEndpointTests
{
    private readonly ApiApplicationFactory<Program> _factory;

    public UsersEndpointTests()
    {
        _factory = new ApiApplicationFactory<Program>();
    }

    [Fact]
    public async Task Create_User_Should_Add_User_To_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();

        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "IntegrationTest@gmail.com")
            .With(cu => cu.Username = "IntegrationTest")
            .With(cu => cu.Password = "Password.1!")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await ResponseHelper.GetApiResultAsync<CreateUserResponseModel>(apiResponse);
        CheckResponse.Succeeded(response);
        context.Users.Should().Contain(u => u.Id == response.Result.Id.ToString());
    }

    [Fact]
    public async Task Create_User_Should_Return_BadRequest_If_The_Email_Is_Incorrect()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "BadEmail")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Create_User_Should_Return_BadRequest_If_The_Username_Is_Incorrect()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "nuyonu@gmail.com")
            .With(cu => cu.Username = "Len")
            .With(cu => cu.Password = "Password.1!")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Create_User_Should_Return_BadRequest_If_The_Password_Is_Incorrect()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var createModel = Builder<CreateUserModel>.CreateNew()
            .With(cu => cu.Email = "nuyonu@gmail.com")
            .With(cu => cu.Password = "incorrect")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Login_Should_Return_User_Information_And_Token()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var loginUserModel = Builder<LoginUserModel>.CreateNew()
            .With(cu => cu.Username = UserConstants.DefaultUserDb.Username)
            .With(cu => cu.Password = UserConstants.DefaultUserDb.Password)
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

        // Assert
        var response = await ResponseHelper.GetApiResultAsync<LoginResponseModel>(apiResponse);
        CheckResponse.Succeeded(response);
        response.Result.Username.Should().Be(UserConstants.DefaultUserDb.Username);
        response.Result.Email.Should().Be(UserConstants.DefaultUserDb.Email);
        response.Result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_Should_Return_NotFoundException_If_Username_Does_Not_Exists_In_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var loginUserModel = Builder<LoginUserModel>.CreateNew()
            .With(cu => cu.Username = "NotExist")
            .With(cu => cu.Password = "Password.1!")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Login_Should_Return_BadRequest_If_Password_Is_Incorrect()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var loginUserModel = Builder<LoginUserModel>.CreateNew()
            .With(cu => cu.Username = UserConstants.DefaultUserDb.Username)
            .With(cu => cu.Password = "Password.1")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task ConfirmEmail_Should_Update_User_Status()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ConfirmEmailUser")
            .With(u => u.Email = "ConfirmEmailUser@email.com")
            .Build();

        var userManager = _factory.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew()
            .With(ce => ce.UserId = user.Id)
            .With(ce => ce.Token = token)
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users/confirmEmail", new JsonContent(confirmEmailModel));

        // Assert
        var response = await ResponseHelper.GetApiResultAsync<ConfirmEmailResponseModel>(apiResponse);
        var userFromDatabase = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        CheckResponse.Succeeded(response);
        response.Result.Confirmed.Should().BeTrue();
        userFromDatabase.EmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmEmail_Should_Return_UnprocessableEntity_If_Token_Or_UserId_Are_Incorrect()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ConfirmEmailUser2")
            .With(u => u.Email = "ConfirmEmailUser2@email.com")
            .Build();

        var userManager = _factory.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew()
            .With(ce => ce.UserId = user.Id)
            .With(ce => ce.Token = "InvalidToken")
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/users/confirmEmail", new JsonContent(confirmEmailModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task ChangePassword_Should_Return_NotFoundException_If_User_Does_Not_Exists_In_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "Password.12!")
            .Build();

        // Act
        var apiResponse = await client.PutAsync($"/api/users/{Guid.NewGuid()}/changePassword",
            new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task ChangePassword_Should_Return_BadRequest_If_OldPassword_Does_Not_Match_User_Password()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest")
            .With(u => u.Email = "ChangePasswordBadRequest@email.com")
            .Build();

        var userManager = _factory.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.12!")
            .With(cu => cu.NewPassword = "Password.1!")
            .Build();

        // Act
        var apiResponse =
            await client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task ChangePassword_Should_Return_BadRequest_If_NewPassword_Does_Not_Follow_The_Rules()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest2")
            .With(u => u.Email = "ChangePasswordBadRequest2@email.com")
            .Build();

        var userManager = _factory.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "string")
            .Build();

        // Act
        var apiResponse =
            await client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await ResponseHelper.GetApiResultAsync<string>(apiResponse);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task ChangePassword_Should_Update_User_Password_If_OldPassword_And_NewPassword_Are_Ok()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var user = Builder<ApplicationUser>.CreateNew()
            .With(u => u.UserName = "ChangePasswordBadRequest3")
            .With(u => u.Email = "ChangePasswordBadRequest3@email.com")
            .With(u => u.EmailConfirmed = true)
            .Build();

        var userManager = _factory.GetRequiredService<UserManager<ApplicationUser>>();

        await userManager.CreateAsync(user, "Password.1!");

        var changePasswordModel = Builder<ChangePasswordModel>.CreateNew()
            .With(cu => cu.OldPassword = "Password.1!")
            .With(cu => cu.NewPassword = "Password.12!")
            .Build();

        // Act
        var apiResponse =
            await client.PutAsync($"/api/users/{user.Id}/changePassword", new JsonContent(changePasswordModel));

        // Assert
        apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await ResponseHelper.GetApiResultAsync<BaseResponseModel>(apiResponse);
        CheckResponse.Succeeded(response);
        response.Result.Id.Should().Be(user.Id);
    }
}
