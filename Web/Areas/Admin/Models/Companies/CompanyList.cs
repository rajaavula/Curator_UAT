using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class CompanyList : BaseModel
	{
		public PopupControlSettings NewCompanyPopupSettings { get; set; }

		public CompanyList()
		{
			NewCompanyPopupSettings = new PopupControlSettings();
		}
	}
}