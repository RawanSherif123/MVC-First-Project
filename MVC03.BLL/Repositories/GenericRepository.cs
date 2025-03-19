using Microsoft.EntityFrameworkCore;
using MVC03.BLL.Interfaces;
using MVC03.DAL.Data.Contexts;
using MVC03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            if(typeof(TEntity) == (typeof(Employee)))
            {
                return (IEnumerable<TEntity>) _context.Employees.Include(E => E.Department).ToList();
            }
            return _context.Set<TEntity>().ToList();
        }

        public TEntity? Get(int id)
        {
            if (typeof(TEntity) == (typeof(Employee)))
            {
                return (_context.Employees.Include(E => E.Department).FirstOrDefault(E => E.Id == id)) as TEntity;
            }
            return _context.Set<TEntity>().Find(id);

        }


        public int Add(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
            return _context.SaveChanges();
        }

        public int Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
            return _context.SaveChanges();
        }

        public int Delete(TEntity model)
        {
            _context.Set<TEntity>().Remove(model);
            return _context.SaveChanges();

        }


    }
}
