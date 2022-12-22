using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class UserGroupInfo
	{
		public int UserGroupID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsOwner { get; set; }
		public bool IsWorker { get; set; }
		public int CompanyID { get; set; }

		public UserGroupInfo(){}

		public UserGroupInfo(DataRow dr)
		{
			UserGroupID = Utils.FromDBValue<int>(dr["UserGroupID"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Description = Utils.FromDBValue<string>(dr["Description"]);
			IsOwner = Utils.FromDBValue<bool>(dr["IsOwner"]);
			IsWorker = Utils.FromDBValue<bool>(dr["IsWorker"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
		}
	}
}
