using Data.Interfaces;
using System.Collections.Generic;

namespace Data.Mappers
{
    public class CategoryEntityWithEventsMapper : ICategoryEntityWithEventsMapper
    {
        private readonly IEventEntityMapper _eventMapper;

        public CategoryEntityWithEventsMapper(IEventEntityMapper eventMapper)
        {
            _eventMapper = eventMapper;
        }

        public Business.Models.Category Map(Data.Entities.Category category)
        {
            var model = new Business.Models.Category
            {
                Id = category.Id,
                Name = category.Name,
                Events = new List<Business.Models.CityEvent>(),
            };
            if (category.Events.Count > 0)
            {
                foreach (var e in category.Events)
                {
                    model.Events.Add(_eventMapper.Map(e));
                }
            }
            return model;
        }
    }
}
