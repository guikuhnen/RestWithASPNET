using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Model;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness) : ControllerBase
	{
		private readonly ILogger<PersonController> _logger = logger;
		private readonly IPersonBusiness _personBusiness = personBusiness;

		[HttpPost]
		public IActionResult Create([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_personBusiness.FindAll());
		}

		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var person = _personBusiness.FindById(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}

		[HttpPut]
		public IActionResult Update([FromBody] Person person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Update(person));
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var person = _personBusiness.FindById(id);

			if (person == null)
				return NotFound();

			_personBusiness.Delete(id);

			return NoContent();
		}
	}
}
