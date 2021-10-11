using Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEventsRepository : IRepository<CityEvent>
    {
        Task<IEnumerable<CityEvent>> FutureEvents();

        Task<IEnumerable<CityEvent>> PastEvents();
    }
}
