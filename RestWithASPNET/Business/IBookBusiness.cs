using RestWithASPNET.Model;

namespace RestWithASPNET.Business
{
	public interface IBookBusiness
    {
        Book Create(Book book);
		ICollection<Book> FindAll();
		Book? FindById(long id);
        Book? Update(Book book);
        void Delete(long id);
    }
}
