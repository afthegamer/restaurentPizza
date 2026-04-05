using FluentValidation;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using restaurent_pizza.Exceptions;

namespace restaurent_pizza.Filters;

// 🔴 ASP.NET — filtre d'exception global (comme GlobalExceptionFilter au travail)
// Intercepte TOUTES les exceptions non gérées et les convertit en ProblemDetails (RFC 7807)
public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IAsyncExceptionFilter  // 🔴 ASP.NET — primary constructor + injection du logger
{
    public Task OnExceptionAsync(ExceptionContext context)  // 🔴 ASP.NET — appelé automatiquement quand une exception est lancée
    {
        // 🔵 C# pur — log l'erreur (comme au travail : logger.LogError)
        logger.LogError(context.Exception, context.Exception.Message);

        // 🔴 ASP.NET — ProblemDetails = format standard RFC 7807 pour les erreurs HTTP
        var problemDetails = new ProblemDetails
        {
            Detail = context.Exception.Message,
            Status = context.Exception switch  // 🔵 C# pur — pattern matching switch expression
            {
                EntityNotFoundException => (int)HttpStatusCode.NotFound,           // 404
                ValidationException     => (int)HttpStatusCode.BadRequest,         // 400
                _ => (int)HttpStatusCode.InternalServerError                      // 500 par défaut
            }
        };

        context.Result = new JsonResult(problemDetails);                           // 🔴 ASP.NET — sérialise le ProblemDetails en JSON
        context.HttpContext.Response.StatusCode = problemDetails.Status.Value;      // 🔴 ASP.NET — met le bon code HTTP
        context.ExceptionHandled = true;                                           // 🔴 ASP.NET — marque l'exception comme traitée
        return Task.CompletedTask;
    }
}