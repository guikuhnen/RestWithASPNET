using RestWithASPNET.Data.Converter.Business;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Hypermedia.Utils;
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

		public PagedSearchVO<BookVO> FindAllWithPagedSearch(string? title, string sortDirection, int pageSize, int currentPage)
		{
			var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc") ? "asc" : "desc";
			var size = pageSize < 1 ? 10 : pageSize;
			var offset = currentPage > 0 ? (currentPage - 1) * size : 0;

			var query = @"select * from books p where 1 = 1 ";
			if (!string.IsNullOrWhiteSpace(title)) 
				query += $" and p.title like '%{title}%' ";

			query += $" order by p.title {sort} limit {size} offset {offset}";

			var countQuery = @"select count(*) from books p where 1 = 1 ";
			if (!string.IsNullOrWhiteSpace(title)) 
				countQuery += $" and p.title like '%{title}%' ";

			var books = _repository.FindAllWithPagedSearch(query);
			var totalResults = _repository.GetCount(countQuery);

			return new PagedSearchVO<BookVO>()
			{
				CurrentPage = currentPage,
				List = _converter.Parse(books),
				PageSize = size,
				SortDirections = sort,
				TotalResults = totalResults
			};
		}
	}
}
