using AutoMapper;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;

        public TodoListService(ITodoListRepository todoListRepository, IMapper mapper)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Create(CreateTodoListModel createTodoListModel)
        {
            var todoList = _mapper.Map<TodoList>(createTodoListModel);
            var addedTodoList = await _todoListRepository.AddAsync(todoList);

            return addedTodoList.Id;
        }
    }
}
