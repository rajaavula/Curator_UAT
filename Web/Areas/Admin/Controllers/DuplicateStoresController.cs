using System;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
    public class DuplicateStoresController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            DuplicateStoresList vm = DuplicateStoresHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult List(DuplicateStoresList vm)
        {
            DuplicateStoresList cached = GetModelFromCache<DuplicateStoresList>(vm.PageID);
            DuplicateStoresHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);

            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            DuplicateStoresList vm = GetModelFromCache<DuplicateStoresList>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                vm.SourceStoreID = Convert.ToInt32(args[0]);
                SaveModelToCache(vm);
            }

            vm.Grids[name].Data = DuplicateStoresHelper.GetData(vm);

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult DuplicateSelectedStores(string pageID, string ids)
        {
            DuplicateStoresList vm = GetModelFromCache<DuplicateStoresList>(pageID);

            string error = DuplicateStoresHelper.DuplicateStores(vm, ids);

            return SendAsJson(error);
        }
    }
}