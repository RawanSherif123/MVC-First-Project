using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.BLL.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
         IDepartmentRepository  departmentRepository { get;  }
         IEmployeeRepository employeeRepository { get; }
        int Complete();
    }
}
