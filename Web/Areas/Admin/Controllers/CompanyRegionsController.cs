using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class CompanyRegionsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult ListEdit()
		{
			CompanyRegionsListEdit vm = CompanyRegionsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult ListEdit(CompanyRegionsListEdit vm)
		{
			CompanyRegionsListEdit cached = GetModelFromCache<CompanyRegionsListEdit>(vm.PageID);
			CompanyRegionsHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult Detail(string pageID, int id)
		{
			CompanyRegionsListEdit vm = GetModelFromCache<CompanyRegionsListEdit>(pageID);
			CompanyRegionInfo info = CompanyRegionsHelper.GetCompanyRegionInfo(vm, id);
			return SendAsJson(info);
		}

		[AjaxOnly]
		public ActionResult Save(string pageID, int? regionID, CompanyRegionInfo info)
		{
			CompanyRegionsListEdit vm = GetModelFromCache<CompanyRegionsListEdit>(pageID);
			string error = CompanyRegionsHelper.Update(vm, regionID, info);
			return SendAsJson(error);
		}

		[AjaxOnly]
		public ActionResult Delete(string pageID, int id)
		{
			CompanyRegionsListEdit vm = GetModelFromCache<CompanyRegionsListEdit>(pageID);
			string error = CompanyRegionsHelper.Delete(vm, id);
			return SendAsJson(error);
		}

		public ActionResult GrdMainCallback(string pageID)
		{
			string name = CallbackID;
			CompanyRegionsListEdit vm = GetModelFromCache<CompanyRegionsListEdit>(pageID);
			vm.Grids[name].Data = CompanyRegionsHelper.GetData(vm);
			return PartialView("GridViewPartial", vm.Grids[name]);
		}
	}
}
