using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCapteurs.Data;
using GestionCapteurs.Data.Model;
using Microsoft.Extensions.Caching.Memory;

namespace GestionCapteurs
{
    public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
     
        var code = StatusCodes.Status500InternalServerError;
        var result = new
        {
            Code = "InternalServerError",
            Message = "Une erreur inattendue s'est produite. Veuillez réessayer plus tard.",
            Details = exception.Message
        };

        if (exception is DbUpdateConcurrencyException)
        {
            code = StatusCodes.Status409Conflict;
            result = new
            {
                Code = "ConcurrencyError",
                Message = "Une erreur de concurrence s'est produite lors de la mise à jour de la ressource.",
                Details = exception.Message
            };
        }
        else if (exception is DbUpdateException)
        {
            code = StatusCodes.Status400BadRequest;
            result = new
            {
                Code = "DatabaseUpdateError",
                Message = "Une erreur de base de données s'est produite lors du traitement de la demande.",
                Details = exception.Message
            };
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        return context.Response.WriteAsJsonAsync(result);
    }
}

}