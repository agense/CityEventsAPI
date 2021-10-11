using Data.Interfaces;

namespace Data.Mappers
{
    public class CategoryEntityMapper : ICategoryEntityMapper
    {
        public  Business.Models.Category Map(Data.Entities.Category category)
        {
            return new Business.Models.Category
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
    }
}
