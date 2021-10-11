using System.ComponentModel.DataAnnotations;

namespace EventsApi.Dto
{
    public class CategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
