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
		public IActionResult Sum(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);

				return Ok(sum.ToString());
			}

			return BadRequest("Invalid input!");
		}

		[HttpGet("subtraction/{firstNumber}/{secondNumber}")]
		public IActionResult Subtraction(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var subtraction = ConvertToDecimal(firstNumber) - ConvertToDecimal(secondNumber);

				return Ok(subtraction.ToString());
			}

			return BadRequest("Invalid input!");
		}

		[HttpGet("multiplication/{firstNumber}/{secondNumber}")]
		public IActionResult Multiplication(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var multiplication = ConvertToDecimal(firstNumber) * ConvertToDecimal(secondNumber);

				return Ok(multiplication.ToString());
			}

			return BadRequest("Invalid input!");
		}

		[HttpGet("division/{firstNumber}/{secondNumber}")]
		public IActionResult Division(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var division = ConvertToDecimal(firstNumber) / ConvertToDecimal(secondNumber);

				return Ok(division.ToString());
			}

			return BadRequest("Invalid input!");
		}

		[HttpGet("mean/{firstNumber}/{secondNumber}")]
		public IActionResult Mean(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				var mean = (ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber)) / 2;

				return Ok(mean.ToString());
			}

			return BadRequest("Invalid input!");
		}

		[HttpGet("square-root/{number}")]
		public IActionResult SquareRoot(string number)
		{
			if (IsNumeric(number))
			{
				var sqrt = Math.Sqrt((double)ConvertToDecimal(number));

				return Ok(sqrt.ToString());
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
