using System.Web;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class CompanyEditHelper
	{
		public static byte[] CompanyLogoPreview
		{
			get { return (byte[])HttpContext.Current.Session["sCompanyLogoPreview"]; }
			set { HttpContext.Current.Session["sCompanyLogoPreview"] = value; }
		}

		public static CompanyEdit CreateModel(int? id)
		{
			CompanyEdit vm = new CompanyEdit();
			vm.CompanyLogoUploadControlSettings = GetCompanyLogoUploadSettings();

			if (id.HasValue)
			{
				CompanyInfo company = CompanyManager.GetCompany(id.Value, true);
				SetModelFromInfo(company, vm);
				CompanyLogoPreview = vm.Logo;
			}

			return vm;
		}

		public static void UpdateModel(CompanyEdit vm, CompanyEdit cached)
		{
			// Update vm with cache for things that don't change or are not on the form
			vm.CompanyID = cached.CompanyID;
			vm.ApplicationTitle = cached.ApplicationTitle;
			vm.Theme = cached.Theme;

			// Keep existing settings
			vm.CompanyLogoUploadControlSettings = cached.CompanyLogoUploadControlSettings;
			vm.Logo = CompanyLogoPreview;
			
			CompanyInfo info = CreateInfoFromModel(vm);
			vm.Exception = CompanyManager.Save(info);
			vm.Success = (vm.Exception == null);

			CompanyInfo company = CompanyManager.GetCompany(vm.CompanyID, true);
			SetModelFromInfo(company, vm);

			App.Companies.RemoveAll(x => x.CompanyID == vm.CompanyID);
			App.Companies.Add(company);
		}

		public static void SetModelFromInfo(CompanyInfo info, CompanyEdit vm)
		{
			vm.CompanyID = info.CompanyID;
			vm.CompanyName = info.Name;
			vm.Notes = info.Notes;
			vm.Live = info.Live;
			vm.ApplicationTitle = info.ApplicationTitle;
			vm.Theme = info.Theme;
			vm.CopyEmailTo = info.CopyEmailTo;
			vm.SecurityAdminEmail = info.SecurityAdminEmail;
			vm.MaximumSessions = info.MaximumSessions;
			vm.RestrictSessions = info.RestrictSessions;
			vm.Logo = info.Logo;
		}

		public static CompanyInfo CreateInfoFromModel(CompanyEdit vm)
		{
			var info = new CompanyInfo();

			info.CompanyID = vm.CompanyID;
			info.Name = vm.CompanyName;
			info.Notes = vm.Notes;
			info.Live = vm.Live;
			info.ApplicationTitle = vm.ApplicationTitle;
			info.Theme = vm.Theme;
			info.CopyEmailTo = vm.CopyEmailTo;
			info.SecurityAdminEmail = vm.SecurityAdminEmail;
			info.MaximumSessions = vm.MaximumSessions;
			info.RestrictSessions = vm.RestrictSessions;
			info.Logo = vm.Logo;

			return info;
		}

		public static UploadControlSettings GetCompanyLogoUploadSettings()
		{
			UploadControlSettings s = new UploadControlSettings();
			Setup.UploadControl(s, "uplCompanyLogo");
			s.CallbackRouteValues = new { area = "Admin", controller = "Companies", action = "CompanyLogoUpload" };
			s.ClientSideEvents.FileUploadComplete = "function(s,e) { LogoUploadComplete(s,e); }";
			
			UploadControlValidationSettings vs = GetCompanyLogoValidationSettings();
			s.ValidationSettings.Assign(vs);

			return s;
		}

		public static UploadControlValidationSettings GetCompanyLogoValidationSettings()
		{
			UploadControlValidationSettings s = new UploadControlValidationSettings();
			s.AllowedFileExtensions = new[] { ".png" };
			s.MaxFileSize = 20971520;
			return s;
		}

		public static void CompanyLogo_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{
			if (!e.UploadedFile.IsValid) return;

			CompanyLogoPreview = e.UploadedFile.FileBytes;
		}
	}
}
