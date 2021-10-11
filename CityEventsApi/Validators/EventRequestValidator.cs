using EventsApi.Dto;
using EventsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Validators
{
    public class EventRequestValidator : IEventRequestValidator
    {

        private Dictionary<string, string> errors = new Dictionary<string, string>();
        private readonly IDateValidator _dateValidator;
        private readonly Business.Interfaces.ICategoryRepository _categoryRepository;


        public EventRequestValidator(IDateValidator dateValidator, Business.Interfaces.ICategoryRepository categoryRepository)
        {
            _dateValidator = dateValidator;
            _categoryRepository = categoryRepository;
        }

        public Dictionary<string, string> GetErrors()
        {
            return errors;
        }

        public async Task<bool> IsValid(EventRequest request)
        {
            await ValidateAllFields(request);
            return errors.Count() == 0;
        }

        private async Task ValidateAllFields(EventRequest request) 
        {
            if (!_dateValidator.ValidateFirstDateIsGreaterThanSecond(request.Start, DateTime.Now))
            {
                if (!errors.ContainsKey(nameof(request.Start)))
                {
                    errors.Add(nameof(request.Start), "Event start date cannot be before the current date");
                }
            }

            if (!_dateValidator.ValidateFirstDateIsGreaterThanSecond(request.End, DateTime.Now))
            {
                if (!errors.ContainsKey(nameof(request.End)))
                {
                    errors.Add(nameof(request.End), "Event end date cannot be before the current date");
                }
            }

            if (!_dateValidator.ValidateFirstDateIsGreaterThanSecond(request.End, request.Start))
            {
                if (!errors.ContainsKey(nameof(request.Start)))
                {
                    errors.Add(nameof(request.Start), "Event end date cannot be before the start date");
                }
            }

            //If request has categories, validate if all categories exist
            if (request.Categories.Count > 0 && !await _categoryRepository.AllExist(request.Categories.Select(c => c.Id).ToArray()))
            {
                if (!errors.ContainsKey(nameof(request.Categories)))
                {
                    errors.Add(nameof(request.Categories), "Error in category list");
                }
            }
        }
    }
}
