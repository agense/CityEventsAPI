using System.Collections.Generic;

namespace EventsApi.Dto
{
    public class EventResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string StartTime { get; set; }

        public string EndDate { get; set; }

        public string EndTime { get; set; }

        public string Location { get; set; }

        public string Website { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public List<CategoryResponse> Categories { get; set; }
    }
}
