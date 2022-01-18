using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoList;

namespace N_Tier.Application.Services;

public interface ITodoListService
{
    Task<CreateTodoListResponseModel> CreateAsync(CreateTodoListModel createTodoListModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<TodoListResponseModel>> GetAllAsync();

    Task<UpdateTodoListResponseModel> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel);
}
