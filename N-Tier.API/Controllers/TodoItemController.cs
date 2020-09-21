using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    public class TodoItemController : ApiController
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            return Ok(await _todoItemService.CreateAsync(createTodoItemModel));
        }
    }
}
