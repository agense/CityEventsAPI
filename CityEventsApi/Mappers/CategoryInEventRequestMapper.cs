using EventsApi.Interfaces;
using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Mappers
{
    public class CategoryInEventRequestMapper : ICategoryInEventRequestMapper
    {
        public Category Map(dynamic request)
        {
            return new Category{ Id = request.Id, Name = request.Name };
        }

    }
}