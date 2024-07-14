using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Repository
{
	public interface IUserRepository
	{
		User? ValidateCredentials(UserVO user);
		User? ValidateCredentials(string userName);
		User? RefreshUserInfo(User user);
		bool RevokeToken(string userName);
	}
}
