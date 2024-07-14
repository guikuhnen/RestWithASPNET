using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
	public interface ILoginBusiness
	{
		TokenVO? ValidateCredentials(UserVO userCredentials);
		TokenVO? ValidateCredentials(TokenVO token);
		bool RevokeToken(string userName);
	}
}
