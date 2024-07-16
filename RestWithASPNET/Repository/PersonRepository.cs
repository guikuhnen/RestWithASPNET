using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Base;

namespace RestWithASPNET.Repository
{
	public class PersonRepository(MySQLContext context) : BaseRepository<Person>(context), IPersonRepository
	{
		public Person? SetStatus(long id)
		{
			var user = _context.People.SingleOrDefault(p => p.Id == id);

			if (user == null)
				return null;

			try
			{
				user.Enabled = !user.Enabled;

				_context.Entry(user).CurrentValues.SetValues(user);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}

			return user;
		}
		public ICollection<Person> FindAllByName(string firstName, string lastName)
		{
			if (firstName == null && lastName == null)
				return null;

			var people = _context.People.AsQueryable();

			if (!string.IsNullOrWhiteSpace(firstName))
				people = people.Where(p => p.FirstName.Contains(firstName));

			if (!string.IsNullOrWhiteSpace(lastName))
				people = people.Where(p => p.LastName.Contains(lastName));

			return [.. people];
		}
	}
}
