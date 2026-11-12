using BarberBoss.Communcations.Enums;
using BarberBoss.Communcations.Requests;
using BarberBoss.Exceptions.Resources;
using FluentValidation;

namespace BarberBoss.Application.Validators.Billings;

public class BillingsValidator : AbstractValidator<RequestRegisterBillingJson>
{
    public BillingsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor<DateOnly>(b => b.Date)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.MANDATORY_DATE);
        RuleFor<string>(b => b.BarberName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.BARBER_NAME_MANDATORY)
            .MinimumLength(2)
            .WithMessage(ResourceErrorMessages.BARBER_NAME_MIN_LENGTH)
            .MaximumLength(80)
            .WithMessage(ResourceErrorMessages.BARBER_NAME_MAX_LENGTH);
        RuleFor<string>(b => b.ClientName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_MANDATORY)
            .MinimumLength(2)
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_MIN_LENGTH)
            .MaximumLength(120)
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_MAX_LENGTH);
        RuleFor<string>(b => b.ServiceName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_MANDATORY)
            .MinimumLength(2)
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_MIN_LENGTH)
            .MaximumLength(120)
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_MAX_LENGTH);
        RuleFor<decimal>(b => b.Amount)
            .GreaterThan(0)
            .When(b => b.Status != Status.Canceled)
            .WithMessage(ResourceErrorMessages.AMOUNT_GT_ZERO);
        RuleFor<decimal>(b => b.Amount)
            .Equal(0)
            .When(b => b.Status == Status.Canceled)
            .WithMessage(ResourceErrorMessages.AMOUNT_MB_ZERO);
        RuleFor<PaymentMethod>(b => b.PaymentMethod)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);
        RuleFor<Status>(b => b.Status)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.STATUS_INVALID);
    }
}
