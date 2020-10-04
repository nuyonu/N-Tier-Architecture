using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Models.Validators.TodoItem;
using Xunit;

namespace N_Tier.Application.UnitTests.Validators
{
    public class CreateTodoItemModelValidatorTests
    {
        private readonly CreateTodoItemModelValidator _sut;

        public CreateTodoItemModelValidatorTests()
        {
            _sut = new CreateTodoItemModelValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var createTodoItemModel = new CreateTodoItemModel { Title = string.Empty };

            // Act
            var result = _sut.TestValidate(createTodoItemModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cti => cti.Title);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Body_Is_Empty()
        {
            // Arrange
            var createTodoItemModel = new CreateTodoItemModel { Body = string.Empty };

            // Act
            var result = _sut.TestValidate(createTodoItemModel);

            // Assert
            result.ShouldHaveValidationErrorFor(cti => cti.Body);
        }

        [Fact]
        public void Validator_Should_Not_Have_Error_When_All_Inputs_Are_Ok()
        {
            // Arrange
            var createTodoItemModel = Builder<CreateTodoItemModel>.CreateNew().Build();

            // Act
            var result = _sut.TestValidate(createTodoItemModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(cti => cti.Title);
            result.ShouldNotHaveValidationErrorFor(cti => cti.Body);
        }
    }
}
