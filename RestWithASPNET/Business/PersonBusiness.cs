using RestWithASPNET.Data.Converter.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository.Base;

namespace RestWithASPNET.Business
{
	public class PersonBusiness(IBaseRepository<Person> repository) : IPersonBusiness
	{
		private readonly IBaseRepository<Person> _repository = repository;
		private readonly PersonConverter _converter = new();

		public PersonVO Create(PersonVO person)
		{
			var personEntity = _converter.Parse(person);

			personEntity = _repository.Create(personEntity);

			return _converter.Parse(personEntity);
		}

		public ICollection<PersonVO> FindAll()
		{
			return _converter.Parse(_repository.FindAll());
		}

		public PersonVO FindById(long id)
		{
			var person = _repository.FindById(id);

			if (person != null)
				return _converter.Parse(person);

			return null;
		}

		public PersonVO Update(PersonVO person)
		{
			var personEntity = _converter.Parse(person);

			personEntity = _repository.Update(personEntity);

			return _converter.Parse(personEntity);
		}

		public void Delete(long id)
		{
			_repository.Delete(id);
		}
	}
}
