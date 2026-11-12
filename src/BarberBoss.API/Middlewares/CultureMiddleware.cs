using System.Globalization;

namespace BarberBoss.API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next) => _next = next;
    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault() ?? string.Empty;

        var cultureInfo = new CultureInfo(supportedLanguages.Exists(l => l.Name.Equals(requestedCulture)) ? requestedCulture : "en");
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
