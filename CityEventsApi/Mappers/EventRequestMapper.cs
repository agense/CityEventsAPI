using EventsApi.Interfaces;
using Business.Models;
using System.Collections.Generic;
using EventsApi.Dto;

namespace EventsApi.Mappers
{
    public class EventRequestMapper : IEventRequestMapper
    {
        private readonly ICategoryInEventRequestMapper _categoryMapper;

        public EventRequestMapper(ICategoryInEventRequestMapper categoryMapper)
        {
            _categoryMapper = categoryMapper;
        }

        public CityEvent Map(EventRequest request)
        {
            var model = new Business.Models.CityEvent
            {
                Title = request.Title,
                Description = request.Description,
                Start = request.Start,
                End = request.End,
                PriceFrom = request.PriceFrom,
                PriceTo = request.PriceTo,
                Location = request.Location,
                Website = request.Website,
                Categories = new List<Business.Models.Category>(),
            };

            foreach (var category in (IEnumerable<dynamic>)request.Categories)
            {
                model.Categories.Add(_categoryMapper.Map(category));
            }
            return model;
        }
    }
}