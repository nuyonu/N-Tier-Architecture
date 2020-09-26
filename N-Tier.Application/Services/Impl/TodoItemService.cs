using AutoMapper;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;

        public TodoItemService(ITodoItemRepository todoItemRepository,
                               ITodoListRepository todoListRepository,
                               IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoItemResponseModel>> GetAllByListIdAsync(Guid id)
        {
            var todoItems = await _todoItemRepository.GetAsync(ti => ti.List.Id == id);

            return _mapper.Map<IEnumerable<TodoItemResponseModel>>(todoItems);
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

        public async Task<Guid> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
        {
            var todoList = await _todoListRepository.GetFirst(tl => tl.Id == updateTodoItemModel.TodoListId);

            if (todoList == null)
                throw new NotFoundException("List does not exist anymore");

            var todoItem = await _todoItemRepository.GetFirst(ti => ti.Id == id);

            if(todoItem == null)
                throw new NotFoundException("Todo item does not exist anymore");

            todoItem.Title = updateTodoItemModel.Title;
            todoItem.Body = updateTodoItemModel.Body;
            todoItem.IsDone = updateTodoItemModel.IsDone;

            await _todoItemRepository.UpdateAsync(todoItem);

            return todoItem.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var todoItem = await _todoItemRepository.GetFirst(ti => ti.Id == id);

            if (todoItem == null)
                throw new NotFoundException("Todo item does not exist anymore");
        }
    }
}
