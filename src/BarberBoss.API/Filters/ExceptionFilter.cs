using BarberBoss.Communcations.Responses;
using BarberBoss.Exceptions.ExceptionsBase;
using BarberBoss.Exceptions.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberBoss.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BarberBossException ex)
        {
            HandleProjectException(context, ex);
            return;
        }
        ThrowUnkownException(context);
    }

    private void HandleProjectException(ExceptionContext context, BarberBossException ex)
    {
        var errorResponse = new ResponseErrorsJson(ex.GetErrors());
        context.HttpContext.Response.StatusCode = ex.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnkownException(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorsJson(ResourceErrorMessages.UNKNOWN_ERROR);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
