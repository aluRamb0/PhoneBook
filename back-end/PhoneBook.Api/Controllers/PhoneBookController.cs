using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneBook.Services;
using PhoneBook.Services.Mapping;

namespace PhoneBook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly PhoneBookService _service;

        public PhoneBookController(PhoneBookService service, IMapper mapper, ILogger<PhoneBookController> logger)
        {
            _service = service;
            Logger = logger;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }
        private ILogger<PhoneBookController> Logger { get; }

        [HttpGet]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(typeof(IEnumerable<PhoneBookDto>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get()
        {
            var records = await _service.GetQuery().AsNoTracking().ToListAsync();
            return Ok(Mapper.Map<IEnumerable<PhoneBookDto>>(records));
        }

        [HttpGet("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(typeof(PhoneBookDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PhoneBookDto>> Get(Guid id)
        {
            var record = await _service.GetQuery().AsNoTracking().FirstOrDefaultAsync(r=> r.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpPost]
        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post(CreatePhoneBookDto input)
        {
            var id = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(Post),id);
        }

        [HttpPut("{id}")]
        [Produces("application/json", "application/xml")]
        [Consumes("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreatePhoneBookDto input)
        {
            Expression<Func<PhoneBook, bool>> expression = r => r.Id == id;
            if (!await _service.PhoneBookExistsAsync(expression))
            {
                Logger.LogTrace($"{nameof(PhoneBook)} record not found");
                return NotFound();
            }

            var updateInput = Mapper.Map<PhoneBookDto>(input);
            updateInput.Id = id;
            await _service.UpdateAsync(updateInput);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _service.PhoneBookExistsAsync(r=> r.Id == id))
            {
                Logger.LogTrace($"{nameof(PhoneBook)} record not found");
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/Entry/{entryId}", Name = "DeleteEntry")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(Guid id, Guid entryId)
        {
            if (!await _service.PhoneBookExistsAsync(r=> r.Id == id))
            {
                Logger.LogTrace($"{nameof(PhoneBook)} record not found");
                return NotFound();
            }
            if (!await _service.EntryExistsAsync(r=> r.Id == entryId))
            {
                Logger.LogTrace($"{nameof(Entry)} record not found");
                return NotFound();
            }
            await _service.DeleteEntryAsync(entryId);
            return Ok();
        }
    }
}