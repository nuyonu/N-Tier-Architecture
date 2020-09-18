namespace N_Tier.Core.Entities
{
    public class TodoItem : BaseEntity
    {
        public string IsDone { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
