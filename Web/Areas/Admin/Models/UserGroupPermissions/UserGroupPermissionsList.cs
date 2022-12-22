using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class UserGroupPermissionsList : BaseModel
	{
		public int UserGroupID { get; set; }
		public string UserObjectIDs { get; set; }
		public bool IsSave { get; set; }

		public List<UserGroupInfo> UserGroups { get; set; }
		public List<UserGroupPermissionInfo> AvailableUserGroupPermissions { get; set; }
		public List<UserGroupPermissionInfo> AssignedUserGroupPermissions { get; set; }
 
		public UserGroupPermissionsList()
		{
			UserGroups = new List<UserGroupInfo>();
			AvailableUserGroupPermissions = new List<UserGroupPermissionInfo>();
			AssignedUserGroupPermissions = new List<UserGroupPermissionInfo>();
		}
	}
}