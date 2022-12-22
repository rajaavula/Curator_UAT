using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class SessionsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult List()
		{
			SessionsList vm = SessionsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult List(SessionsList vm)
		{
			SessionsList cached = GetModelFromCache<SessionsList>(vm.PageID);
			SessionsHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}
		public ActionResult GrdMainCallback(string pageID)
		{
			string name = CallbackID;
			SessionsList vm = GetModelFromCache<SessionsList>(pageID);
			vm.Grids[name].Data = SessionsHelper.GetData(vm);
			return PartialView("GridViewPartial", vm.Grids[name]);
		}
	}
}
