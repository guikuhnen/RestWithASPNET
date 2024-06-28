using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermedia.Abstract;
using System.Text.Json.Serialization;

namespace RestWithASPNET.Data.VO
{
	public class BookVO : ISupportsHyperMedia
	{
		public long Id { get; set; }

		public string? Title { get; set; }

		public string? Author { get; set; }

		public decimal Price { get; set; }

		[JsonPropertyName("launch_date")]
		public DateTime LaunchDate { get; set; }

		public ICollection<HyperMediaLink> Links { get; set; } = [];
	}
}
