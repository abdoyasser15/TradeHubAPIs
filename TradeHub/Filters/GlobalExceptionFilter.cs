using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

public class GlobalProblemDetailsExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var httpContext = context.HttpContext;

        var problem = exception switch
        {
            ArgumentException ex => CreateProblem(
                StatusCodes.Status400BadRequest,
                "Bad Request",
                ex.Message,
                httpContext),

            KeyNotFoundException ex => CreateProblem(
                StatusCodes.Status404NotFound,
                "Not Found",
                ex.Message,
                httpContext),

            SqlException ex => CreateProblem(
                StatusCodes.Status503ServiceUnavailable,
                "Service Unavailable",
                ex.Message,
                httpContext),

            InvalidOperationException ex => CreateProblem(
                StatusCodes.Status409Conflict,
                "Conflict",
                ex.Message,
                httpContext),

            _ => CreateProblem(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                exception.Message + (exception.InnerException != null ? " | " + exception.InnerException.Message : ""),
                httpContext)
        };

        context.Result = new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };

        context.ExceptionHandled = true;
    }

    private static ProblemDetails CreateProblem(
        int status,
        string title,
        string detail,
        HttpContext context)
    {
        return new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{status}",
            Title = title,
            Status = status,
            Detail = detail,
            Instance = context.Request.Path
        };
    }
}