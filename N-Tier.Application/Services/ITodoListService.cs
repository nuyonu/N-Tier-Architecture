using N_Tier.Application.Models.TodoList;
using System;
using System.Threading.Tasks;

namespace N_Tier.Application.Services
{
    public interface ITodoListService
    {
        Task<Guid> CreateAsync(CreateTodoListModel createTodoListModel);
        Task DeleteAsync(Guid id);
    }
}
