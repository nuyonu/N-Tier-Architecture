using FluentValidation;
using N_Tier.Application.Models.TodoItem;

namespace N_Tier.Application.Models.Validators.TodoItem
{
    public class CreateTodoItemModelValidator : AbstractValidator<CreateTodoItemModel>
    {
        public CreateTodoItemModelValidator()
        {
            var configuration = new
            {
                MinimumTitleLength = 5,
                MaximumTitleLength = 50,
                MinimumBodyLength = 5,
                MaximumBodyLength = 100
            };

            RuleFor(cti => cti.Title)
                .MinimumLength(configuration.MinimumTitleLength)
                .WithMessage($"Todo item title should have minimum ${configuration.MaximumTitleLength} characters")
                .MaximumLength(configuration.MaximumTitleLength)
                .WithMessage($"Todo item title should have maximum {configuration.MaximumTitleLength} characters");

            RuleFor(cti => cti.Body)
                .MinimumLength(configuration.MinimumBodyLength)
                .WithMessage($"Todo item body should have minimum {configuration.MinimumBodyLength} characters")
                .MaximumLength(configuration.MaximumBodyLength)
                .WithMessage($"Todo item body should have maximum {configuration.MaximumBodyLength} characters");
        }
    }
}
