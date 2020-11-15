using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N_Tier.API.Controllers
{
    [Authorize]
    public class TodoListsController : ApiController
    {
        private readonly ITodoListService _todoListService;
        private readonly ITodoItemService _todoItemService;

        public TodoListsController(ITodoListService todoListService, ITodoItemService todoItemService)
        {
            _todoListService = todoListService;
            _todoItemService = todoItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(ApiResult<IEnumerable<TodoListResponseModel>>.Success200(await _todoListService.GetAllAsync()));
        }

        [HttpGet("{id}/todoItems")]
        public async Task<IActionResult> GetAllTodoItemsAsync(Guid id)
        {
            return Ok(ApiResult<IEnumerable<TodoItemResponseModel>>.Success200(await _todoItemService.GetAllByListIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateTodoListModel createTodoListModel)
        {
            return Ok(ApiResult<CreateTodoListResponseModel>.Success(201, await _todoListService.CreateAsync(createTodoListModel)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel)
        {
            return Ok(ApiResult<UpdateTodoListResponseModel>.Success200(await _todoListService.UpdateAsync(id, updateTodoListModel)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            return Ok(ApiResult<BaseResponseModel>.Success200(await _todoListService.DeleteAsync(id)));
        }
    }
}
