namespace N_Tier.Application.Models.Validators.TodoItem
{
    public static class CreateTodoItemValidatorConfiguration
    {
        public static int MinimumTitleLength = 5;
        public static int MaximumTitleLength = 50;
        public static int MinimumBodyLength = 5;
        public static int MaximumBodyLength = 100;
    }
}
