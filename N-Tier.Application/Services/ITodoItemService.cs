using N_Tier.Application.Models.TodoItem;
using System;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface ITodoItemService
    {
        Task<Guid> CreateAsync(CreateTodoItemModel createTodoItemModel);
    }
}
