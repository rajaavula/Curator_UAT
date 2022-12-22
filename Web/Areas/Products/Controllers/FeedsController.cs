using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class FeedsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ListEdit()
        {
            FeedsListEdit vm = FeedsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(FeedsListEdit vm)
        {
            FeedsListEdit cached = GetModelFromCache<FeedsListEdit>(vm.PageID);
            FeedsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            FeedsListEdit vm = GetModelFromCache<FeedsListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                bool refresh = bool.Parse(args[0]);
                if (refresh)    // We may have manually removed some rows from the grid data so no need to go to DB
                {
                    vm.Grids[name].Data = FeedsHelper.GetData();
                    SaveModelToCache(vm);
                }
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Update(FeedInfo info)
        {
            object obj = FeedsHelper.Save(info);
            return SendAsJson(obj);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            FeedsListEdit cached = GetModelFromCache<FeedsListEdit>(pageID);
            object obj = FeedsHelper.GetDetail(cached, id);
            return SendAsJson(obj);
        }
    }
}