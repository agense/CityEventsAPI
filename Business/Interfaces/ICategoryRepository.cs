using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<bool> AllExist(int[] ids);

        public Task<Category> FindOneWithRelatedEvents(int key);

        public Task<Category> FindOneWithRelatedFutureEvents(int key);

    }
}
