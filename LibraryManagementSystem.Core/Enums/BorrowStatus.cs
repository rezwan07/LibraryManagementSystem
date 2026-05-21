using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Enums
{
    public enum BorrowStatus
    {
        Borrowed =1,
        Returned = 2,
        Overdue = 3,
        ReturnedLate = 4
    }
}
