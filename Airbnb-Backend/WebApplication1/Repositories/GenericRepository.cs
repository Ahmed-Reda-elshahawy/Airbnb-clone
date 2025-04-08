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

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIDAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        public async Task CreateAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            context.Entry(entity).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
        }

    // Save changes to the database
    
    }
}
