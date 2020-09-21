namespace N_Tier.Core.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsDone { get; set; }

        public virtual TodoList List { get; set; }
    }
}
