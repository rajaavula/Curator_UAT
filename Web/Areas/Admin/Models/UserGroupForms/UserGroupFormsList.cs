using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class UserGroupFormsList : BaseModel
	{
		public int UserGroupID { get; set; }
		public string FormIDs { get; set; }
		public bool IsSave { get; set; }

		public List<UserGroupInfo> UserGroups { get; set; }
		public List<FormInfo> AvailableForms { get; set; }
		public List<FormInfo> AssignedForms { get; set; }

		public UserGroupFormsList()
		{
			UserGroups = new List<UserGroupInfo>();

			AvailableForms = new List<FormInfo>();

			AssignedForms = new List<FormInfo>();
		}
	}
}