using RestWithASPNET.Model;

namespace RestWithASPNET.Services
{
    public interface IPersonService
    {
        Person Create(Person person);
        Person FindById(long id);
        ICollection<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);
    }
}
