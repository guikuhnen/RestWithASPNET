namespace RestWithASPNET.Data.VO
{
	public class TokenVO(bool authenticated, string created, string expiration, string accessToken, string refreshToken)
	{
		public bool Authenticated { get; set; } = authenticated;

		public string Created { get; set; } = created;

		public string Expiration { get; set; } = expiration;

		public string AccessToken { get; set; } = accessToken;

		public string RefreshToken { get; set; } = refreshToken;
	}
}
