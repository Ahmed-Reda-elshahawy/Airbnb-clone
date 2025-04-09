using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Interfaces;
using WebApplication1.Models;

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
        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        #region Delete Method
        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Set<Listing>().FindAsync(id);  // Find the Listing by its Id

            if (entity != null)
            {
                context.Set<Listing>().Remove(entity);  // Remove the entity from the DbSet
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

        #region Filtering Method
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
        public async Task<IEnumerable<T>> GetAllAsync(Dictionary<string, string> queryParams)
        {
            var query = context.Set<T>().AsQueryable();
            query = ApplyFilters(query, queryParams);
            return await query.ToListAsync();
        }
        public async Task<T> GetByIDAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        #endregion
    }
}


//var entityType = typeof(T);
//var dtoType = typeof(TDto);


//foreach (var property in dtoType.GetProperties())
//{
//    // Check if the property value in the DTO is null
//    var value = property.GetValue(updateDto);

//    // If the property is null, preserve the existing value in the entity
//    if (value == null || value.ToString() == "0")
//    {
//        Console.WriteLine("ffffffffffffffffffffffffffffFFFFFFFFFFFFFF");
//        var entityProperty = entityType.GetProperty(property.Name);
//        if (entityProperty != null)
//        {
//            Console.WriteLine("fffffffsdjaizfhiojzpiefjqhffffffffffffffffffffffFFFFFFFFFFFFFF");

//            // Get the current value of the property in the entity
//            var currentEntityValue = entityProperty.GetValue(entity);

//            // Set the entity property value back to its current value (if the DTO value is null)
//            entityProperty.SetValue(entity, currentEntityValue);
//        }
//    }
//}