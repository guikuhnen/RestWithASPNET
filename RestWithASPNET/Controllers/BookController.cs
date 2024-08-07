﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Filters;

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

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(List<PersonVO>))]
		[ProducesResponseType(500)]
		[TypeFilter(typeof(HyperMediaFilter))]
		public IActionResult GetAll()
        {
            return Ok(_bookBusiness.FindAll());
        }

        [HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(PersonVO))]
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
		[ProducesResponseType(200, Type = typeof(PersonVO))]
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

			return NoContent();
        }
    }
}
