using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Model.Base;
using RestWithASPNET.Model.Context;

namespace RestWithASPNET.Repository.Base
{
	public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
	{
		protected MySQLContext _context;
		private DbSet<T> _dbSet;

		public BaseRepository(MySQLContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public T Create(T obj)
		{
			try
			{
				_dbSet.Add(obj);
				_context.SaveChanges();

				return obj;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ICollection<T> FindAll()
		{
			return [.. _dbSet];
		}

		public T FindById(long id)
		{
			return _dbSet.SingleOrDefault(p => p.Id.Equals(id));
		}

		public T Update(T obj)
		{
			var result = FindById(obj.Id);
			if (result != null)
				try
				{
					_dbSet.Entry(result).CurrentValues.SetValues(obj);
					_context.SaveChanges();

					return result;
				}
				catch (Exception)
				{
					throw;
				}

			return null;
		}

		public void Delete(long id)
		{
			var result = FindById(id);
			if (result != null)
				try
				{
					_dbSet.Remove(result);
					_context.SaveChanges();
				}
				catch (Exception)
				{
					throw;
				}
		}

		public bool Exists(long id)
		{
			return _dbSet.Any(p => p.Id.Equals(id));
		}

		public ICollection<T> FindAllWithPagedSearch(string query)
		{
			return [.._dbSet.FromSqlRaw<T>(query)];
		}

		public int GetCount(string query)
		{
			var result = "";

			using (var connection = _context.Database.GetDbConnection())
			{
				connection.Open();

				using var command = connection.CreateCommand();
				command.CommandText = query;
				result = command.ExecuteScalar()?.ToString();
			}

			_ = int.TryParse(result, out int value);

			return value;
		}
	}
}
