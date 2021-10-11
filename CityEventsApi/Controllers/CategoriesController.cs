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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ICategoryRequestMapper _requestMapper;
        private readonly ICategoryResponseMapper _responseMapper;
        private readonly IEventResponseMapper _eventResponseMapper;
        private readonly LoggingOptions _logOptions;
        private readonly IErrorLog _logger;

        public CategoriesController(
            ICategoryRepository repository,
            ICategoryRequestMapper requestMapper,
            ICategoryResponseMapper responseMapper,
            IEventResponseMapper eventResponseMapper,
            IOptions<LoggingOptions> logOptions,
            IErrorLog logger
            )
        {
            _repository = repository;
            _requestMapper = requestMapper;
            _responseMapper = responseMapper;
            _eventResponseMapper = eventResponseMapper;
            _logOptions = logOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>A collection of categories</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get()
        {
            try
            {
                var data = await _repository.All();

                List<CategoryResponse> responseData = new List<CategoryResponse>();
                data.ToList().ForEach(c => responseData.Add(_responseMapper.Map(c)));
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
        }

        /// <summary>
        /// Gets one category
        /// </summary>
        /// <param name="key">Category id</param>
        /// <returns>Single Category</returns>
        [HttpGet]
        [Route("{key:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CategoryResponse>> GetOne(int key)
        {
            try
            {
                if (!await _repository.Exists(key)) return NotFound();

                var category = await _repository.FindOne(key);
                return Ok(_responseMapper.Map(category));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
            
        }

        /// <summary>
        /// Returns events related to category
        /// </summary>
        /// <param name="key">Category id</param>
        /// <returns>A collection of upcoming events related to category</returns>
        /// <remarks>
        /// Returns a collection of upcooming events related to a category 
        ///</remarks>
        [HttpGet]
        [Route("{key:int}/events")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<EventResponse>>> GetCategoryEvents(int key) 
        {
            try
            {
                if (!await _repository.Exists(key)) return NotFound();

                var category = await _repository.FindOneWithRelatedFutureEvents(key);

                List<EventResponse> categoryEvents = new List<EventResponse>();
                category.Events.ToList().ForEach(c => categoryEvents.Add(_eventResponseMapper.Map(c)));

                return Ok(categoryEvents);
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
            
        }

        /// <summary>
        /// Creates a category
        /// </summary>
        /// <param name="request">Category Data</param>
        /// <returns>Created Category</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CategoryResponse>> Post([FromBody] CategoryRequest request)
        {
            try
            {
                var created = await _repository.Create(_requestMapper.Map(request));

                return CreatedAtAction(nameof(GetOne), new { key = created.Id }, _responseMapper.Map(created));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }

        }

        /// <summary>
        /// Updates a category
        /// </summary>
        /// <param name="key">Category id</param>
        /// <param name="request">Category data</param>
        /// <returns>Updated Category</returns>
        [HttpPut("{key:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CategoryResponse>> Update([FromRoute] int key, [FromBody] CategoryRequest request) 
        {
            try
            {
                if (!await _repository.Exists(key)) return NotFound();

                var updated = await _repository.Update(key, _requestMapper.Map(request));
                return Ok(_responseMapper.Map(updated));
            }
            catch (Exception ex)
            {
                _logger.Log(_logOptions.ErrorLog, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured.");
            }
            
        }

        /// <summary>
        /// Deletes a category and related events
        /// </summary>
        /// <param name="key">Category id</param>
        /// <returns>Void</returns>
        [HttpDelete("{key:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Delete(int key)
        {
            try
            {
                if (!await _repository.Exists(key)) return NotFound();
                await _repository.Delete(key);
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
