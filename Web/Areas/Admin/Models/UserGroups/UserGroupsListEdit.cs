using System.Collections.Generic;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class UserGroupsListEdit : BaseModel
	{
		public List<ValueDescription> Warehouses { get; set; }
	}
}