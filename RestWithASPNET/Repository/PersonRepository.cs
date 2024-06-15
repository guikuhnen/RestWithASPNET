using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;

namespace RestWithASPNET.Repository
{
	public class PersonRepository(MySQLContext context) : IPersonRepository
	{
		private MySQLContext _context = context;

		public Person Create(Person person)
		{
			try
			{
				_context.Add(person);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}

			return person;
		}

		public ICollection<Person> FindAll()
		{
			return [.. _context.People];
		}

		public Person FindById(long id)
		{
			return _context.People.SingleOrDefault(p => p.Id.Equals(id));
		}

		public Person Update(Person person)
		{
			if (!Exists(person.Id))
				return null;

			var result = FindById(person.Id);
			try
			{
				_context.Entry(result).CurrentValues.SetValues(person);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}

			return result;
		}

		public void Delete(long id)
		{
			var person = FindById(id);

			if (person != null)
				try
				{
					_context.People.Remove(person);
					_context.SaveChanges();
				}
				catch (Exception)
				{
					throw;
				}
		}

		public bool Exists(long id)
		{
			return _context.People.Any(p => p.Id.Equals(id));
		}
	}
}
