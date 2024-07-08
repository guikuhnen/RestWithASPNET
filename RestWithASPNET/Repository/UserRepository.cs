using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Repository
{
	public class UserRepository(MySQLContext context) : IUserRepository
	{
		private readonly MySQLContext _context = context;

		public User? ValidateCredentials(UserVO user)
		{
			var pass = ComputeHash(user.Password, SHA256.Create());

			return _context.Users.FirstOrDefault(u =>
						(u.UserName == user.UserName) && (u.Password == pass));
		}

		private static string ComputeHash(string input, HashAlgorithm hashAlgorithm)
		{
			var hashedBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

			var sBuilder = new StringBuilder();

			foreach (var item in hashedBytes)
				sBuilder.Append(item.ToString("x2"));

			return sBuilder.ToString();
		}

		public User? RefreshUserInfo(User user)
		{
			var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
			if (result != null)
				try
				{
					_context.Users.Entry(result).CurrentValues.SetValues(user);
					_context.SaveChanges();

					return result;
				}
				catch (Exception)
				{
					throw;
				}

			return null;
		}
	}
}
