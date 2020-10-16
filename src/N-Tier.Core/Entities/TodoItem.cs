using N_Tier.Core.Common;
using System;

namespace N_Tier.Core.Entities
{
    public class TodoItem : BaseEntity, IAuditedEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsDone { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual TodoList List { get; set; }
    }
}
