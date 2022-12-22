using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class FormController : AuthorizeController
	{
		#region Form Editor

		[HttpGet]
		public ActionResult FormEditor()
		{
			FormEditor vm = FormEditorHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult FormEditor(FormEditor vm)
		{
			FormEditor cached = GetModelFromCache<FormEditor>(vm.PageID);
			FormEditorHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult FormEditor_Detail(string pageID, int formID)
		{
			FormEditor vm = GetModelFromCache<FormEditor>(pageID);
			FormInfo info = FormEditorHelper.GetForm(vm, formID);
			return SendAsJson(info);
		}

		[AjaxOnly]
		public ActionResult FormEditor_Save(string pageID, int? formID, FormInfo info)
		{
			FormEditor vm = GetModelFromCache<FormEditor>(pageID);
			string error = FormEditorHelper.Update(vm, formID, info);
			SaveModelToCache(vm);
			return SendAsJson(error);
		}

		[AjaxOnly]
		public ActionResult FormEditor_Delete(string pageID, int formID)
		{
			FormEditor vm = GetModelFromCache<FormEditor>(pageID);
			string error = FormEditorHelper.Delete(vm, formID);
			SaveModelToCache(vm);
			return SendAsJson(error);
		}
        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            FormEditor vm = GetModelFromCache<FormEditor>(pageID);
            vm.Grids[name].Data = FormEditorHelper.GetData(vm);
            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        #endregion
    }
}
