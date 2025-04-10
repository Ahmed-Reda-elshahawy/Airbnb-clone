using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace WebApplication1.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID(int id);
        void Create(T entity);
        void UpdateAsync(T entity);
        void Delete(T entity);
        void Save();

        #region Async
        Task<IEnumerable<T>> GetAllAsync(Dictionary<string, string> queryParams);
        Task CreateAsync<T>(T entity) where T:class;
        Task<T> UpdateAsync<T, TDto>(Guid id, TDto updateDto) where T : class;
        Task DeleteAsync(Guid id);
        Task<T> GetByIDAsync(Guid id);
        #endregion
    }
}

