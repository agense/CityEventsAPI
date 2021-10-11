using EventsApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventsApi.Interfaces
{
    public interface IEventRequestValidator
    {
        public Dictionary<string, string> GetErrors();

        public Task<bool> IsValid(EventRequest request);
    }
}
