using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N_Tier.Api.IntergrationTests.Config;
using N_Tier.Api.IntergrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Api.IntergrationTests.Tests
{
    [TestFixture]
    public class TodoListEndpointTests : BaseOneTimeSetup
    {
        //private IHost _host;
        //private HttpClient _client;

        [Test]
        public async Task Create_Should_Add_TodoList_In_Database()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var host = _host;

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var createTodoListModel = Builder<CreateTodoListModel>.CreateNew().Build();

            // Act
            var apiResponse = await _client.PostAsync("/api/TodoLists", new JsonContent(createTodoListModel));

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<Guid>>(await apiResponse.Content.ReadAsStringAsync());
            var todoListFromDatabase = await context.TodoLists.Where(u => u.Id == response.Result).FirstOrDefaultAsync();
            CheckResponse.Succeded(response, 201);
            todoListFromDatabase.Should().NotBeNull();
            todoListFromDatabase.Title.Should().Be(createTodoListModel.Title);
        }

        [Test]
        public async Task Create_Should_Return_BadRequest_If_Title_Is_Incorrect()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var createTodoListModel = Builder<CreateTodoListModel>.CreateNew().With(ctl => ctl.Title = "1").Build();

            // Act
            var apiResponse = await _client.PostAsync("/api/TodoLists", new JsonContent(createTodoListModel));

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            var todoListFromDatabase = await context.TodoLists.Where(tl => tl.Title == createTodoListModel.Title).FirstOrDefaultAsync();
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            CheckResponse.Failure(response, 400);
            todoListFromDatabase.Should().BeNull();
        }

        [Test]
        public async Task Update_Should_Update_Todo_List_From_Database()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            var user = await context.Users.Where(u => u.Email == "nuyonu@gmail.com").FirstOrDefaultAsync();

            var todoListFromDatabase = context.TodoLists.Add(Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid()).With(tl => tl.CreatedBy = user.Id).Build()).Entity;

            context.SaveChanges();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().With(utl => utl.Title = "UpdateTodoListTitleIntegration").Build();

            // Act
            var apiResponse = await _client.PutAsync($"/api/TodoLists/{todoListFromDatabase.Id}", new JsonContent(updateTodoListModel));

            // Assert
            context = (await GetNewHostAsync()).Services.GetRequiredService<DatabaseContext>();
            var response = JsonConvert.DeserializeObject<ApiResult<Guid>>(await apiResponse.Content.ReadAsStringAsync());
            var updatedTodoListFromDatabase = await context.TodoLists.Where(tl => tl.Id == response.Result).FirstOrDefaultAsync();
            CheckResponse.Succeded(response);
            updatedTodoListFromDatabase.Should().NotBeNull();
            updatedTodoListFromDatabase.Title.Should().Be(updateTodoListModel.Title);
        }

        [Test]
        public async Task Update_Should_Return_NotFound_If_Todo_List_Does_Not_Exist_Anymore()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().With(utl => utl.Title = "UpdateTodoListIntegration").Build();

            // Act
            var apiResponse = await _client.PutAsync($"/api/TodoLists/{Guid.NewGuid()}", new JsonContent(updateTodoListModel));

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            var updatedTodoListFromDatabase = await context.TodoLists.Where(tl => tl.Title == updateTodoListModel.Title).FirstOrDefaultAsync();
            apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            CheckResponse.Failure(response, 404);
            updatedTodoListFromDatabase.Should().BeNull();
        }

        [Test]
        public async Task Update_Should_Return_BadRequest_If_Todo_List_Does_Not_Belong_To_User()
        {
            // Arrange  
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            var todoListFromDatabase = context.TodoLists.Add(Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid()).Build()).Entity;

            context.SaveChanges();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().Build();

            // Act
            var apiResponse = await _client.PutAsync($"/api/TodoLists/{todoListFromDatabase.Id}", new JsonContent(updateTodoListModel));

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            var updatedTodoListFromDatabase = await context.TodoLists.Where(tl => tl.Title == updateTodoListModel.Title).FirstOrDefaultAsync();
            apiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            CheckResponse.Failure(response, 400);
            updatedTodoListFromDatabase.Should().NotBeNull();
            updatedTodoListFromDatabase.Title.Should().Be(todoListFromDatabase.Title);
        }

        [Test]
        public async Task Delete_Should_Delete_Todo_List_From_Database()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            var user = await context.Users.Where(u => u.Email == "nuyonu@gmail.com").FirstOrDefaultAsync();

            var todoListFromDatabase = context.TodoLists.Add(Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid()).With(tl => tl.CreatedBy = user.Id).Build()).Entity;

            context.SaveChanges();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().Build();

            // Act
            var apiResponse = await _client.DeleteAsync($"/api/TodoLists/{todoListFromDatabase.Id}");

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<Guid>>(await apiResponse.Content.ReadAsStringAsync());
            var updatedTodoListFromDatabase = await context.TodoLists.Where(tl => tl.Id == response.Result).FirstOrDefaultAsync();
            CheckResponse.Succeded(response);
            updatedTodoListFromDatabase.Should().BeNull();
        }

        [Test]
        public async Task Delete_Should_Return_NotFound_If_Todo_List_Does_Not_Exist_Anymore()
        {
            // Arrange
            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            // Act
            var apiResponse = await _client.DeleteAsync($"/api/TodoLists/{Guid.NewGuid()}");

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
            apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            CheckResponse.Failure(response, 404);
        }

        [Test]
        public async Task Get_Todo_Lists_Should_Return_All_Todo_Lists_For_Specified_User_From_Database()
        {
            // Arrange
            //var _host = await SingletonConfig.Get_hostInstanceAsync();

            var context = _host.Services.GetRequiredService<DatabaseContext>();

            var user = await context.Users.Where(u => u.Email == "nuyonu@gmail.com").FirstOrDefaultAsync();

            context.TodoLists.RemoveRange(context.TodoLists.ToList());

            var todoLists = Builder<TodoList>.CreateListOfSize(10).All().With(tl => tl.Id = Guid.NewGuid()).With(tl => tl.CreatedBy = user.Id).Build();
            var todoListsNotBelongToTheUser = Builder<TodoList>.CreateListOfSize(10).All()
                                                .With(tl => tl.Id = Guid.NewGuid()).With(tl => tl.CreatedBy = Guid.NewGuid().ToString()).Build();

            context.TodoLists.AddRange(todoLists);
            context.TodoLists.AddRange(todoListsNotBelongToTheUser);

            context.SaveChanges();

            //var _client = await SingletonConfig.GetAuthenticated_clientInstanceAsync();

            // Act
            var apiResponse = await _client.GetAsync($"/api/TodoLists");

            // Assert
            var response = JsonConvert.DeserializeObject<ApiResult<IEnumerable<TodoListResponseModel>>>(await apiResponse.Content.ReadAsStringAsync());
            CheckResponse.Succeded(response);
            response.Result.Should().HaveCount(10);
        }
    }
}
