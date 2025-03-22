using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Data.Contexts;

namespace MVC03.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;


        public IDepartmentRepository departmentRepository { get; }

        public IEmployeeRepository employeeRepository { get; }

        public UnitOfWork(CompanyDbContext context)
        {
            
            _context = context;
            departmentRepository = new DepartmentRepository(_context);
            employeeRepository = new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

       
        public async ValueTask DisposeAsync()
        {
           await _context.DisposeAsync();
        }
    }
}
