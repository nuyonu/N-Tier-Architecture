using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using N_Tier.Api.IntegrationTests.Config;
using N_Tier.Api.IntegrationTests.Config.Constants;
using N_Tier.Api.IntegrationTests.Helpers;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using Newtonsoft.Json;
using Xunit;

namespace N_Tier.Api.IntegrationTests.Tests;

public class TodoItemEndpointTests
{
    private readonly ApiApplicationFactory<Program> _factory;

    public TodoItemEndpointTests()
    {
        _factory = new ApiApplicationFactory<Program>();
    }

    [Fact]
    public async Task Create_Should_Add_TodoItem_In_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();

        var user = await context.Users.Where(u => u.Email == UserConstants.DefaultUserDb.Email).FirstOrDefaultAsync();

        var todoListFromDatabase = Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = user.Id).Build();

        await context.TodoLists.AddAsync(todoListFromDatabase);

        await context.SaveChangesAsync();

        var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew()
            .With(cti => cti.TodoListId = todoListFromDatabase.Id).Build();

        // Act
        var apiResponse = await client.PostAsync("/api/todoItems", new JsonContent(createTodoItemModel));

        // Assert
        var response =
            JsonConvert.DeserializeObject<ApiResult<CreateTodoItemResponseModel>>(
                await apiResponse.Content.ReadAsStringAsync());
        var todoItemFromDatabase =
            await context.TodoItems.Where(ti => ti.Id == response.Result.Id).FirstOrDefaultAsync();
        CheckResponse.Succeeded(response);
        todoItemFromDatabase.Should().NotBeNull();
        todoItemFromDatabase.Title.Should().Be(createTodoItemModel.Title);
        todoItemFromDatabase.List.Id.Should().Be(todoListFromDatabase.Id);
    }

    [Fact]
    public async Task Create_Should_Return_Not_Found_If_Todo_List_Does_Not_Exist_Anymore()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();

        var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew().With(cti => cti.TodoListId = Guid.NewGuid())
            .Build();

        // Act
        var apiResponse = await client.PostAsync("/api/todoItems", new JsonContent(createTodoItemModel));

        // Assert
        var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Update_Should_Update_Todo_Item_From_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();
    
        var user = await context.Users.Where(u => u.Email == UserConstants.DefaultUserDb.Email).FirstOrDefaultAsync();
    
        var todoListFromDatabase = Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = user.Id).Build();
    
        var todoItemFromDatabase = Builder<TodoItem>.CreateNew().With(ti => ti.Id = Guid.NewGuid())
            .With(ti => ti.CreatedBy = user.Id).Build();
    
        todoListFromDatabase.Items.Add(todoItemFromDatabase);
    
        await context.TodoLists.AddAsync(todoListFromDatabase);
    
        await context.SaveChangesAsync();
    
        var updateTodoItemModel = Builder<UpdateTodoItemModel>.CreateNew()
            .With(cti => cti.TodoListId = todoListFromDatabase.Id)
            .With(cti => cti.Title = "UpdateTodoItemTitle")
            .With(cti => cti.Body = "UpdateTodoItemBody").Build();
    
        // Act
        var apiResponse = await client.PutAsync($"/api/todoItems/{todoItemFromDatabase.Id}",
            new JsonContent(updateTodoItemModel));
    
        // Assert
        context = _factory.GetRequiredService<DatabaseContext>();
        var response =
            JsonConvert.DeserializeObject<ApiResult<CreateTodoItemResponseModel>>(
                await apiResponse.Content.ReadAsStringAsync());
        var modifiedTodoItemFromDatabase =
            await context.TodoItems.Where(ti => ti.Id == response.Result.Id).FirstOrDefaultAsync();
        CheckResponse.Succeeded(response);
        modifiedTodoItemFromDatabase.Should().NotBeNull();
        modifiedTodoItemFromDatabase.Title.Should().Be(updateTodoItemModel.Title);
        modifiedTodoItemFromDatabase.Body.Should().Be(updateTodoItemModel.Body);
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_If_Todo_List_Does_Not_Exist_Anymore()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var updateTodoItemModel = Builder<UpdateTodoItemModel>.CreateNew().Build();

        // Act
        var apiResponse =
            await client.PutAsync($"/api/todoItems/{Guid.NewGuid()}", new JsonContent(updateTodoItemModel));

        // Assert
        var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Update_Should_Return_NotFound_If_Todo_Item_Does_Not_Exist_Anymore()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();

        var user = await context.Users.Where(u => u.Email == UserConstants.DefaultUserDb.Email).FirstOrDefaultAsync();

        var todoListFromDatabase = Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = user.Id).Build();

        await context.TodoLists.AddAsync(todoListFromDatabase);

        await context.SaveChangesAsync();

        var updateTodoItemModel = Builder<UpdateTodoItemModel>.CreateNew()
            .With(cti => cti.TodoListId = todoListFromDatabase.Id).Build();

        // Act
        var apiResponse =
            await client.PutAsync($"/api/todoItems/{Guid.NewGuid()}", new JsonContent(updateTodoItemModel));

        // Assert
        var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task Delete_Should_Delete_Todo_Item_From_Database()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();

        var user = await context.Users.Where(u => u.Email == UserConstants.DefaultUserDb.Email).FirstOrDefaultAsync();

        var todoItemFromDatabase = Builder<TodoItem>.CreateNew().With(ti => ti.Id = Guid.NewGuid())
            .With(ti => ti.CreatedBy = user.Id).Build();

        var todoListFromDatabase = Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = user.Id).Build();

        todoListFromDatabase.Items.Add(todoItemFromDatabase);

        await context.TodoLists.AddAsync(todoListFromDatabase);

        await context.SaveChangesAsync();

        // Act
        var apiResponse = await client.DeleteAsync($"/api/todoItems/{todoItemFromDatabase.Id}");

        // Assert
        var response =
            JsonConvert.DeserializeObject<ApiResult<BaseResponseModel>>(await apiResponse.Content.ReadAsStringAsync());
        var deletedTodoListFromDatabase =
            await context.TodoItems.Where(ti => ti.Id == response.Result.Id).FirstOrDefaultAsync();
        CheckResponse.Succeeded(response);
        deletedTodoListFromDatabase.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_If_Item_Does_Not_Exist_Anymore()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();

        // Act
        var apiResponse = await client.DeleteAsync($"/api/todoItems/{Guid.NewGuid()}");

        // Assert
        var response = JsonConvert.DeserializeObject<ApiResult<string>>(await apiResponse.Content.ReadAsStringAsync());
        apiResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        CheckResponse.Failure(response);
    }

    [Fact]
    public async Task GetAllByListId_Should_Return_All_Todo_Items_From_Specific_List()
    {
        // Arrange
        var client = await _factory.CreateDefaultClientAsync();
        var context = _factory.GetRequiredService<DatabaseContext>();

        var user = await context.Users.Where(u => u.Email == UserConstants.DefaultUserDb.Email).FirstOrDefaultAsync();

        var todoListFromDatabase = Builder<TodoList>.CreateNew().With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = user.Id).Build();

        todoListFromDatabase.Items.AddRange(Builder<TodoItem>.CreateListOfSize(25).All()
            .With(ti => ti.Id = Guid.NewGuid()).Build());

        var todoListFromAnotherUsers = Builder<TodoList>.CreateListOfSize(10).All()
            .With(tl => tl.Id = Guid.NewGuid())
            .With(tl => tl.CreatedBy = Guid.NewGuid().ToString())
            .Build();

        foreach (var todoList in todoListFromAnotherUsers)
            todoList.Items.AddRange(Builder<TodoItem>.CreateListOfSize(10).All().With(ti => ti.Id = Guid.NewGuid())
                .Build());

        await context.TodoLists.AddAsync(todoListFromDatabase);
        await context.TodoLists.AddRangeAsync(todoListFromAnotherUsers);

        await context.SaveChangesAsync();

        // Act
        var apiResponse = await client.GetAsync($"/api/todoLists/{todoListFromDatabase.Id}/todoItems");

        // Assert
        var response =
            JsonConvert.DeserializeObject<ApiResult<IEnumerable<TodoItemResponseModel>>>(
                await apiResponse.Content.ReadAsStringAsync());
        CheckResponse.Succeeded(response);
        response.Result.Should().NotBeNullOrEmpty();
        response.Result.Should().HaveCount(25);
        response.Result.Should().BeEquivalentTo(todoListFromDatabase.Items, options => options.Including(tl => tl.Id));
    }
}
