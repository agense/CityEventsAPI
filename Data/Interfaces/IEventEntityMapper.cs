using Business.Models;

namespace Data.Interfaces
{
    public interface IEventEntityMapper
    {
        public CityEvent Map(Data.Entities.CityEvent cEvent);
    }
}
