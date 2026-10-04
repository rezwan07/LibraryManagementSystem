using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Exceptions
{
    public abstract class LibraryException : Exception
    {
        /// 400 = Bad Request
        /// 404 = Not Found
        /// 409 = Conflict (duplicate)
        /// 500 = Server Error
        public int StatusCode { get; }
        protected LibraryException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
