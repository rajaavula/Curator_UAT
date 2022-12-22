using System;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
    public class VisitorLogController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            VisitorLogList vm = VisitorLogHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult List(VisitorLogList vm)
        {
            VisitorLogList cached = GetModelFromCache<VisitorLogList>(vm.PageID);
            VisitorLogHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);

            SaveModelToCache(vm);

            return View(vm);
        }
        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            VisitorLogList vm = GetModelFromCache<VisitorLogList>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 2)
            {
                vm.FromDate = DateTime.Parse(args[0]);
                vm.ToDate = DateTime.Parse(args[1]);
                SaveModelToCache(vm);
            }

            vm.Grids[name].Data = VisitorLogHelper.GetData(vm);

            return PartialView("GridViewPartial", vm.Grids[name]);
        }
    }
}
