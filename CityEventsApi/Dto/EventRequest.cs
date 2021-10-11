using Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsApi.Dto
{
    public class EventRequest
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public string Location { get; set; }

        [Url]
        public string Website { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Incorrect price value. Price must be a positive number.")]
        public decimal PriceFrom { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Incorrect price value. Price must be a positive number.")]
        public decimal PriceTo { get; set; }

        public List<Category> Categories { get; set; }
    }
}
