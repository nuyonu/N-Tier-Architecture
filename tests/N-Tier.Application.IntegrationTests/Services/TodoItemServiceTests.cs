using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using N_Tier.Application.MappingProfiles;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services.Impl;
using N_Tier.DataAccess.Persistence;
using N_Tier.DataAccess.Repositories.Impl;
using N_Tier.Shared.Services;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace N_Tier.Application.IntegrationTests.Services
{
    public class TodoItemServiceTests
    {
        private readonly DatabaseContext _context;
        private readonly TodoListService _sut;

        public TodoItemServiceTests()
        {
            DbContextOptions<DatabaseContext> options;
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("Integration tests");
            options = builder.Options;

            var claimService = Substitute.For<IClaimService>();

            claimService.GetUserId().Returns(new Guid().ToString());

            _context = new DatabaseContext(options, claimService);

            var todoItemRepository = new TodoItemRepository(_context);
            var todoListRepository = new TodoListRepository(_context);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoItemProfile());
                cfg.AddProfile(new TodoListProfile());
            });

            var mapper = mockMapper.CreateMapper();

            _sut = new TodoListService(todoListRepository, mapper, claimService);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllByListIdAsync()
        {
            // TODO, only for testing now
            var createTodoListModel = new CreateTodoListModel()
            {
                Title = "Test"
            };

            var result = await _sut.CreateAsync(createTodoListModel);

            var list = _context.TodoLists.ToList();

            result.Should().NotBeEmpty();
            _context.TodoLists.Should().HaveCount(1);
            _context.TodoLists.FirstOrDefault(tl => tl.Id == result).Should().NotBeNull();
        }
    }
}
