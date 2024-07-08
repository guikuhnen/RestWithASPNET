using Microsoft.IdentityModel.Tokens;
using RestWithASPNET.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Services
{
	public class TokenService(TokenConfiguration configuration) : ITokenService
	{
		private readonly TokenConfiguration _configuration = configuration;

		public string GenerateAccessToken(IEnumerable<Claim> claims)
		{
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: _configuration.Issuer,
				audience: _configuration.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_configuration.Minutes),
				signingCredentials: signinCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);

				return Convert.ToBase64String(randomNumber);
			};
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
				ValidateLifetime = false,
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
				throw new SecurityTokenException("Invalid Token!");

			return principal;
		}
	}
}
