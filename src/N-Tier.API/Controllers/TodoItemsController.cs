using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers;

[Authorize]
public class TodoItemsController(ITodoItemService todoItemService) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTodoItemModel createTodoItemModel)
    {
        return Ok(ApiResult<CreateTodoItemResponseModel>.Success(
            await todoItemService.CreateAsync(createTodoItemModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
    {
        return Ok(ApiResult<UpdateTodoItemResponseModel>.Success(
            await todoItemService.UpdateAsync(id, updateTodoItemModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await todoItemService.DeleteAsync(id)));
    }
}
