namespace RestWithASPNET.Hypermedia.Abstract
{
	public interface ISupportHyperMedia
	{
		ICollection<HyperMediaLink> Links { get; set; }
	}
}
