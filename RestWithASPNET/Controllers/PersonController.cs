using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Hypermedia.Utils;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Authorize("Bearer")]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness) : ControllerBase
	{
		private readonly ILogger<PersonController> _logger = logger;
		private readonly IPersonBusiness _personBusiness = personBusiness;

		[HttpPost]
		[ProducesResponseType(200, Type = typeof(PersonVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Create(person));
		}

		[HttpGet("{sortDirection}/{pageSize}/{currentPage}")]
		[ProducesResponseType(200, Type = typeof(PagedSearchVO<PersonVO>))]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult GetAll([FromQuery] string? name, string sortDirection, int pageSize, int currentPage)
		{
			return Ok(_personBusiness.FindAllWithPagedSearch(name, sortDirection, pageSize, currentPage));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(PersonVO))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Get(long id)
		{
			var person = _personBusiness.FindById(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}

		[HttpPut]
		[ProducesResponseType(200, Type = typeof(PersonVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] PersonVO person)
		{
			if (person == null)
				return BadRequest();

			return Ok(_personBusiness.Update(person));
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult Delete(long id)
		{
			var person = _personBusiness.FindById(id);

			if (person == null)
				return NotFound();

			_personBusiness.Delete(id);

			return NoContent();
		}

		[HttpPatch("{id}")]
		[ProducesResponseType(200, Type = typeof(PersonVO))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Patch(long id)
		{
			var person = _personBusiness.SetStatus(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}


		[HttpGet("findPersonByName")]
		[ProducesResponseType(200, Type = typeof(List<PersonVO>))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult FindPersonByName([FromQuery]string? firstName, [FromQuery]string? lastName)
		{
			var people = _personBusiness.FindAllByName(firstName, lastName);

			if (people == null)
				return NotFound();

			return Ok(people);
		}
	}
}
