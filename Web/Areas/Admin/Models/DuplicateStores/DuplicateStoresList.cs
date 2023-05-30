using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class DuplicateStoresList : BaseModel
	{
		public int SourceStoreID { get; set; }
		public List<StoreInfo> Stores { get; set; }

		public DuplicateStoresList()
		{
			Stores = new List<StoreInfo>();
		}
	}
}