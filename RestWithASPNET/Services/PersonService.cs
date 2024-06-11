using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;

namespace RestWithASPNET.Services
{
	public class PersonService(MySQLContext context) : IPersonService
	{
		private readonly MySQLContext _context = context;

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

		public Person? FindById(long id)
		{
			return _context.People.SingleOrDefault(p => p.Id.Equals(id));
		}

		public Person Update(Person person)
		{
			var exists = FindById(person.Id);
			if (exists == null)
				return new Person();

			try
			{
				_context.Entry(exists).CurrentValues.SetValues(person);
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}

			return exists;
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
	}
}
