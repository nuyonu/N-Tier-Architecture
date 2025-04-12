using MapsterMapper;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using N_Tier.Shared.Services;

namespace N_Tier.Application.Services.Impl;

public class TodoListService(ITodoListRepository todoListRepository, IMapper mapper, IClaimService claimService)
    : ITodoListService
{
    public async Task<IEnumerable<TodoListResponseModel>> GetAllAsync()
    {
        var currentUserId = claimService.GetUserId();

        var todoLists = await todoListRepository.GetAllAsync(tl => tl.CreatedBy == currentUserId);

        return mapper.Map<IEnumerable<TodoListResponseModel>>(todoLists);
    }

    public async Task<CreateTodoListResponseModel> CreateAsync(CreateTodoListModel createTodoListModel)
    {
        var todoList = mapper.Map<TodoList>(createTodoListModel);

        var addedTodoList = await todoListRepository.AddAsync(todoList);

        return new CreateTodoListResponseModel
        {
            Id = addedTodoList.Id
        };
    }

    public async Task<UpdateTodoListResponseModel> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel)
    {
        var todoList = await todoListRepository.GetFirstAsync(tl => tl.Id == id);

        var userId = claimService.GetUserId();

        if (userId != todoList.CreatedBy)
            throw new BadRequestException("The selected list does not belong to you");

        todoList.Title = updateTodoListModel.Title;

        return new UpdateTodoListResponseModel
        {
            Id = (await todoListRepository.UpdateAsync(todoList)).Id
        };
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var todoList = await todoListRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await todoListRepository.DeleteAsync(todoList)).Id
        };
    }
}
