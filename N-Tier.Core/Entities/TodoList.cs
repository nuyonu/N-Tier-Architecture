using N_Tier.Core.Common;
using System.Collections.Generic;

namespace N_Tier.Core.Entities
{
    public class TodoList : AdvancedBaseEntity
    {
        public string Title { get; set; }
        public List<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
