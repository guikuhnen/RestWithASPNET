using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Filters;

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
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpGet]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult GetAll()
		{
			return Ok(_personBusiness.FindAll());
		}

		[HttpGet("{id}")]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Get(long id)
		{
			var person = _personBusiness.FindById(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}

		[HttpPut]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] PersonVO person)
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
