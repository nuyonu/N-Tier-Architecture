using FluentValidation;
using N_Tier.Application.Models.TodoItem;

namespace N_Tier.Application.Models.Validators.TodoItem
{
    public class CreateTodoItemModelValidator : AbstractValidator<CreateTodoItemModel>
    {
        public CreateTodoItemModelValidator()
        {
            RuleFor(cti => cti.Title)
                .MinimumLength(CreateTodoItemValidatorConfiguration.MinimumTitleLength)
                .WithMessage($"Todo item title should have minimum ${CreateTodoItemValidatorConfiguration.MaximumTitleLength} characters")
                .MaximumLength(CreateTodoItemValidatorConfiguration.MaximumTitleLength)
                .WithMessage($"Todo item title should have maximum {CreateTodoItemValidatorConfiguration.MaximumTitleLength} characters");

            RuleFor(cti => cti.Body)
                .MinimumLength(CreateTodoItemValidatorConfiguration.MinimumBodyLength)
                .WithMessage($"Todo item body should have minimum {CreateTodoItemValidatorConfiguration.MinimumBodyLength} characters")
                .MaximumLength(CreateTodoItemValidatorConfiguration.MaximumBodyLength)
                .WithMessage($"Todo item body should have maximum {CreateTodoItemValidatorConfiguration.MaximumBodyLength} characters");
        }
    }
}
