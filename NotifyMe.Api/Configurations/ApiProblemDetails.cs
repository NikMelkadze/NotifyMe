using Microsoft.AspNetCore.Mvc;
using NotifyMe.Domain.Exceptions;

namespace NotifyMe.Api.Configurations;

public static class ApiProblemDetails
{
    public static ProblemDetails FromException(Exception ex)
    {
        return ex switch
        {
            ValidationException ve => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = ve.Message
            },

            NotFoundException ne => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Detail = ne.Message
            },


            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred"
            }
        };
    }
}