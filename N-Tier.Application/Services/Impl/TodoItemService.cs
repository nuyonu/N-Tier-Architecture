using AutoMapper;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;

        public TodoItemService(ITodoItemRepository todoItemRepository, ITodoListRepository todoListRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            var todoList = await _todoListRepository.GetFirst(tl => tl.Id == createTodoItemModel.TodoListId);
            var todoItem = _mapper.Map<TodoItem>(createTodoItemModel);

            todoItem.List = todoList ?? throw new NotFoundException("List does not exist anymore");
            todoItem.IsDone = false;

            await _todoItemRepository.AddAsync(todoItem);

            return todoItem.Id;
        }
    }
}
