using BarberBoss.Communcations.Filters;
using BarberBoss.Exceptions.Resources;
using FluentValidation;

namespace BarberBoss.Application.Validators.Utils;

public class PaginationFilterValidation : AbstractValidator<PaginationFilter>
{
    private readonly List<string> validOrderBy =
        [
            "id", "id_desc",
            "date", "date_desc",
            "barber_name", "barber_name_desc",
            "client_name", "client_name_desc",
            "service_name", "service_name_desc",
            "amount", "amount_desc",
            "payment_method", "payment_method_desc",
            "status", "status_desc",
            "notes", "notes_desc",
            "created_at", "created_at_desc",
            "updated_at", "updated_at_desc"
        ];
    public PaginationFilterValidation()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor<int>(f => f.PageSize)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.INVALID_PAGE_SIZE);
        RuleFor<int>(f => f.PageNumber)
            .InclusiveBetween(1, 50)
            .WithMessage(ResourceErrorMessages.INVALID_PAGE_SIZE);
        RuleFor<string>(f => f.OrderBy ?? string.Empty)
            .Must(BeAValidSortColumn)
            .When(f => !string.IsNullOrWhiteSpace(f.OrderBy))
            .WithMessage(string.Format(ResourceErrorMessages.INVALID_ORDER_BY, string.Join(", ", validOrderBy)));
    }

    private bool BeAValidSortColumn(string? orderBy)
    {
        if (string.IsNullOrEmpty(orderBy))
        {
            return true;
        }
        return validOrderBy.Contains(orderBy.ToLower());
    }
}
