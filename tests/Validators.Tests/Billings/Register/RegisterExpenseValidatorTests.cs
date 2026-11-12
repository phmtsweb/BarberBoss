using BarberBoss.Application.Validators.Billings;
using BarberBoss.Communcations.Enums;
using BarberBoss.Exceptions.Resources;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Billings.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_BarberName_Should_Fail(string? name)
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.BarberName = name!;
        // Act
        var result = validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.BARBER_NAME_MANDATORY));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_ClientName_Should_Fail(string? name)
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.ClientName = name!;
        // Act
        var result = validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.CLIENT_NAME_MANDATORY));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_ServiceName_Should_Fail(string? name)
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.ServiceName = name!;
        // Act
        var result = validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.SERVICE_NAME_MANDATORY));
    }

    [Theory]
    [InlineData(0, Status.Pending)]
    [InlineData(0, Status.Canceled)]
    [InlineData(-1, Status.Pending)]
    [InlineData(-1, Status.Canceled)]
    public void Amount_Should_Be_Greater_Than_Zero_When_Status_Is_Not_Canceled(decimal amount, Status status)
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Amount = amount;
        request.Status = status;
        // Act
        var result = validator.Validate(request);
        // Assert
        if (status != Status.Canceled && amount <= 0)
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_GT_ZERO));
        }
        else if (status == Status.Canceled && amount != 0)
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MB_ZERO));
        }
        else
        {
            result.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void Date_Should_Not_Be_Empty()
    {
        // Arrange
        var validator = new BillingsValidator();
        var request = RequestRegisterBillingJsonBuilder.Build();
        request.Date = default;
        // Act
        var result = validator.Validate(request);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.MANDATORY_DATE));
    }
}
