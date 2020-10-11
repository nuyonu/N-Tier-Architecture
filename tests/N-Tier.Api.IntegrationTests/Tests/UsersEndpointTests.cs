using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Api.IntegrationTests.Config;
using N_Tier.Api.IntegrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.User;
using N_Tier.DataAccess.Persistence;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace N_Tier.Api.IntegrationTests.Tests
{
    public class UsersEndpointTests
    {
        [Fact]
        public async Task Create_User_Should_Add_User_To_Database()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var context = host.Services.GetRequiredService<DatabaseContext>();

            var client = host.GetTestClient();

            var createModel = Builder<CreateUserModel>.CreateNew()
                .With(cu => cu.Email = "IntegrationTest@gmail.com")
                .With(cu => cu.Username = "IntegrationTest")
                .With(cu => cu.Password = "Password.1!")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var response = JsonConvert.DeserializeObject<ApiResult<Guid>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Succeded(response, 201);
            context.Users.Should().Contain(u => u.Id == response.Result.ToString());
        }

        [Fact]
        public async Task Create_User_Should_Return_BadRequest_If_The_Email_Is_Incorrect()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var createModel = Builder<CreateUserModel>.CreateNew()
                .With(cu => cu.Email = "BadEmail")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Failure(response, 400);
        }

        [Fact]
        public async Task Create_User_Should_Return_BadRequest_If_The_Username_Is_Incorrect()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var createModel = Builder<CreateUserModel>.CreateNew()
                .With(cu => cu.Email = "nuyonu@gmail.com")
                .With(cu => cu.Username = "Len")
                .With(cu => cu.Password = "Password.1!")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Failure(response, 400);
        }

        [Fact]
        public async Task Create_User_Should_Return_BadRequest_If_The_Password_Is_Incorrect()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var createModel = Builder<CreateUserModel>.CreateNew()
                .With(cu => cu.Email = "nuyonu@gmail.com")
                .With(cu => cu.Password = "incorrect")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users", new JsonContent(createModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Failure(response, 400);
        }

        [Fact]
        public async Task Login_Should_Return_User_Informations_And_Token()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var loginUserModel = Builder<LoginUserModel>.CreateNew()
                .With(cu => cu.Username = "nuyonu")
                .With(cu => cu.Password = "Password.1!")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<LoginResponseModel>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Succeded(response);
            response.Result.Username.Should().Be("nuyonu");
            response.Result.Email.Should().Be("nuyonu@gmail.com");
            response.Result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_Should_Return_NotFoundException_If_Username_Does_Not_Exists_In_Database()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var loginUserModel = Builder<LoginUserModel>.CreateNew()
                .With(cu => cu.Username = "NotExist")
                .With(cu => cu.Password = "Password.1!")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Failure(response, 404);
        }

        [Fact]
        public async Task Login_Should_Return_BadRequest_If_Password_Is_Incorrect()
        {
            // Arrange
            var host = await SingletonConfig.GetHostInstanceAsync();

            var client = host.GetTestClient();

            var loginUserModel = Builder<LoginUserModel>.CreateNew()
                .With(cu => cu.Username = "nuyonu")
                .With(cu => cu.Password = "Password.1")
                .Build();

            // Act
            var apiResponse = await client.PostAsync("/api/users/authenticate", new JsonContent(loginUserModel));

            // Assert
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Failure(response, 400);
        }
    }
}
