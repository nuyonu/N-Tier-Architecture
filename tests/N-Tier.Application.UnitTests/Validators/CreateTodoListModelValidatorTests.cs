using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using N_Tier.Application.Models.TodoList;
using N_Tier.Application.Models.Validators.TodoList;
using Xunit;

namespace N_Tier.Application.UnitTests.Validators
{
    public class CreateTodoListModelValidatorTests
    {
        private readonly CreateTodoListModelValidator _sut;

        public CreateTodoListModelValidatorTests()
        {
            _sut = new CreateTodoListModelValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var createTodoListModel = new CreateTodoListModel { Title = string.Empty };

            // Act
            var result = _sut.TestValidate(createTodoListModel);

            // Assert
            result.ShouldHaveValidationErrorFor(ctl => ctl.Title);
        }

        [Fact]
        public void Validator_Should_Not_Have_Error_When_All_Inputs_Are_Ok()
        {
            // Arrange
            var createTodoListModel = Builder<CreateTodoListModel>.CreateNew().Build();

            // Act
            var result = _sut.TestValidate(createTodoListModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(ctl => ctl.Title);
        }
    }
}
