namespace LeadingEdge.Curator.Web.Home.Models
{
	public class HomeDetails : BaseModel
	{
		public bool NewProductNotifications { get; set; }
		public bool ChangedProductNotifications { get; set; }
		public bool DeactivatedProductNotifications { get; set; }
	}
}