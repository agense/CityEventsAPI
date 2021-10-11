using Business.Interfaces;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Data.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EventsContext _dbContext;
        private readonly ICategoryEntityMapper _categoryMapper;
        private readonly ICategoryEntityWithEventsMapper _categoryWithEventsMapper;

        public CategoryRepository(
            EventsContext context, 
            ICategoryEntityMapper categoryMapper, 
            ICategoryEntityWithEventsMapper categoryWithEventsMapper
        )
        {
            _dbContext = context;
            _categoryMapper = categoryMapper;
            _categoryWithEventsMapper = categoryWithEventsMapper;
        }

        public async Task<IEnumerable<Business.Models.Category>> All()
        {
            return await _dbContext.Categories.OrderBy(c =>c.Name).Select( c => _categoryMapper.Map(c)).ToListAsync();
        }

        public async Task<long> Count()
        {
            return await _dbContext.Categories.CountAsync();
        }

        public async Task<Business.Models.Category> Create(Business.Models.Category model)
        {
            var category = new Data.Entities.Category
            {
                Id = model.Id,
                Name = model.Name,
            };
            var result = await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return _categoryMapper.Map(result.Entity);
        }

        public async Task<Business.Models.Category> Update(int id, Business.Models.Category model)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return null;

            category.Name = model.Name;

            await _dbContext.SaveChangesAsync();
            return _categoryMapper.Map(category);
        }

        public async Task Delete(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null) {
                 _dbContext.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
             return await _dbContext.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> AllExist(int[] categories) {

            var validIds = await _dbContext.Categories.Where(c => categories.Contains(c.Id)).Select(c => c.Id).ToListAsync();
            return categories.Except(validIds).Count() == 0; 
        }

        public async Task<Business.Models.Category> FindOne(int id)
        {
             var category =  await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
             if (category == null) return null;
             return _categoryMapper.Map(category);
        }

        public async Task<Business.Models.Category> FindOneWithRelatedEvents(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Events)
                .ThenInclude(e =>e.Categories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;
            return _categoryWithEventsMapper.Map(category);
        }

        public async Task<Business.Models.Category> FindOneWithRelatedFutureEvents(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Events.Where(e => e.Start >= DateTime.Now))
                .ThenInclude(e => e.Categories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;
            return _categoryWithEventsMapper.Map(category);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync(); 
        }
    }
}
