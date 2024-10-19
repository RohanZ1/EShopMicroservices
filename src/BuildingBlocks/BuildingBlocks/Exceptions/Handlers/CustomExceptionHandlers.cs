﻿using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace BuildingBlocks.Exceptions.Handlers
{
    public class CustomExceptionHandlers(ILogger<CustomExceptionHandlers> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {exceptionMessage}, Time of Occurence {time}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
               (
                  exception.Message,
                  exception.GetType().Name,
                  context.Response.StatusCode = StatusCodes.Status400BadRequest
               ),
                NotFoundException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ =>
                 (
                  exception.Message,
                 exception.GetType().Name,
                 context.Response.StatusCode = StatusCodes.Status500InternalServerError
                 )

            };

            ProblemDetails problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };
            problemDetails.Extensions.Add("traceid", context.TraceIdentifier);
            if (exception is ValidationException ex)
            {
                problemDetails.Extensions.Add("ValidationErrors", ex.Errors);
            }
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
