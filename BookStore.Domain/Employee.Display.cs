using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain
{
    public partial class Employee
    {
        public string FullName => $"{FirstName} {LastName}";
        public string LoginUserName => $"{FirstName.Substring(0, 2)}{LastName.Substring(0,2)}{EmployeeId}";
    }
}
