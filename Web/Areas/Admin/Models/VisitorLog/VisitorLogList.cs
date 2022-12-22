using System;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class VisitorLogList : BaseModel
	{
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
	}
}