using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class VisitorLogInfo
	{
		public int VisitorLogID { get; set; }
		public DateTime DateTime { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }
		public string UserGroup { get; set; }
		public string Region { get; set; }
		public DateTime? SessionEnd { get; set; }

		public VisitorLogInfo() { }

		public VisitorLogInfo(DataRow dr)
		{
			VisitorLogID = Utils.FromDBValue<int>(dr["VisitorLogID"]);
			DateTime = U.GetLocalTime(Utils.FromDBValue<DateTime>(dr["DateTime"]));
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Position = Utils.FromDBValue<string>(dr["Position"]);
			UserGroup = Utils.FromDBValue<string>(dr["UserGroup"]);
			Region = Utils.FromDBValue<string>(dr["Region"]);
			SessionEnd = Utils.FromDBValue<DateTime?>(dr["SessionEnd"]);
		}
	}
}
