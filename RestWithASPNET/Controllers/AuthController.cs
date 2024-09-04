using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.HATEOAS.Hypermedia.Filters;
using RestWithASPNET.Model;

namespace RestWithASPNET.Controllers
{
	[ApiVersion("1")]
	[ApiController]
	[Route("api/[controller]/v{version:apiVersion}")]
	public class AuthController(ILogger<AuthController> logger, ILoginBusiness loginBusiness) : ControllerBase
	{
		private readonly ILogger<AuthController> _logger = logger;
		private readonly ILoginBusiness _loginBusiness = loginBusiness;

		[HttpPost]
		[Route("signin")]
		[ProducesResponseType(200, Type = typeof(TokenVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(500)]
		public IActionResult SignIn([FromBody] UserVO user)
		{
			if (user == null)
				return BadRequest();

			var token = _loginBusiness.ValidateCredentials(user);

			if (token == null)
				return Unauthorized();

			return Ok(token);
		}

		[HttpPost]
		[Route("refresh")]
		[ProducesResponseType(200, Type = typeof(TokenVO))]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public IActionResult Refresh([FromBody] TokenVO tokenVO)
		{
			if (tokenVO == null)
				return BadRequest();

			var token = _loginBusiness.ValidateCredentials(tokenVO);

			if (token == null)
				return BadRequest();

			return Ok(token);
		}

		[HttpGet]
		[Route("revoke")]
		[Authorize("Bearer")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(500)]
		public IActionResult Revoke()
		{
			var userName = User.Identity?.Name;

			if (string.IsNullOrWhiteSpace(userName))
				return BadRequest();

			var result = _loginBusiness.RevokeToken(userName);

			if (!result)
				return BadRequest();

			return NoContent();
		}
	}
}
