using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business
{
	public class PersonBusiness(IPersonRepository repository) : IPersonBusiness
	{
		private readonly IPersonRepository _repository = repository;

		public Person Create(Person person)
		{
			return _repository.Create(person);
		}

		public ICollection<Person> FindAll()
		{
			return _repository.FindAll();
		}

		public Person FindById(long id)
		{
			return _repository.FindById(id);
		}

		public Person Update(Person person)
		{
			return _repository.Update(person);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}
	}
}
