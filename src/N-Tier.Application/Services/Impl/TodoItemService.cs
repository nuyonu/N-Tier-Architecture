using AutoMapper;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
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
            var todoItems = await _todoItemRepository.GetAllAsync(ti => ti.List.Id == id);

            return _mapper.Map<IEnumerable<TodoItemResponseModel>>(todoItems);
        }

        public async Task<CreateTodoItemResponseModel> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            var todoList = await _todoListRepository.GetFirstAsync(tl => tl.Id == createTodoItemModel.TodoListId);
            var todoItem = _mapper.Map<TodoItem>(createTodoItemModel);

            todoItem.List = todoList;

            return new CreateTodoItemResponseModel
            {
                Id = (await _todoItemRepository.AddAsync(todoItem)).Id
            };
        }

        public async Task<UpdateTodoItemResponseModel> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
        {
            var todoItem = await _todoItemRepository.GetFirstAsync(ti => ti.Id == id);

            _mapper.Map(updateTodoItemModel, todoItem);

            return new UpdateTodoItemResponseModel
            {
                Id = (await _todoItemRepository.UpdateAsync(todoItem)).Id
            };
        }

        public async Task<BaseResponseModel> DeleteAsync(Guid id)
        {
            var todoItem = await _todoItemRepository.GetFirstAsync(ti => ti.Id == id);

            return new BaseResponseModel
            {
                Id = (await _todoItemRepository.DeleteAsync(todoItem)).Id
            };
        }
    }
}
