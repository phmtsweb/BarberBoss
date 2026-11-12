using BarberBoss.Communcations.Requests;
using BarberBoss.Communcations.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.Mappers.Billings;

public class BillingMapper : BaseMapper<Billing, RequestRegisterBillingJson, ResponseBillingJson, ResponseShortBillingJson>
{
}
