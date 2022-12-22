using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class CollectionItemInfo
	{
		public int ItemID { get; set; }
		public int CreatedByID { get; set; }
		public string CreatedByName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int ModifiedByID { get; set; }
		public string ModifiedByName { get; set; }
		public DateTime ModifiedDate { get; set; }
		public int CollectionID { get; set; }
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public int? Quantity { get; set; }
		public decimal? Litres { get; set; }

		public CollectionItemInfo()
		{
		}

		public CollectionItemInfo(DataRow dr)
		{
			ItemID = Utils.FromDBValue<int>(dr["ItemID"]);
			CreatedByID = Utils.FromDBValue<int>(dr["CreatedByID"]);
			CreatedByName = Utils.FromDBValue<string>(dr["CreatedByName"]);
			CreatedDate = Utils.FromDBValue<DateTime>(dr["CreatedDate"]);
			ModifiedByID = Utils.FromDBValue<int>(dr["ModifiedByID"]);
			ModifiedByName = Utils.FromDBValue<string>(dr["ModifiedByName"]);
			ModifiedDate = Utils.FromDBValue<DateTime>(dr["ModifiedDate"]);
			CollectionID = Utils.FromDBValue<int>(dr["CollectionID"]);
			ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
			ProductName = Utils.FromDBValue<string>(dr["ProductName"]);
			Quantity = Utils.FromDBValue<int?>(dr["Quantity"]);
			Litres = Utils.FromDBValue<decimal?>(dr["Litres"]);
		}
	}
}
