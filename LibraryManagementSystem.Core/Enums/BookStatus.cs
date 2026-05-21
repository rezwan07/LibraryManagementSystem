using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Enums
{
    public enum BookStatus
    {
        Available = 1,
        Borrowed = 2,
        Lost = 3,
        Damaged = 4,
        UnderMaintenance = 5
    }
}
