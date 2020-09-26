using FluentValidation;
using N_Tier.Application.Models.TodoList;

namespace N_Tier.Application.Models.Validators.TodoList
{
    public class CreateTodoListModelValidator : AbstractValidator<CreateTodoListModel>
    {
        public CreateTodoListModelValidator()
        {
            RuleFor(ctl => ctl.Title)
                .MinimumLength(5).WithMessage("Todo list title should have minimum 5 characters")
                .MaximumLength(50).WithMessage("Todo list title should have maximum 50 characters");
        }
    }
}
