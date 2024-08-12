using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;

namespace RestWithASPNET.Business
{
	public interface IBookBusiness
    {
        BookVO Create(BookVO book);
		ICollection<BookVO> FindAll();
		BookVO FindById(long id);
        BookVO Update(BookVO book);
        void Delete(long id);
		PagedSearchVO<BookVO> FindAllWithPagedSearch(string? title, string sortDirection, int pageSize, int currentPage);
	}
}
