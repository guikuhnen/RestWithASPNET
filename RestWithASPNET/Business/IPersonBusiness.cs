using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
	public interface IPersonBusiness
    {
        Person Create(Person person);
        ICollection<Person> FindAll();
		Person? FindById(long id);
		Person? Update(Person person);
        void Delete(long id);
    }
}
