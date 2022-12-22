using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class CategoryMappingsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ListEdit()
        {
            var vm = CategoryMappingsHelper.CreateModel();

            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(CategoryMappingsListEdit vm)
        {
            var cached = GetModelFromCache<CategoryMappingsListEdit>(vm.PageID);

            CategoryMappingsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);

            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult ListEditConfirm(string pageID, int id)
        {
            var vm = GetModelFromCache<CategoryMappingsListEdit>(pageID);

            CategoryMappingsHelper.Check(vm, id);

            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            var name = CallbackID;

            var vm = GetModelFromCache<CategoryMappingsListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                var arg = args[0];

                vm.FeedID = string.IsNullOrWhiteSpace(arg) ? null : int.Parse(args[0]);

                SaveModelToCache(vm);

                vm.Grids[name].Data = CategoryMappingsHelper.GetData(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            var cached = GetModelFromCache<CategoryMappingsListEdit>(pageID);

            var info = CategoryMappingsHelper.GetDetail(cached, id);

            return SendAsJson(info);
        }

        [AjaxOnly]
        public ActionResult Update(CategoryMappingInfo info)
        {
            var error = CategoryMappingsHelper.Update(info);

            return Content(error);
        }
    }
}