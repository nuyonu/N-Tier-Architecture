using FizzWare.NBuilder;
using FluentValidation.TestHelper;
using N_Tier.Application.Models.User;
using N_Tier.Application.Models.Validators.User;
using Xunit;

namespace N_Tier.Application.UnitTests.Validators
{
    public class ConfirmEmailModelValidatorTests
    {
        private readonly ConfirmEmailModelValidator _sut;

        public ConfirmEmailModelValidatorTests()
        {
            _sut = new ConfirmEmailModelValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Token_Is_Empty()
        {
            // Arrange
            var confirmEmailModel = new ConfirmEmailModel { Token = string.Empty };

            // Act
            var result = _sut.TestValidate(confirmEmailModel);

            // Assert
            result.ShouldHaveValidationErrorFor(ce => ce.Token);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var confirmEmailModel = new ConfirmEmailModel { UserId = string.Empty };

            // Act
            var result = _sut.TestValidate(confirmEmailModel);

            // Assert
            result.ShouldHaveValidationErrorFor(ce => ce.UserId);
        }

        [Fact]
        public void Validator_Should_Not_Have_Error_When_All_Inputs_Are_Ok()
        {
            // Arrange
            var confirmEmailModel = Builder<ConfirmEmailModel>.CreateNew().Build();

            // Act
            var result = _sut.TestValidate(confirmEmailModel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(ce => ce.Token);
            result.ShouldNotHaveValidationErrorFor(ce => ce.UserId);
        }
    }
}
