using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Persistence;

namespace N_Tier.Infrastructure.Repositories.Impl
{
    public class TodoListRepository : BaseRepository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(DatabaseContext context) : base(context)
        { }
    }
}
