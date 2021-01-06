using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base("Duplicate Exception")
        {
        }
    }
}
