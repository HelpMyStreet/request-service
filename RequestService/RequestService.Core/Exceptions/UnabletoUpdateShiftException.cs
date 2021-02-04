using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Exceptions
{
    public class UnableToUpdateShiftException : Exception
    {
        public UnableToUpdateShiftException() : base("Unable to update shift")
        {
        }

        public UnableToUpdateShiftException(string details) : base(details)
        {
        }
    }
}
