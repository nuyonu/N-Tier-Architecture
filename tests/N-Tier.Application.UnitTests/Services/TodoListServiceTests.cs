using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services.Impl;
using N_Tier.Core.Entities;
using NSubstitute;
using Xunit;

namespace N_Tier.Application.UnitTests.Services;

public class TodoListServiceTests : BaseServiceTestConfiguration
{
    private readonly TodoListService _sut;

    public TodoListServiceTests()
    {
        _sut = new TodoListService(TodoListRepository, Mapper, ClaimService);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Todo_Lists()
    {
        //Arrange
        var todoLists = Builder<TodoList>.CreateListOfSize(10).Build().ToList();

        TodoListRepository.GetAllAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoLists);

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().HaveCount(10);
        ClaimService.Received().GetUserId();
        await TodoListRepository.Received().GetAllAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
    }

    [Fact]
    public async Task CreateAsync_Should_Add_New_Entity_To_Database()
    {
        //Arrange
        var createTodoListModel = Builder<CreateTodoListModel>.CreateNew().Build();
        var todoList = Mapper.Map<TodoList>(createTodoListModel);
        todoList.Id = Guid.NewGuid();

        TodoListRepository.AddAsync(Arg.Any<TodoList>()).Returns(todoList);

        //Act
        var result = await _sut.CreateAsync(createTodoListModel);

        //Assert
        result.Id.Should().Be(todoList.Id);
        await TodoListRepository.Received().AddAsync(Arg.Any<TodoList>());
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_Exception_If_Todo_List_Does_Not_Belong_To_HimAsync()
    {
        //Arrange
        var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().Build();
        var todoList = Builder<TodoList>.CreateNew().Build();

        TodoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);

        //Act
        Func<Task> callUpdateAsync = async () => await _sut.UpdateAsync(Guid.NewGuid(), updateTodoListModel);

        //Assert
        await callUpdateAsync.Should().ThrowAsync<BadRequestException>()
            .WithMessage("The selected list does not belong to you");
        await TodoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        ClaimService.Received().GetUserId();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_Entity_From_DatabaseAsync()
    {
        //Arrange
        var updateTodoListModel = Builder<UpdateTodoListModel>.CreateNew().Build();
        var todoListId = Guid.NewGuid();
        var todoList = Builder<TodoList>.CreateNew()
            .With(tl => tl.CreatedBy = new Guid().ToString())
            .With(tl => tl.Id = todoListId)
            .Build();

        TodoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
        TodoListRepository.UpdateAsync(Arg.Any<TodoList>()).Returns(todoList);

        //Act
        var result = await _sut.UpdateAsync(todoListId, updateTodoListModel);

        //Assert
        result.Id.Should().Be(todoListId);
        await TodoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        ClaimService.Received().GetUserId();
        await TodoListRepository.Received().UpdateAsync(Arg.Any<TodoList>());
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Entity_From_Database()
    {
        //Arrange
        var todoListId = Guid.NewGuid();
        var todoList = Builder<TodoList>.CreateNew()
            .With(tl => tl.CreatedBy = new Guid().ToString())
            .With(tl => tl.Id = todoListId)
            .Build();

        TodoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
        TodoListRepository.DeleteAsync(Arg.Any<TodoList>()).Returns(todoList);

        //Act
        var result = await _sut.DeleteAsync(Guid.NewGuid());

        //Assert
        result.Id.Should().Be(todoListId);
        await TodoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        await TodoListRepository.Received().DeleteAsync(Arg.Any<TodoList>());
    }
}
