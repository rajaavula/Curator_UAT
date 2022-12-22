using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class FormInfo
	{
		public int FormID { get; set; }
		public string Group { get; set; }
		public string Area { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Parameters { get; set; }
		public string Name { get; set; }
		public int PlaceholderID { get; set; }
		public int DisplayOrder { get; set; }
		public string Notes { get; set; }
		public bool OwnersOnly { get; set; }

		public FormInfo()
		{
		}

		public FormInfo(DataRow dr)
		{
			FormID = Utils.FromDBValue<int>(dr["FormID"]);
			Group = Utils.FromDBValue<string>(dr["Group"]);
			Area = Utils.FromDBValue<string>(dr["Area"]);
			Controller = Utils.FromDBValue<string>(dr["Controller"]);
			Action = Utils.FromDBValue<string>(dr["Action"]);
			Parameters = Utils.FromDBValue<string>(dr["Parameters"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			PlaceholderID = Utils.FromDBValue<int>(dr["PlaceholderID"]);
			DisplayOrder = Utils.FromDBValue<int>(dr["DisplayOrder"]);
			Notes = Utils.FromDBValue<string>(dr["Notes"]);
			OwnersOnly = Utils.FromDBValue<bool>(dr["OwnersOnly"]);
		}
	}
}
