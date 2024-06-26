using System.Text;

namespace RestWithASPNET.Hypermedia
{
	public class HyperMediaLink
	{
		public string Rel { get; set; }

		private string _href;
		public string Href
		{
			get
			{
				object _lock = new();
				lock (_lock)
				{
					StringBuilder sb = new(_href);
					return sb.Replace("%2f", "/").ToString();
				}
			}
			set { _href = value; }
		}

		public string Type { get; set; }
		public string Action { get; set; }
	}
}
