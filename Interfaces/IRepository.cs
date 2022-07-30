using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        Task<T> GetLastAsync();
        Task<bool> UpdateAsync(T entity);
    }
}