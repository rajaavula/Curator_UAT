using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class UserLoginHistoryInfo
	{
		public int CompanyID { get; set; }
		public string CompanyName { get; set; }
		public int RegionID { get; set; }
		public string RegionName { get; set; }
		public int UserID { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public int UserGroupID { get; set; }
		public string UserGroupName { get; set; }
		public DateTime? LastLogin { get; set; }

		public UserLoginHistoryInfo(){}

		public UserLoginHistoryInfo(DataRow dr)
		{
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			CompanyName = Utils.FromDBValue<string>(dr["CompanyName"]);
			RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
			RegionName = Utils.FromDBValue<string>(dr["RegionName"]);
			UserID = Utils.FromDBValue<int>(dr["UserID"]);
			UserName = Utils.FromDBValue<string>(dr["UserName"]);
			Email = Utils.FromDBValue<string>(dr["Email"]);
			UserGroupID = Utils.FromDBValue<int>(dr["UserGroupID"]);
			UserGroupName = Utils.FromDBValue<string>(dr["UserGroupName"]);
			LastLogin = Utils.FromDBValue<DateTime?>(dr["LastLogin"]);
		}
	}
}
