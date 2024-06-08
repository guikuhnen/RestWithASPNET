using RestWithASPNET.Model;

namespace RestWithASPNET.Services
{
	public class PersonService : IPersonService
	{
		public Person Create(Person person)
		{
			return person;
		}

		public Person FindById(long id)
		{
			return new Person()
			{
				Id = 1,
				FirstName = "Guilherme",
				LastName = "Kuhnen",
				Address = "Blumenau/SC",
				Gender = "Male"
			};
		}

		public ICollection<Person> FindAll()
		{
			return
			[
				new Person()
				{
					Id = 1,
					FirstName = "Guilherme",
					LastName = "Kuhnen",
					Address = "Blumenau/SC",
					Gender = "Male"
				},
				new Person()
				{
					Id = 2,
					FirstName = "Guilherme",
					LastName = "Kuhnen 2",
					Address = "Indaial/SC",
					Gender = "Male"
				}
			];
		}

		public Person Update(Person person)
		{
			return person;
		}

		public void Delete(long id)
		{
			// Deletou
		}
	}
}
