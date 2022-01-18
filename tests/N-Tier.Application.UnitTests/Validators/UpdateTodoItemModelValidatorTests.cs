using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Models.Validators.TodoItem;
using Xunit;

namespace N_Tier.Application.UnitTests.Validators;

public class UpdateTodoItemModelValidatorTests
{
    private readonly UpdateTodoItemModelValidator _sut;

    public UpdateTodoItemModelValidatorTests()
    {
        _sut = new UpdateTodoItemModelValidator();
    }

    [Fact]
    public void Validator_Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var updateTodoItemModel = new UpdateTodoItemModel { Title = string.Empty };

        // Act
        var result = _sut.TestValidate(updateTodoItemModel);

        // Assert
        result.ShouldHaveValidationErrorFor(uti => uti.Title);
    }

    [Fact]
    public void Validator_Should_Have_Error_When_Body_Is_Empty()
    {
        // Arrange
        var updateTodoItemModel = new UpdateTodoItemModel { Body = string.Empty };

        // Act
        var result = _sut.TestValidate(updateTodoItemModel);

        // Assert
        result.ShouldHaveValidationErrorFor(uti => uti.Body);
    }

    [Fact]
    public void Validator_Should_Not_Have_Error_When_All_Inputs_Are_Ok()
    {
        // Arrange
        var updateTodoItemModel = Builder<UpdateTodoItemModel>.CreateNew().Build();

        // Act
        var result = _sut.TestValidate(updateTodoItemModel);

        // Assert
        result.ShouldNotHaveValidationErrorFor(uti => uti.Title);
        result.ShouldNotHaveValidationErrorFor(uti => uti.Body);
    }
}
