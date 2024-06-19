using RestWithASPNET.Data.Converter.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Repository.Base;

namespace RestWithASPNET.Business
{
	public class BookBusiness(IBaseRepository<Book> repository) : IBookBusiness
    {
        private readonly IBaseRepository<Book> _repository = repository;
		private readonly BookConverter _converter = new();

		public BookVO Create(BookVO book)
		{
			var bookEntity = _converter.Parse(book);

			bookEntity = _repository.Create(bookEntity);

			return _converter.Parse(bookEntity);
		}

		public ICollection<BookVO> FindAll()
		{
			return _converter.Parse(_repository.FindAll());
		}

        public BookVO FindById(long id)
		{
			var book = _repository.FindById(id);

			if (book != null)
				return _converter.Parse(book);

			return null;
		}

        public BookVO Update(BookVO book)
		{
			var bookEntity = _converter.Parse(book);

			bookEntity = _repository.Update(bookEntity);

			return _converter.Parse(bookEntity);
		}

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
	}
}
