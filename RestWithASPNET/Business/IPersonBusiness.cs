using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
	public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
		[Obsolete]
        ICollection<PersonVO> FindAll();
		PersonVO? FindById(long id);
		PersonVO Update(PersonVO person);
        void Delete(long id);
		PersonVO? SetStatus(long id);
		ICollection<PersonVO> FindAllByName(string firstName, string lastName);
		PagedSearchVO<PersonVO> FindAllWithPagedSearch(string? name, string sortDirection, int pageSize, int currentPage);
	}
}
