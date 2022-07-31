using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Api.Common
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
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
                var knownEx = GetKnownException(ex);

                await HandleKnownException(context, knownEx.Result, knownEx.Code);
            }
        }

        private Task HandleKnownException(HttpContext context, string result, HttpStatusCode code)
        {
            if (result != string.Empty)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }
            return Task.CompletedTask;
        }

        private (string Result, HttpStatusCode Code) GetKnownException(Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string result;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Errors = validationException.Failures
                    });
                    break;
                case ArgumentException argumentException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Errors = new List<string>
                        {
                            argumentException.Message
                        }
                    });
                    break;
                case BaseDomainException baseDomainException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Errors = new List<string>
                        {
                            baseDomainException.Message
                        },
                        Details = baseDomainException.Details
                    });
                    break;
                case ExternalServiceException externalServiceException:
                    code = externalServiceException.StatusCode;

                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Details = externalServiceException.Message
                    });
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;

                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Details = notFoundException.Message
                    });
                    break;
                default:
                    result = JsonSerializer.Serialize(new DefaultExceptionDto
                    {
                        Errors = new List<string>
                        {
                            exception.Message
                        }
                    });

                    break;
            }

            return (result, code);
        }
    }
}