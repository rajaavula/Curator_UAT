using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Home.Models
{
	public class HomeRegions : BaseModel
	{
		public int RegionID { get; set; }
		public List<CompanyRegionInfo> Regions { get; set; }
	}
}