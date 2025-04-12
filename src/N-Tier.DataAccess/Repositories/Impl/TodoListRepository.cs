using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class TodoListRepository(DatabaseContext context) : BaseRepository<TodoList>(context), ITodoListRepository;
