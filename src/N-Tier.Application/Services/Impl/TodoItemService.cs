using MapsterMapper;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class TodoItemService(
    ITodoItemRepository todoItemRepository,
    ITodoListRepository todoListRepository,
    IMapper mapper)
    : ITodoItemService
{
    public async Task<IEnumerable<TodoItemResponseModel>> GetAllByListIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var todoItems = await todoItemRepository.GetAllAsync(ti => ti.List.Id == id);

        return mapper.Map<IEnumerable<TodoItemResponseModel>>(todoItems);
    }

    public async Task<CreateTodoItemResponseModel> CreateAsync(CreateTodoItemModel createTodoItemModel,
        CancellationToken cancellationToken = default)
    {
        var todoList = await todoListRepository.GetFirstAsync(tl => tl.Id == createTodoItemModel.TodoListId);
        var todoItem = mapper.Map<TodoItem>(createTodoItemModel);

        todoItem.List = todoList;

        return new CreateTodoItemResponseModel
        {
            Id = (await todoItemRepository.AddAsync(todoItem)).Id
        };
    }

    public async Task<UpdateTodoItemResponseModel> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel,
        CancellationToken cancellationToken = default)
    {
        var todoItem = await todoItemRepository.GetFirstAsync(ti => ti.Id == id);

        mapper.Map(updateTodoItemModel, todoItem);

        return new UpdateTodoItemResponseModel
        {
            Id = (await todoItemRepository.UpdateAsync(todoItem)).Id
        };
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var todoItem = await todoItemRepository.GetFirstAsync(ti => ti.Id == id);

        return new BaseResponseModel
        {
            Id = (await todoItemRepository.DeleteAsync(todoItem)).Id
        };
    }
}
