using BarberBoss.Communcations.Enums;
using BarberBoss.Communcations.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterBillingJsonBuilder
{
    public static RequestRegisterBillingJson Build()
    {
        return new Faker<RequestRegisterBillingJson>()
            .RuleFor(r => r.Date, f => f.Date.RecentDateOnly())
            .RuleFor(r => r.BarberName, f =>
            {
                var fullName = f.Name.FullName();
                return fullName.Length > 80 ? fullName[..80] : fullName;
            })
            .RuleFor(r => r.ClientName, f =>
            {
                var fullName = f.Name.FullName();
                return fullName.Length > 120 ? fullName[..120] : fullName;
            })
            .RuleFor(r => r.ServiceName, f =>
            {
                var serviceName = f.Commerce.ProductName();
                return serviceName.Length > 120 ? serviceName[..120] : serviceName;
            })
            .RuleFor(r => r.Amount, f => f.Random.Decimal(min: 1, max: 120))
            .RuleFor(r => r.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(r => r.Status, f => f.PickRandomWithout<Status>(Status.Canceled))
            .Generate();
    }
}
