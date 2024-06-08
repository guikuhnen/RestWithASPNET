using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace RestWithASPNET.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CalculatorController : ControllerBase
	{
		private readonly ILogger<CalculatorController> _logger;

		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpGet("sum/{firstNumber}/{secondNumber}")]
		public IActionResult Get(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);

				return Ok(sum.ToString());
			}

			return BadRequest("Invalid input!");
		}

		private static bool IsNumeric(string strNumber)
		{
			bool isNumber = double.TryParse(strNumber, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
			return isNumber;
		}

		private static decimal ConvertToDecimal(string strNumber)
		{
			if (decimal.TryParse(strNumber, out decimal decimalValue))
				return decimalValue;

			return 0;
		}
	}
}
