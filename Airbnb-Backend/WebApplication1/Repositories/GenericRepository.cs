using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AirbnbDBContext context;

        public GenericRepository(AirbnbDBContext _context)
        {
            context = _context;
        }
        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetByID(int id)
        {
            return context.Set<T>().Find(id);
        }
        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
