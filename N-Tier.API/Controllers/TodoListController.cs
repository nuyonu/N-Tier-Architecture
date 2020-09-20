using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    public class TodoListController : ApiController
    {
        private readonly ITodoListService _todoListService;

        public TodoListController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateTodoListModel createTodoListModel)
        {
            return Ok(await _todoListService.Create(createTodoListModel));
        }
    }
}
