using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Resources.Reports;

namespace BarberBoss.Domain.Extensions;

public static class PaymentMethodExtension
{
    public static string PaymentMethodToString(this PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.Cash => ResourceReportMessages.PAYMENT_METHOD_CASH,
            PaymentMethod.Card => ResourceReportMessages.PAYMENT_METHOD_CARD,
            PaymentMethod.Pix => ResourceReportMessages.PAYMENT_METHOD_PIX,
            PaymentMethod.Other => ResourceReportMessages.PAYMENT_METHOD_OTHER,
            _ => string.Empty
        };
    }
}
