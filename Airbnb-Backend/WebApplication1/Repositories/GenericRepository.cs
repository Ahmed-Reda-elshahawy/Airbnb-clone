﻿using AutoMapper;
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
            int pageNumber = queryParams.ContainsKey("pageNumber") ? int.Parse(queryParams["pageNumber"]) : 1;
            int pageSize = queryParams.ContainsKey("pageSize") ? int.Parse(queryParams["pageSize"]) : 2;
            query = query.Take(pageSize * pageNumber);
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