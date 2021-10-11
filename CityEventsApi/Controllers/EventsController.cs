using Business;
using Business.Interfaces;
using Business.Interfaces.Log;
using EventsApi.Dto;
using EventsApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsRepository _eventRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventResponseMapper _eventResponseMapper;
        private readonly IEventRequestMapper _eventRequestMapper;
        private readonly LoggingOptions _logOptions;
        private readonly IErrorLog _logger;
        private readonly IEventRequestValidator _eventValidator;

        public EventsController(
            IEventsRepository eventRepository,
            ICategoryRepository categoryRepository,
            IEventRequestMapper eventRequestMapper,
            IEventResponseMapper eventResponseMapper,
            IOptions<LoggingOptions> logOptions,
            IErrorLog logger,
            IEventRequestValidator eventValidator
            )
        {
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _eventResponseMapper = eventResponseMapper;
            _eventRequestMapper = eventRequestMapper;
            _logOptions = logOptions.Value;
            _logger = logger;
            _eventValidator = eventValidator;
        }

        /// <summary>
        /// Get a collection of upcoming city events    
        /// </summary>
        /// <returns>A collection of upcoming city events  </returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<EventResponse>>> Get()
        {
            try
            {
                var data = await _eventRepository.FutureEvents();
                List<EventResponse> responseData = new List<EventResponse>();

                data.ToList().ForEach(c => responseData.Add(_eventResponseMapper.Map(c)));
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }   
        }

        /// <summary>
        /// Get a collection of city events that have passed    
        /// </summary>
        /// <returns>A collection of city events that have passed</returns>
        [HttpGet]
        [Route("passed")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<EventResponse>>> GetPassedEvents()
        {
            try
            {
                var data = await _eventRepository.PastEvents();
                List<EventResponse> responseData = new List<EventResponse>();

                data.ToList().ForEach(c => responseData.Add(_eventResponseMapper.Map(c)));
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
        }
        /// <summary>
        /// Gets a single event by id
        /// </summary>
        /// <param name="key">Event id</param>
        /// <returns>Single event</returns>
        [HttpGet]
        [Route("{key:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<EventResponse>> GetOne(int key)
        {
            try
            {
                if (!await _eventRepository.Exists(key)) return NotFound();

                var cityEvent = await _eventRepository.FindOne(key);
                return Ok(_eventResponseMapper.Map(cityEvent));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            } 
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        /// <param name="request">Event request data</param>
        /// <returns>Created Event</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<EventResponse>> Post([FromBody] EventRequest request)
        {
            try
            {
                if (! await _eventValidator.IsValid(request)){
                    foreach (var error in _eventValidator.GetErrors())
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var created = await _eventRepository.Create(_eventRequestMapper.Map(request));
                return CreatedAtAction(nameof(GetOne), new { key = created.Id }, _eventResponseMapper.Map(created));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
        }

        /// <summary>
        /// Update Existing Event Data
        /// </summary>
        /// <param name="key">Event id</param>
        /// <param name="request">Event request data</param>
        /// <returns>Updated Event</returns>
        [HttpPut("{key:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<EventResponse>> Update([FromRoute] int key, [FromBody] EventRequest request)
        {
            try
            {
                if (!await _eventRepository.Exists(key)) return NotFound();

                if (!await _eventValidator.IsValid(request))
                {
                    foreach (var error in _eventValidator.GetErrors())
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _eventRepository.Update(key, _eventRequestMapper.Map(request));
                return Ok(_eventResponseMapper.Map(updated));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
        }

        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <param name="key">Event id</param>
        /// <returns>Void</returns>
        [HttpDelete("{key:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Delete(int key)
        {
            try
            {
                if (!await _eventRepository.Exists(key)) return NotFound();
                await _eventRepository.Delete(key);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            } 
        }

       
    }
}
