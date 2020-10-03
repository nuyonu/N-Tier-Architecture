using AutoMapper;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using N_Tier.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public TodoListService(ITodoListRepository todoListRepository, IMapper mapper, IClaimService claimService)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<IEnumerable<TodoListResponseModel>> GetAllAsync()
        {
            var currentUserId = _claimService.GetUserId();

            var todoLists = await _todoListRepository.GetAsync(tl => tl.CreatedBy == currentUserId);

            return _mapper.Map<IEnumerable<TodoListResponseModel>>(todoLists);
        }

        public async Task<Guid> CreateAsync(CreateTodoListModel createTodoListModel)
        {
            var todoList = _mapper.Map<TodoList>(createTodoListModel);

            var addedTodoList = await _todoListRepository.AddAsync(todoList);

            return addedTodoList.Id;
        }

        public async Task<Guid> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel)
        {
            var todoList = await _todoListRepository.GetFirstAsync(tl => tl.Id == id);

            if(todoList == null)
                throw new NotFoundException("List does not exist anymore");

            var userId = _claimService.GetUserId();

            if (userId != todoList.CreatedBy)
                throw new BadRequestException("The selected list does not belong to you");

            todoList.Title = updateTodoListModel.Title;

            return (await _todoListRepository.UpdateAsync(todoList)).Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var todoList = await _todoListRepository.GetFirstAsync(tl => tl.Id == id);

            if (todoList == null)
                throw new NotFoundException("List does not exist anymore");

            return (await _todoListRepository.DeleteAsync(todoList)).Id;
        }
    }
}
