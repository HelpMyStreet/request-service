using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Exceptions
{
    public class InvalidFilterException : Exception
    {
        public InvalidFilterException() : base("Invalid filter")
        {
        }
    }
}
