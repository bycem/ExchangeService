using System;

namespace Domain.Exceptions
{
    public class BaseDomainException : Exception
    {
        public string Details { get; set; }
        public BaseDomainException(string message, string details = null) : base(message)
        {
            Details = details;
        }
    }
}