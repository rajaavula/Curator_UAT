using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class UserGroupPermissionInfo
	{
		public int UserGroupPermissionID { get; set; }
		public int UserGroupID { get; set; }
		public int UserObjectID { get; set; }
		public int CompanyID { get; set; }
		public string Type { get; set; }
		public string Code { get; set; }
		public string Group { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public UserGroupPermissionInfo(){}

		public UserGroupPermissionInfo(DataRow dr)
		{
			UserGroupPermissionID = Utils.FromDBValue<int>(dr["UserGroupPermissionID"]);
			UserGroupID = Utils.FromDBValue<int>(dr["UserGroupID"]);
			UserObjectID = Utils.FromDBValue<int>(dr["UserObjectID"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			Type = Utils.FromDBValue<string>(dr["Type"]);
			Code = Utils.FromDBValue<string>(dr["Code"]);
			Group = Utils.FromDBValue<string>(dr["Group"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Description = Utils.FromDBValue<string>(dr["Description"]);
		}
	}
}
