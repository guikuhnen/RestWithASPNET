using RestWithASPNET.Model;

namespace RestWithASPNET.Repository
{
	public interface IBookRepository
	{
        Book Create(Book person);
        Book FindById(long id);
        ICollection<Book> FindAll();
        Book Update(Book person);
        void Delete(long id);
        bool Exists(long id);
    }
}
