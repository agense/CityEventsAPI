using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Interfaces
{
    public interface IEventRequestMapper
    {
        public CityEvent Map(EventRequest request);
    }
}
