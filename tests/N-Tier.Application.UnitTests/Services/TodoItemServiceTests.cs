using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services.Impl;
using N_Tier.Core.Entities;
using NSubstitute;
using Xunit;

namespace N_Tier.Application.UnitTests.Services;

public class TodoItemServiceTests : BaseServiceTestConfiguration
{
    private readonly TodoItemService _sut;

    public TodoItemServiceTests()
    {
        _sut = new TodoItemService(TodoItemRepository, TodoListRepository, Mapper);
    }

    [Fact]
    public async Task GetAllByListIdAsync_Should_Return_All_Todo_Items_From_A_List()
    {
        // Arrange
        var todoItems = Builder<TodoItem>.CreateListOfSize(10).Build().ToList();

        TodoItemRepository.GetAllAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).Returns(todoItems);

        // Act
        var result = await _sut.GetAllByListIdAsync(Guid.NewGuid());

        // Assert
        result.Should().HaveCount(10);
        await TodoItemRepository.Received().GetAllAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
    }

    [Fact]
    public async Task CreateAsync_Should_Add_New_Entity_To_Database()
    {
        // Arrange
        var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew().Build();
        var todoList = Builder<TodoList>.CreateNew().Build();
        var todoItem = Builder<TodoItem>.CreateNew().With(ti => ti.Id = Guid.NewGuid()).Build();

        TodoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
        TodoItemRepository.AddAsync(Arg.Any<TodoItem>()).Returns(todoItem);

        // Act
        var result = await _sut.CreateAsync(createTodoItemModel);

        // Assert
        result.Id.Should().Be(todoItem.Id);
        await TodoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        await TodoItemRepository.Received().AddAsync(Arg.Any<TodoItem>());
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Entity_From_Database()
    {
        // Arrange
        var todoItem = Builder<TodoItem>.CreateNew().With(c => c.Id = Guid.NewGuid()).Build();
        TodoItemRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).Returns(todoItem);
        TodoItemRepository.DeleteAsync(Arg.Any<TodoItem>()).Returns(todoItem);

        // Act
        var result = await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        result.Id.Should().Be(todoItem.Id);
        await TodoItemRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
        await TodoItemRepository.Received().DeleteAsync(Arg.Any<TodoItem>());
    }
}
