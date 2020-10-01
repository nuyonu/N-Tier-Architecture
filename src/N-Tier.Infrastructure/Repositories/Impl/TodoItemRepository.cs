using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Persistence;

namespace N_Tier.Infrastructure.Repositories.Impl
{
    public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(DatabaseContext context) : base(context)
        { }
    }
}
