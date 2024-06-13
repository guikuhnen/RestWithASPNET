using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
	public interface IPersonBusiness
    {
        Person Create(Person person);
        Person FindById(long id);
        ICollection<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);
    }
}
