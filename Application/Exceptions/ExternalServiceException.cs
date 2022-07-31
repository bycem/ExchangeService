using System;
using System.Net;

namespace Application.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ExternalServiceException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}