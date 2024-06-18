using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Model;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BookController(ILogger<BookController> logger, IBookBusiness bookBusiness) : ControllerBase
    {
        private readonly ILogger<BookController> _logger = logger;
        private readonly IBookBusiness _bookBusiness = bookBusiness;

		[HttpPost]
		public IActionResult Create([FromBody] Book book)
		{
			if (book == null) 
                return BadRequest();

			return Ok(_bookBusiness.Create(book));
		}

		[HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bookBusiness.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var book = _bookBusiness.FindById(id);

            if (book == null) 
                return NotFound();

            return Ok(book);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Book book)
        {
            if (book == null) 
                return BadRequest();

            return Ok(_bookBusiness.Update(book));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
		{
			var book = _bookBusiness.FindById(id);

			if (book == null)
				return NotFound();

			return NoContent();
        }
    }
}
