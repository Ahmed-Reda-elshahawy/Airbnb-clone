using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using System.Linq.Expressions;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;

        public GenericRepository(AirbnbDBContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        #endregion
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
        public void UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        #region Delete Method
        public async Task DeleteAsync<T>(Guid id) where T:class
        {
            var entity = await context.Set<T>().FindAsync(id);  

            if (entity != null)
            {
                context.Set<T>().Remove(entity);  // Remove the entity from the DbSet
                await context.SaveChangesAsync();  // Save changes to the database
            }
            else
            {
                throw new Exception("Entity not found.");
            }
        }
        #endregion

        #region Create Method
        public async Task CreateAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Update Methods
        public async Task<T> UpdateAsync<T, TDto>(Guid id, TDto updateDto) where T : class
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
                return null;

            mapper.Map(updateDto, entity);

            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        #endregion

        #region Filtering Method (Helper Method)
        private static IQueryable<T> ApplyFilters(IQueryable<T> query, Dictionary<string, string> queryParams)
        {
            foreach (var param in queryParams)
            {
                var propertyName = param.Key;
                var propertyValue = param.Value;

                var propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    Console.WriteLine($"Property {propertyName} not found on {typeof(T).Name}. Skipping.");
                    continue;
                }
                try
                {
                    var propertyType = propertyInfo.PropertyType;
                    var isNullable = Nullable.GetUnderlyingType(propertyType) != null;
                    var underlyingType = isNullable ? Nullable.GetUnderlyingType(propertyType) : propertyType;

                    object typedValue = null;

                    if (!string.IsNullOrEmpty(propertyValue))
                    {
                        if (underlyingType == typeof(Guid))
                        {
                            typedValue = Guid.Parse(propertyValue);
                        }
                        else if (underlyingType.IsEnum)
                        {
                            typedValue = Enum.Parse(underlyingType, propertyValue);
                        }
                        else
                        {
                            typedValue = Convert.ChangeType(propertyValue, underlyingType);
                        }
                    }

                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyExpression = Expression.Property(parameter, propertyInfo);
                    var constantExpression = Expression.Constant(typedValue, propertyType);

                    var equalityExpression = Expression.Equal(propertyExpression, constantExpression);
                    var lambda = Expression.Lambda<Func<T, bool>>(equalityExpression, parameter);

                    query = query.Where(lambda);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing filter for {propertyName}: {ex.Message}");
                    continue;
                }
            }

            return query;
        }

        #endregion

        #region Get Methods
        public Guid GetCurrentUserId()
        {
            // Find the name identifier claim (contains user ID)
            //var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //    throw new InvalidOperationException("User ID claim not found"); // Throw exception if claim not found

            //return Guid.Parse(userIdClaim.Value); // Parse claim value to Guid
            return Guid.Parse("40512BA8-7C83-41B1-BDA6-415EBA1909CD"); // Parse claim value to Guid
            //return Guid.Parse("62199C29-0A98-4F17-9FFB-806B7E2CF252");
        }
        public async Task<IEnumerable<T>> GetAllAsync(Dictionary<string, string> queryParams, List<string> includeProperties = null)
        {
            var query = context.Set<T>().AsQueryable();
            query = ApplyFilters(query, queryParams);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            if (queryParams.ContainsKey("pageNumber"))
            {
                int pageNumber = int.Parse(queryParams["pageNumber"]);
                query = query.Take(3 * pageNumber); // Limit records
            }
            return await query.ToListAsync();
        }
        public async Task<T> GetByIDAsync(Guid id, List<string> includeProperties = null)
        {
            var query = context.Set<T>().AsQueryable();
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            //return await context.Set<T>().FindAsync(id);
        }
        #endregion

    }
}