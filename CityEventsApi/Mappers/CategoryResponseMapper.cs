using EventsApi.Interfaces;
using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Mappers
{
    public class CategoryResponseMapper : ICategoryResponseMapper
    {
        public CategoryResponse Map(Category model)
        {
            return new CategoryResponse { Id = model.Id, Name = model.Name };
        }
    }
}