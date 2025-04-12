using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class TodoItemRepository(DatabaseContext context) : BaseRepository<TodoItem>(context), ITodoItemRepository;
