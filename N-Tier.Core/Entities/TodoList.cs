using N_Tier.Core.Entities.Common;
using System.Collections.Generic;

namespace N_Tier.Core.Entities
{
    public class TodoList : BaseEntity
    {
        public string Title { get; set; }
        public List<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
