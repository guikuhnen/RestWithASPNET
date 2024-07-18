using RestWithASPNET.Data.Converter.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business
{
	public class PersonBusiness(IPersonRepository repository) : IPersonBusiness
	{
		private readonly IPersonRepository _repository = repository;
		private readonly PersonConverter _converter = new();

		public PersonVO Create(PersonVO person)
		{
			var personEntity = _converter.Parse(person);

			personEntity = _repository.Create(personEntity);

			return _converter.Parse(personEntity);
		}

		[Obsolete]
		public ICollection<PersonVO> FindAll()
		{
			return _converter.Parse(_repository.FindAll());
		}

		public PersonVO? FindById(long id)
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

		public PersonVO? SetStatus(long id)
		{
			var personEntity = _repository.SetStatus(id);

			if (personEntity == null)
				return null;

			return _converter.Parse(personEntity);
		}

		public ICollection<PersonVO> FindAllByName(string firstName, string lastName)
		{
			return _converter.Parse(_repository.FindAllByName(firstName, lastName));
		}

		public PagedSearchVO<PersonVO> FindAllWithPagedSearch(string? name, string sortDirection, int pageSize, int currentPage)
		{
			var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc") ? "asc" : "desc";
			var size = pageSize < 1 ? 10 : pageSize;
			var offset = currentPage > 0 ? (currentPage - 1) * size : 0;

			var query = @"select * from person p where 1 = 1 ";
			var countQuery = @"select count(*) from person p where 1 = 1 ";

			if (!string.IsNullOrWhiteSpace(name))
			{
				var filter = $"and p.first_name like '%{name}%' ";
				query += filter;
				countQuery += filter;
			}

			query += $"order by p.first_name {sort} limit {size} offset {offset} ";

			var people = _repository.FindAllWithPagedSearch(query);
			var totalResults = _repository.GetCount(countQuery);

			return new PagedSearchVO<PersonVO>()
			{
				CurrentPage = currentPage,
				List = _converter.Parse(people),
				PageSize = size,
				SortDirections = sort,
				TotalResults = totalResults
			};
		}
	}
}
