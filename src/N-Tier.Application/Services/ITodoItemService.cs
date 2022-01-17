using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface ITodoItemService
    {
        Task<CreateTodoItemResponseModel> CreateAsync(CreateTodoItemModel createTodoItemModel, CancellationToken cancellationToken = default);

        Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<TodoItemResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UpdateTodoItemResponseModel> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel, CancellationToken cancellationToken = default);
    }
}
