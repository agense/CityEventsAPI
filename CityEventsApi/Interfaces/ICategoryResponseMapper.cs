using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Interfaces
{
    public interface ICategoryResponseMapper
    {
        public CategoryResponse Map(Category model);
    }
}
