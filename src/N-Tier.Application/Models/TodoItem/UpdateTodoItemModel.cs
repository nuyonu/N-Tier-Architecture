using System;

namespace N_Tier.Application.Models.TodoItem
{
    public class UpdateTodoItemModel
    {
        public Guid TodoListId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsDone { get; set; }
    }

    public class UpdateTodoItemResponseModel : BaseResponseModel
    { }
}
