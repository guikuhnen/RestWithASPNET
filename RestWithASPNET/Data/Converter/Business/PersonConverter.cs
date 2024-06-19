using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Data.Converter.Business
{
	public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
	{
		#region Business > VO

		public Person Parse(PersonVO origin)
		{
			if (origin == null)
				return null;

			return new Person()
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender
			};
		}

		public ICollection<Person> Parse(ICollection<PersonVO> origin)
		{
			if (origin == null)
				return null;

			return origin.Select(Parse).ToList();
		}

		#endregion

		#region VO > Business

		public PersonVO Parse(Person origin)
		{
			if (origin == null)
				return null;

			return new PersonVO()
			{
				Id = origin.Id,
				FirstName = origin.FirstName,
				LastName = origin.LastName,
				Address = origin.Address,
				Gender = origin.Gender
			};
		}

		public ICollection<PersonVO> Parse(ICollection<Person> origin)
		{
			if (origin == null)
				return null;

			return origin.Select(Parse).ToList();
		}

		#endregion
	}
}
