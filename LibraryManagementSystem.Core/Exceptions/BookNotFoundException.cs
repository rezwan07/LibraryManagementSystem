using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Exceptions
{
    public class BookNotFoundException : LibraryException
    {
        public BookNotFoundException(string message) : base(message, statusCode : 404)
        {

        }
    }
}
