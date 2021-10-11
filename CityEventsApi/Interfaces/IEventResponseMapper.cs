using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Interfaces
{
    public interface IEventResponseMapper
    {
        public EventResponse Map(CityEvent model);
    }
}
