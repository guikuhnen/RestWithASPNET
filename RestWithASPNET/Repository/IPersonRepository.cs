using RestWithASPNET.Model;
using RestWithASPNET.Repository.Base;

namespace RestWithASPNET.Repository
{
	public interface IPersonRepository : IBaseRepository<Person>
	{
		Person? SetStatus(long id);
		ICollection<Person> FindAllByName(string firstName, string lastName);
	}
}
