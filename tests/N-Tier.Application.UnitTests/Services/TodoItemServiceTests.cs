using FizzWare.NBuilder;
using FluentAssertions;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services.Impl;
using N_Tier.Core.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace N_Tier.Application.UnitTests.Services
{
    public class TodoItemServiceTests : BaseServiceTestConfiguration
    {
        private readonly TodoItemService _sut;

        public TodoItemServiceTests()
        {
            _sut = new TodoItemService(_todoItemRepository, _todoListRepository, _mapper);
        }

        [Fact]
        public async Task GetAllByListIdAsync_Should_Return_All_Todo_Items_From_A_List()
        {
            // Arrange
            var todoItems = Builder<TodoItem>.CreateListOfSize(10).Build().ToList();

            _todoItemRepository.GetAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).Returns(todoItems);

            // Act
            var result = await _sut.GetAllByListIdAsync(Guid.NewGuid());

            // Assert
            result.Should().HaveCount(10);
            await _todoItemRepository.Received().GetAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_Exception_If_List_Does_Not_Exist_Anymore()
        {
            // Arrange
            var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew().Build();

            _todoListRepository.GetAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).ReturnsNull();

            // Act
            Func <Task> callCreateAsync = async () => await _sut.CreateAsync(createTodoItemModel);

            // Assert
            callCreateAsync.Should().Throw<NotFoundException>().WithMessage("List does not exist anymore");
            await _todoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        }

        [Fact]
        public async Task CreateAsync_Should_Add_New_Entity_To_Database()
        {
            // Arrange
            var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew().Build();
            var todoList = Builder<TodoList>.CreateNew().Build();
            var todoItem = Builder<TodoItem>.CreateNew().With(ti => ti.Id = Guid.NewGuid()).Build();

            _todoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
            _todoItemRepository.AddAsync(Arg.Any<TodoItem>()).Returns(todoItem);

            // Act
            var result = await _sut.CreateAsync(createTodoItemModel);

            // Assert
            result.Id.Should().Be(todoItem.Id);
            await _todoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
            await _todoItemRepository.Received().AddAsync(Arg.Any<TodoItem>());
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_If_List_Does_Not_Exist_Anymore()
        {
            // Arrange
            var updateTodoItem = Builder<UpdateTodoItemModel>.CreateNew().Build();
            _todoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).ReturnsNull();

            // Act
            Func<Task> callCreateAsync = async () => await _sut.UpdateAsync(Guid.NewGuid(), updateTodoItem);

            // Assert
            callCreateAsync.Should().Throw<NotFoundException>().WithMessage("List does not exist anymore");
            await _todoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_If_Todo_Item_Does_Not_Exist_Anymore()
        {
            // Arrange
            var updateTodoItem = Builder<UpdateTodoItemModel>.CreateNew().Build();
            var todoList = Builder<TodoList>.CreateNew().Build();

            _todoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
            _todoItemRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).ReturnsNull();

            // Act
            Func<Task> callCreateAsync = async () => await _sut.UpdateAsync(Guid.NewGuid(), updateTodoItem);

            // Assert
            callCreateAsync.Should().Throw<NotFoundException>().WithMessage("Todo item does not exist anymore");
            await _todoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
            await _todoItemRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Existing_Entity_From_Database()
        {
            // Arrange
            var updateTodoItem = Builder<UpdateTodoItemModel>.CreateNew().Build();
            var todoList = Builder<TodoList>.CreateNew().Build();
            var todoItem = Builder<TodoItem>.CreateNew().Build();
            todoItem.Id = Guid.NewGuid();

            _todoListRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>()).Returns(todoList);
            _todoItemRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).Returns(todoItem);
            _todoItemRepository.UpdateAsync(Arg.Any<TodoItem>()).Returns(todoItem);

            // Act
            var result = await _sut.UpdateAsync(Guid.NewGuid(), updateTodoItem);

            // Assert
            result.Id.Should().Be(todoItem.Id);
            await _todoListRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoList, bool>>>());
            await _todoItemRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
            await _todoItemRepository.Received().UpdateAsync(Arg.Any<TodoItem>());
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_Exception_If_List_Does_Not_Exist_Anymore()
        {
            // Arrange
            _todoItemRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).ReturnsNull();

            // Act
            Func<Task> callDeleteAsync = async () => await _sut.DeleteAsync(Guid.NewGuid());

            // Assert
            callDeleteAsync.Should().Throw<NotFoundException>().WithMessage("Todo item does not exist anymore");
            await _todoItemRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Entity_From_Database()
        {
            // Arrange
            var todoItem = Builder<TodoItem>.CreateNew().With(c => c.Id = Guid.NewGuid()).Build();
            _todoItemRepository.GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>()).Returns(todoItem);
            _todoItemRepository.DeleteAsync(Arg.Any<TodoItem>()).Returns(todoItem);

            // Act
            var result = await _sut.DeleteAsync(Guid.NewGuid());

            // Assert
            result.Id.Should().Be(todoItem.Id);
            await _todoItemRepository.Received().GetFirstAsync(Arg.Any<Expression<Func<TodoItem, bool>>>());
            await _todoItemRepository.Received().DeleteAsync(Arg.Any<TodoItem>());
        }
    }
}
