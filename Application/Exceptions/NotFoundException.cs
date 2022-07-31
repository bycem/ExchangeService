using System;

namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string propertyName):base($"{propertyName} not found")
        {
            
        }
    }
}