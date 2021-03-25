using System;
using System.Collections.Generic;
using System.Text;

namespace DataExtractor.Biz.Exceptions
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException()
        {
        }

        public InvalidFormatException(string message)
            : base(message)
        {
        }

        public InvalidFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
