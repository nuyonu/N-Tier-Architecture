using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
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
        public async Task<ActionResult> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            return Ok(ApiResult<Guid>.Success(201, await _todoItemService.CreateAsync(createTodoItemModel)));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
        {
            return Ok(ApiResult<Guid>.Success200(await _todoItemService.UpdateAsync(id, updateTodoItemModel)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return Ok(ApiResult<Guid>.Success200(await _todoItemService.DeleteAsync(id)));
        }
    }
}
