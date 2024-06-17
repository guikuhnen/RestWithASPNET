using RestWithASPNET.Model;
using RestWithASPNET.Repository;

namespace RestWithASPNET.Business
{
	public class BookBusiness(IBookRepository repository) : IBookBusiness
    {
        private readonly IBookRepository _repository = repository;

		public Book Create(Book book)
		{
			return _repository.Create(book);
		}

		public ICollection<Book> FindAll()
        {
            return _repository.FindAll();
        }

        public Book FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Book Update(Book book)
        {
            return _repository.Update(book);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
	}
}
