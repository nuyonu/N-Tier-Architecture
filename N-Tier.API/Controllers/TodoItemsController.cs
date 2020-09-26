using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services;
using System;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    [Authorize]
    public class TodoItemsController : ApiController
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemsController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            return Ok(await _todoItemService.CreateAsync(createTodoItemModel));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Guid>> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
        {
            return Ok(await _todoItemService.UpdateAsync(id, updateTodoItemModel));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _todoItemService.DeleteAsync(id);

            return NoContent();
        }
    }
}
