using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Model;
using RestWithASPNET.Services;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class PersonController(ILogger<PersonController> logger, IPersonService personService) : ControllerBase
	{
		private readonly ILogger<PersonController> _logger = logger;
		private readonly IPersonService _personService = personService;

		[HttpPost]
		public IActionResult Create([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personService.Create(person));
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_personService.FindAll());
		}

		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var person = _personService.FindById(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}

		[HttpPut]
		public IActionResult Update([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personService.Update(person));
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var person = _personService.FindById(id);

			if (person == null)
				return NotFound();

			_personService.Delete(id);

			return NoContent();
		}
	}
}
