namespace LeadingEdge.Curator.Web.Home.Models
{
	public class HomeIndex : BaseModel
	{
		public int RegionID { get; set; }
		public string LanguageID { get; set; }

        public int Year { get; set; }
	}
}