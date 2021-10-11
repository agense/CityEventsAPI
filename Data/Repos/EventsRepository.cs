using Business.Interfaces;
using Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CityEventEntity = Data.Entities.CityEvent;
using CategoryEntity = Data.Entities.Category;
using Data.Interfaces;

namespace Data.Repos
{
    public class EventsRepository : IEventsRepository
    {
        private readonly EventsContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventEntityMapper _eventMapper;

        public EventsRepository(
            EventsContext context, 
            ICategoryRepository categoryRepository,
            IEventEntityMapper eventMapper
            )
        {
            _dbContext = context;
            _categoryRepository = categoryRepository;
            _eventMapper = eventMapper;
        }

        public async Task<IEnumerable<CityEvent>> All()
        {
            return await _dbContext.Events
                .Include(e => e.Categories)
                .OrderByDescending(e => e.Start)
                .Select(ev => _eventMapper.Map(ev))
                .ToListAsync();
        }

        public async Task<IEnumerable<CityEvent>> FutureEvents()
        {
            return await _dbContext.Events
                .Where(e => e.Start >= DateTime.Now)
                .Include(e => e.Categories)
                .OrderBy(e => e.Start)
                .Select(ev => _eventMapper.Map(ev)).
                ToListAsync();
        }

        public async Task<IEnumerable<CityEvent>> PastEvents()
        {
            return await _dbContext.Events
                .Where(e => e.Start < DateTime.Now)
                .Include(e => e.Categories)
                .OrderByDescending(e => e.Start)
                .Select(ev => _eventMapper.Map(ev))
                .ToListAsync();
        }

        public async Task<long> Count()
        {
            return await _dbContext.Events.CountAsync();
        }

        public async Task<CityEvent> Create(CityEvent model)
        {
            var newEvent = new CityEventEntity
            {
                Title = model.Title,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                PriceFrom = model.PriceFrom,
                PriceTo = model.PriceTo,
                Location = model.Location,
                Website = model.Website,
                Categories = new List<CategoryEntity>()
            };

            AddEventCategories(newEvent, model.Categories);

            var result = await _dbContext.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();
            return _eventMapper.Map(result.Entity);
            
        }

        public async Task<CityEvent> Update(int id, CityEvent model)
        {
            var cEvent = await _dbContext.Events.
                Include(e => e.Categories).
                FirstOrDefaultAsync(c => c.Id == id);
            if (cEvent == null) return null;

            cEvent.Title = model.Title;
            cEvent.Description = model.Description;
            cEvent.Start = model.Start;
            cEvent.End = model.End;
            cEvent.PriceFrom = model.PriceFrom;
            cEvent.PriceTo = model.PriceTo;
            cEvent.Location = model.Location;
            cEvent.Website = model.Website;

            UpdateEventCategories(cEvent, model.Categories);

            await _dbContext.SaveChangesAsync();
            return _eventMapper.Map(cEvent);
        }

        public async Task Delete(int id)
        {
            var ev = await _dbContext.Events.FirstOrDefaultAsync(c => c.Id == id);
            if (ev != null)
            {
                _dbContext.Remove(ev);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _dbContext.Events.AnyAsync(c => c.Id == id);
        }

        public async Task<CityEvent> FindOne(int id)
        {
            var ev = await _dbContext.Events.
                Include(e => e.Categories).
                FirstOrDefaultAsync(c => c.Id == id);
            if (ev == null) return null;
            return _eventMapper.Map(ev);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        private void AddEventCategories(CityEventEntity cEvent, List<Category> categories)
        {
            if (categories.Count() > 0)
            {
                cEvent.Categories.AddRange(categories
                .Select(category => _dbContext.Categories
                .FirstOrDefault(c => c.Id == category.Id)));
            }
        }

        private void UpdateEventCategories(CityEventEntity cEvent, List<Category> newCategories)
        {
            var newCategoryIds = newCategories.Select(c => c.Id).ToArray();
            var existingCategoryIds = cEvent.Categories.Select(c => c.Id).ToArray();

            //Remove all categories not present in model categories
            if (cEvent.Categories.Count() > 0)
            {
                cEvent.Categories.RemoveAll(c => !newCategoryIds.Contains(c.Id));
            }
            //Add new categories
            AddEventCategories(cEvent, newCategories.Where(c => !existingCategoryIds.Contains(c.Id)).ToList() );
        }
    }
}
