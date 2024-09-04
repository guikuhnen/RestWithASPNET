using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.HATEOAS.Hypermedia.Filters;
using RestWithASPNET.HATEOAS.Hypermedia.Utils;
using System.Xml.Linq;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
    [ApiController]
	[Authorize("Bearer")]
	[Route("api/[controller]/v{version:apiVersion}")]
    public class BookController(ILogger<BookController> logger, IBookBusiness bookBusiness) : ControllerBase
    {
        private readonly ILogger<BookController> _logger = logger;
        private readonly IBookBusiness _bookBusiness = bookBusiness;

		[HttpPost]
		[ProducesResponseType(200, Type = typeof(BookVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Create([FromBody] BookVO book)
		{
			if (book == null) 
                return BadRequest();

			return Ok(_bookBusiness.Create(book));
		}

		[HttpGet("{sortDirection}/{pageSize}/{currentPage}")]
		[ProducesResponseType(200, Type = typeof(PagedSearchVO<BookVO>))]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult GetAll([FromQuery] string? title, string sortDirection, int pageSize, int currentPage)
		{
            return Ok(_bookBusiness.FindAllWithPagedSearch(title, sortDirection, pageSize, currentPage));
        }

        [HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(BookVO))]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Get(long id)
        {
            var book = _bookBusiness.FindById(id);

            if (book == null) 
                return NotFound();

            return Ok(book);
        }

        [HttpPut]
		[ProducesResponseType(200, Type = typeof(BookVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult Update([FromBody] BookVO book)
        {
            if (book == null) 
                return BadRequest();

            return Ok(_bookBusiness.Update(book));
        }

        [HttpDelete("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
		public IActionResult Delete(long id)
		{
			var book = _bookBusiness.FindById(id);

			if (book == null)
				return NotFound();

			_bookBusiness.Delete(id);

			return NoContent();
        }
    }
}
