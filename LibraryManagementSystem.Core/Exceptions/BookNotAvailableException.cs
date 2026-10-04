using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Exceptions
{
    public class BookNotAvailableException : LibraryException
    {
        public BookNotAvailableException(string message) : base(message, statusCode: 400)
        {

        }
    }
}
