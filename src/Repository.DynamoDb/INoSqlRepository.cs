using System;
using System.Threading.Tasks;

namespace Repository.Abstractions
{
    public interface INoSqlRepository<T>
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetByIdAsync(string id);
    }
}
