using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Interfaces
{
    public interface ICategoryInEventRequestMapper
    {
        public Category Map(dynamic request);
    }
}
