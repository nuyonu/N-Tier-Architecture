using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers;

[Authorize]
public class TodoListsController(ITodoListService todoListService, ITodoItemService todoItemService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(ApiResult<IEnumerable<TodoListResponseModel>>.Success(await todoListService.GetAllAsync()));
    }

    [HttpGet("{id:guid}/todoItems")]
    public async Task<IActionResult> GetAllTodoItemsAsync(Guid id)
    {
        return Ok(ApiResult<IEnumerable<TodoItemResponseModel>>.Success(
            await todoItemService.GetAllByListIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTodoListModel createTodoListModel)
    {
        return Ok(ApiResult<CreateTodoListResponseModel>.Success(
            await todoListService.CreateAsync(createTodoListModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel)
    {
        return Ok(ApiResult<UpdateTodoListResponseModel>.Success(
            await todoListService.UpdateAsync(id, updateTodoListModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await todoListService.DeleteAsync(id)));
    }
}
