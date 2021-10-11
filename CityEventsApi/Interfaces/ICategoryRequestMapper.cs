using Business.Models;
using EventsApi.Dto;

namespace EventsApi.Interfaces
{
    public interface ICategoryRequestMapper
    {
        public Category Map(CategoryRequest request);
    }
}
