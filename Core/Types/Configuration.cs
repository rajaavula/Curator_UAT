
namespace LeadingEdge.Curator.Core
{
	public class Configuration
	{
		public string DatabaseServer { get; set; }
		public string CuratorDatabase { get; set; }
		public string ProductsDBDatabase { get; set; }
		public string OrdersDBDatabase { get; set; }
		public string SuppliersDBDatabase { get; set; }
		public string ApplicationUrl { get; set; }
		public bool IsLive { get; set; }
		public string WebInstrumentationKey { get; set; }
		public string ErrorEmailRecipient { get; set; }
	}
}
