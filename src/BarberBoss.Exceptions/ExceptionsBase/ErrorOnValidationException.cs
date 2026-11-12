using System.Net;

namespace BarberBoss.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(List<string> errors) : BarberBossException(string.Empty)
{
    private readonly List<string> _errors = errors;
    public override int StatusCode => (int) HttpStatusCode.BadRequest;
    public override List<string> GetErrors() => _errors;
}
