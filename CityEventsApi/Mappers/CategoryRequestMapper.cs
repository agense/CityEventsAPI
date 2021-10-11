using EventsApi.Interfaces;
using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Mappers
{
    public class CategoryRequestMapper : ICategoryRequestMapper
    {
        public Category Map(CategoryRequest request)
        {
            return new Business.Models.Category { Name = request.Name };
        }
    }
}