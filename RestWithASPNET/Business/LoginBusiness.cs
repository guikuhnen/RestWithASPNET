using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNET.Configurations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Repository;
using RestWithASPNET.Services;
using System.Security.Claims;

namespace RestWithASPNET.Business
{
	public class LoginBusiness(TokenConfiguration configuration, IUserRepository userRepository, ITokenService tokenService) : ILoginBusiness
	{
		private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

		private readonly IUserRepository _userRepository = userRepository;
		private readonly ITokenService _tokenService = tokenService;
		private readonly TokenConfiguration _configuration = configuration;

		public TokenVO? ValidateCredentials(UserVO userCredentials)
		{
			var user = _userRepository.ValidateCredentials(userCredentials);
			if (user == null)
				return null;

			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
				new(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};

			var accessToken = _tokenService.GenerateAccessToken(claims);
			var refreshToken = _tokenService.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpire);

			var createDate = DateTime.Now;
			var expirationDate = createDate.AddMinutes(_configuration.Minutes);

			_userRepository.RefreshUserInfo(user);

			return new TokenVO(true, createDate.ToString(DATE_FORMAT), expirationDate.ToString(DATE_FORMAT), accessToken, refreshToken);
		}
	}
}
