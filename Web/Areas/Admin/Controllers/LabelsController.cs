using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class LabelsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult ListEdit()
		{
			LabelsListEdit vm = LabelsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult ListEdit(LabelsListEdit vm)
		{
			LabelsListEdit cached = GetModelFromCache<LabelsListEdit>(vm.PageID);
			LabelsHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult Detail(string pageID, int id)
		{
			LabelsListEdit vm = GetModelFromCache<LabelsListEdit>(pageID);
			LabelInfo info = LabelsHelper.GetLabelInfo(vm, id);

			return SendAsJson(info);
		}

		[HttpGet]
		public ActionResult New()
		{
			LabelsListEdit vm = LabelsHelper.CreateModel();
			SaveModelToCache(vm);
			return View("ListEdit");
		}

		[AjaxOnly]
		public ActionResult Save(string pageID, int? labelID, string labelType, int placeholderID, string languageID, string labelText, string toolTipText)
		{
			LabelsListEdit vm = GetModelFromCache<LabelsListEdit>(pageID);
			string error = LabelsHelper.Update(vm, labelID, labelType, placeholderID, languageID, labelText, toolTipText);
			return SendAsJson(error);
		}

		[AjaxOnly]
		public ActionResult Delete(string pageID, int id)
		{
			LabelsListEdit vm = GetModelFromCache<LabelsListEdit>(pageID);
			string error = LabelsHelper.Delete(vm, id);
			return SendAsJson(error);
		}

		public ActionResult GrdMainCallback(string pageID)
		{
			string name = CallbackID;
			LabelsListEdit vm = GetModelFromCache<LabelsListEdit>(pageID);
			vm.Grids[name].Data = LabelsHelper.GetData(vm);

			return PartialView("GridViewPartial", vm.Grids[name]);
		}
	}
}
