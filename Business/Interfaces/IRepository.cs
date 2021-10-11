using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> All();

        Task<bool> Exists(int key);

        Task<T> FindOne(int key);

        Task<T> Create(T model);

        Task<T> Update(int key, T model);

        Task Delete(int key);

        Task SaveChanges();

        Task<long> Count();
        
    }
}
