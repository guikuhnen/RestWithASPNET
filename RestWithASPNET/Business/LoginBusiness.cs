using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNET.Configurations;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
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

			return UpdateUserToken(accessToken, refreshToken, user);
		}

		public TokenVO? ValidateCredentials(TokenVO token)
		{
			var accessToken = token.AccessToken;
			var refreshToken = token.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

			var userName = principal.Identity?.Name;
			var user = _userRepository.ValidateCredentials(userName ?? string.Empty);

			if (user == null
				|| user.RefreshToken != refreshToken
				|| user.RefreshTokenExpiryTime <= DateTime.Now)
				return null;

			accessToken = _tokenService.GenerateAccessToken(principal.Claims);
			refreshToken = _tokenService.GenerateRefreshToken();

			return UpdateUserToken(accessToken, refreshToken, user);
		}

		private TokenVO UpdateUserToken(string accessToken, string refreshToken, User user)
		{
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpire);

			_userRepository.RefreshUserInfo(user);

			var createDate = DateTime.Now;
			var expirationDate = createDate.AddMinutes(_configuration.Minutes);

			return new TokenVO(true, createDate.ToString(DATE_FORMAT), expirationDate.ToString(DATE_FORMAT), accessToken, refreshToken);
		}

		public bool RevokeToken(string userName)
		{
			return _userRepository.RevokeToken(userName);
		}
	}
}
