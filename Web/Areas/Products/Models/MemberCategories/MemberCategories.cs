using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Products.Models
{
	public class MemberCategories : BaseModel
	{
		public List<CategoryInfo> Categories { get; set; }
		public int StoreID { get; set; }
		public List<StoreInfo> MemberStoreList { get; set; }
		public string ExportButtonString { get; set; }

		public MemberCategories()
		{

		}
	}
}