using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Exceptions
{
    public class DuplicateISBNException : LibraryException
    {
        public DuplicateISBNException(string message) : base(message, statusCode : 409)
        {
            
        }
    }
}
