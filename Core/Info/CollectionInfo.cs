using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class CollectionInfo
	{
		public int CollectionID { get; set; }
		public int CreatedByID { get; set; }
		public string CreatedByName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int ModifiedByID { get; set; }
		public string ModifiedByName { get; set; }
		public DateTime ModifiedDate { get; set; }
		public int SiteID { get; set; }
		public string SiteName { get; set; }
		public DateTime CollectDate { get; set; }

		public CollectionInfo()
		{
		}

		public CollectionInfo(DataRow dr)
		{
			CollectionID = Utils.FromDBValue<int>(dr["CollectionID"]);
			CreatedByID = Utils.FromDBValue<int>(dr["CreatedByID"]);
			CreatedByName = Utils.FromDBValue<string>(dr["CreatedByName"]);
			CreatedDate = Utils.FromDBValue<DateTime>(dr["CreatedDate"]);
			ModifiedByID = Utils.FromDBValue<int>(dr["ModifiedByID"]);
			ModifiedByName = Utils.FromDBValue<string>(dr["ModifiedByName"]);
			ModifiedDate = Utils.FromDBValue<DateTime>(dr["ModifiedDate"]);
			SiteID = Utils.FromDBValue<int>(dr["SiteID"]);
			SiteName = Utils.FromDBValue<string>(dr["SiteName"]);
			CollectDate = Utils.FromDBValue<DateTime>(dr["CollectDate"]);
		}
	}
}
