namespace BarberBoss.Communcations.Responses;

public class ResponseErrorsJson
{
    public List<string> Errors { get; }

    public ResponseErrorsJson(string error) => Errors = [error];
    public ResponseErrorsJson(List<string> errors) => Errors = errors;
}
