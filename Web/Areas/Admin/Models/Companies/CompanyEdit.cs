using System;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class CompanyEdit : BaseModel
	{
		public int CompanyID { get; set; }
		public string CompanyName { get; set; }
		public string Notes { get; set; }
		public bool Live { get; set; }
		public string ApplicationTitle { get; set; }
		public string Theme { get; set; }
		public string CopyEmailTo { get; set; }
		public string SecurityAdminEmail { get; set; }
		public int MaximumSessions { get; set; }
		public bool RestrictSessions { get; set; }
		public byte[] Logo { get; set; }

		public UploadControlSettings CompanyLogoUploadControlSettings { get; set; }

		public string ReloadUrl
		{
			get { return (CompanyID > 0) ? String.Format("/Admin/Companies/Edit/{0}", CompanyID) : "/Admin/Companies/New"; }
		}

		public CompanyEdit()
		{
			CompanyLogoUploadControlSettings = new UploadControlSettings();
		}
	}
}