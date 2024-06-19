using RestWithASPNET.Data.Converter.Contract;
using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Data.Converter.Business
{
	public class BookConverter : IParser<BookVO, Book>, IParser<Book, BookVO>
	{
		#region Business > VO

		public Book Parse(BookVO origin)
		{
			if (origin == null)
				return null;

			return new Book()
			{
				Id = origin.Id,
				Author = origin.Author,
				LaunchDate = origin.LaunchDate,
				Price = origin.Price,
				Title = origin.Title
			};
		}

		public ICollection<Book> Parse(ICollection<BookVO> origin)
		{
			if (origin == null)
				return null;

			return origin.Select(Parse).ToList();
		}

		#endregion

		#region VO > Business

		public BookVO Parse(Book origin)
		{
			if (origin == null)
				return null;

			return new BookVO()
			{
				Id = origin.Id,
				Author = origin.Author,
				LaunchDate = origin.LaunchDate,
				Price = origin.Price,
				Title = origin.Title
			};
		}

		public ICollection<BookVO> Parse(ICollection<Book> origin)
		{
			if (origin == null)
				return null;

			return origin.Select(Parse).ToList();
		}

		#endregion
	}
}
