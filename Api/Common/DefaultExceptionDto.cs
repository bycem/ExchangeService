using System.Collections.Generic;

namespace Api.Common
{
    public class DefaultExceptionDto
    {
        public List<string> Errors { get; set; } = new();
        public string Details { get; set; }

    }
}