using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class CityEvent
    {
        [Key]
        public int Id { get; set; }

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

        public string Website { get; set; }

        [Column(TypeName = "decimal(8,2)")]      
        public decimal PriceFrom { get; set; }

        [Column(TypeName = "decimal(8,2)")]  
        public decimal PriceTo { get; set; }

        public List<Category> Categories { get; set; }
    }
}
