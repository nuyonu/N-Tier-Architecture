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
        public async Task<IActionResult> CreateAsync(CreateTodoItemModel createTodoItemModel)
        {
            return Ok(ApiResult<CreateTodoItemResponseModel>.Success(201, await _todoItemService.CreateAsync(createTodoItemModel)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
        {
            return Ok(ApiResult<UpdateTodoItemResponseModel>.Success200(await _todoItemService.UpdateAsync(id, updateTodoItemModel)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            return Ok(ApiResult<BaseResponseModel>.Success200(await _todoItemService.DeleteAsync(id)));
        }
    }
}
