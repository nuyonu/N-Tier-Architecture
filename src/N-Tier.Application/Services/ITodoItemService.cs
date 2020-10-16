using N_Tier.Application.Models.TodoItem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface ITodoItemService
    {
        Task<Guid> CreateAsync(CreateTodoItemModel createTodoItemModel);

        Task<Guid> DeleteAsync(Guid id);

        Task<IEnumerable<TodoItemResponseModel>> GetAllByListIdAsync(Guid id);

        Task<Guid> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel);
    }
}
