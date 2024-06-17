using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;

namespace RestWithASPNET.Repository
{
	public class BookRepository(MySQLContext context) : IBookRepository
	{
		private MySQLContext _context = context;

		public Book Create(Book person)
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

		public ICollection<Book> FindAll()
		{
			return [.. _context.Books];
		}

		public Book FindById(long id)
		{
			return _context.Books.SingleOrDefault(p => p.Id.Equals(id));
		}

		public Book Update(Book person)
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
					_context.Books.Remove(person);
					_context.SaveChanges();
				}
				catch (Exception)
				{
					throw;
				}
		}

		public bool Exists(long id)
		{
			return _context.Books.Any(p => p.Id.Equals(id));
		}
	}
}
