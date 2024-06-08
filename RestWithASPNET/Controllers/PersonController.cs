using Microsoft.AspNetCore.Mvc;

namespace RestWithASPNET.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly ILogger<PersonController> _logger;

		public PersonController(ILogger<PersonController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return BadRequest("Invalid input!");
		}
	}
}
