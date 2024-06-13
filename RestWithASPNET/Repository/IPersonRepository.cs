using RestWithASPNET.Model;

namespace RestWithASPNET.Repository
{
	public interface IPersonRepository
	{
        Person Create(Person person);
        Person FindById(long id);
        ICollection<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);
        bool Exists(long id);
    }
}
