using Business.Models;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Data.Mappers
{
    public class EventEntityMapper : IEventEntityMapper
    {
        public CityEvent Map(Data.Entities.CityEvent cEvent)
        {
            var eventModel = new CityEvent
            {
                Id = cEvent.Id,
                Title = cEvent.Title,
                Description = cEvent.Description,
                Start = cEvent.Start,
                End = cEvent.End,
                PriceFrom = cEvent.PriceFrom,
                PriceTo = cEvent.PriceTo,
                Location = cEvent.Location,
                Website = cEvent.Website,
                Categories = new List<Business.Models.Category>()

            };
            if (cEvent.Categories.Count() > 0)
            {
                eventModel.Categories.AddRange(cEvent.Categories.Select(c => new Business.Models.Category { Id = c.Id, Name = c.Name }));
            }
            return eventModel;
        }
    }
}
