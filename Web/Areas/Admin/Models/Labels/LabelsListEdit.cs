using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class LabelsListEdit : BaseModel
	{
		public List<CompanyInfo> Companies { get; set; }
		public int SelectedCompanyID { get; set; }

		public LabelsListEdit()
		{
			Companies = new List<CompanyInfo>();
		}
	}
}