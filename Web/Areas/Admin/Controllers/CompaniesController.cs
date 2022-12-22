using System;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class CompaniesController : AuthorizeController
	{
		#region Companies List

		[HttpGet]
		public ActionResult List()
		{
			CompanyList vm = CompanyListHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult List(CompanyList vm)
		{
			CompanyList cached = GetModelFromCache<CompanyList>(vm.PageID);
			CompanyListHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult ListPreview(string pageID, int id)
		{
			CompanyList cached = GetModelFromCache<CompanyList>(pageID);
			object obj = CompanyListHelper.GetPreviewData(cached, id);
			return SendAsJson(obj);
		}

		#endregion

		#region Companies Edit

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			CompanyEdit vm = CompanyEditHelper.CreateModel(id);

			if (TempData["Success"] != null)
			{
				vm.Success = (bool)TempData["Success"];
			}

			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult Edit(CompanyEdit vm)
		{
			CompanyEdit cached = GetModelFromCache<CompanyEdit>(vm.PageID);
			CompanyEditHelper.UpdateModel(vm, cached);
			SaveModelToCache(vm);

			if (vm.Exception == null)
			{
				TempData["Success"] = true;
				return RedirectToAction("Edit", "Companies", new { id = vm.CompanyID });
			}

			return View(vm);
		}

		[HttpPost]
		public ActionResult CompanyLogoUpload()
		{
			UploadControlValidationSettings validationSettings = CompanyEditHelper.GetCompanyLogoValidationSettings();
			UploadControlExtension.GetUploadedFiles("uplCompanyLogo", validationSettings, CompanyEditHelper.CompanyLogo_FileUploadComplete);
			return null;
		}

		[HttpGet]
		public ActionResult CompanyLogoPreview(string id)
		{
			CompanyEdit cached = GetModelFromCache<CompanyEdit>(id);
			byte[] logoBytes = CompanyEditHelper.CompanyLogoPreview;

			if (logoBytes == null || logoBytes.Length < 1)
			{
				logoBytes = cached.Logo;
			}

			return File(logoBytes, "image/jpg");
		}

		#endregion
		
		#region Companies New

		[HttpPost]
		public ActionResult ListNew(string pageID)
		{
			CompanyList vm = GetModelFromCache<CompanyList>(pageID);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult ListNewCreate(string pageID, CompanyInfo info)
		{
			CompanyList vm = GetModelFromCache<CompanyList>(pageID);
			info.OwnerPassword = U.Encrypt(info.OwnerPassword);

			Exception ex = CompanyListHelper.CreateCompany(vm, info);
			SaveModelToCache(vm);

			return Content(ex == null ? String.Empty : ex.Message);
		}
        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            CompanyList vm = GetModelFromCache<CompanyList>(pageID);
            vm.Grids[name].Data = CompanyListHelper.GetData(vm);
            return PartialView("GridViewPartial", vm.Grids[name]);
		}
        #endregion

    }
}
