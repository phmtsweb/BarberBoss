using System.Net;

namespace BarberBoss.Exceptions.ExceptionsBase;

public class NotFoundException(string message) : BarberBossException(message)
{
    public override int StatusCode => (int) HttpStatusCode.NotFound;

    public override List<string> GetErrors() => [Message];
}
