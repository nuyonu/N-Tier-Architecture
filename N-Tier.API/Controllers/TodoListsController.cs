using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    public class TodoListsController : ApiController
    {
        private readonly ITodoListService _todoListService;

        public TodoListsController(ITodoListService todoListService)
        {
            _todoListService = todoListService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync(CreateTodoListModel createTodoListModel)
        {
            return Ok(await _todoListService.CreateAsync(createTodoListModel));
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _todoListService.DeleteAsync(id);

            return NoContent();
        }
    }
}
