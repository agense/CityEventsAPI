using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Business.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string  Name { get; set; }

        [JsonIgnore]
        public List<CityEvent> Events { get; set; }
    }
}
