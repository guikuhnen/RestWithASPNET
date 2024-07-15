using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermedia.Abstract;
using System.Text.Json.Serialization;

namespace RestWithASPNET.Data.VO
{
	public class PersonVO : ISupportsHyperMedia
	{
		public long Id { get; set; }

		[JsonPropertyName("first_name")]
		public string? FirstName { get; set; }

		[JsonPropertyName("last_name")]
		public string? LastName { get; set; }

		public string? Address { get; set; }

		[JsonIgnore]
		public string? Gender { get; set; }

		public bool Enabled { get; set; }

		public ICollection<HyperMediaLink> Links { get; set; } = [];
	}
}
