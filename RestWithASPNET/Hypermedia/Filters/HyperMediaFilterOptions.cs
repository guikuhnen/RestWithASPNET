using RestWithASPNET.Hypermedia.Abstract;

namespace RestWithASPNET.Hypermedia.Filters
{
	public class HyperMediaFilterOptions
	{
		public ICollection<IResponseEnricher> ContentResponseEnricherList { get; set; } = [];
	}
}
