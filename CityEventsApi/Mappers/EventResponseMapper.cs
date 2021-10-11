using EventsApi.Interfaces;
using Business.Models;
using EventsApi.Dto;
using System;
using System.Collections.Generic;

namespace EventsApi.Mappers
{

    public class EventResponseMapper : IEventResponseMapper
    {
        private readonly ICategoryResponseMapper _categoryResponse;

        public EventResponseMapper(ICategoryResponseMapper categoryResponse)
        {
            _categoryResponse = categoryResponse;
        }

        public EventResponse Map(CityEvent ev)
        {
            var eventResponse = new EventResponse
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDate = ev.Start.ToString("yyyy-MM-dd"),
                StartTime = ev.Start.ToString("HH:mm"),
                EndDate = ev.End.ToString("yyyy-MM-dd"),
                EndTime = ev.End.ToString("HH:mm"),
                PriceFrom = decimal.Round(ev.PriceFrom, 2, MidpointRounding.AwayFromZero),
                PriceTo = decimal.Round(ev.PriceTo, 2, MidpointRounding.AwayFromZero),
                Location = ev.Location,
                Website = ev.Website,
                Categories = new List<CategoryResponse>(),
            };
            if (ev.Categories.Count > 0)
            {
                ev.Categories.ForEach(category =>
                {
                    eventResponse.Categories.Add(_categoryResponse.Map(category));
                });
            }
            return eventResponse;
        }
    }


}