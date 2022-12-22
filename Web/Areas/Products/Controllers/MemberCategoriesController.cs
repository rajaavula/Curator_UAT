using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class MemberCategoriesController : AuthorizeController
    {
        #region Member Categories

        [HttpGet]
        public ActionResult ListEdit()
        {
            MemberCategories vm = MemberCategoriesHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(MemberCategories vm)
        {
            MemberCategories cached = GetModelFromCache<MemberCategories>(vm.PageID);
            MemberCategoriesHelper.UpdateModel(vm, cached, IsExporting);
            if (IsExporting) {
                var result = ExportGridView(ExportType.Xlsx, cached.Grids[ExportedGridName]);
                MemberCategoriesHelper.SetExportCommandButton(cached);
                return result;
            }
            SaveModelToCache(vm);
            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            MemberCategories vm = GetModelFromCache<MemberCategories>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1 && string.IsNullOrEmpty(args[0]) == false)
            {
                vm.StoreID = int.Parse(args[0]);
                vm.Grids["GrdMain"] = MemberCategoriesHelper.GetGridView("GrdMain", false, vm);
                vm.Categories = MemberCategoriesHelper.GetData(vm.StoreID);
                vm.Grids["GrdMain"].Data = vm.Categories;

                SaveModelToCache(vm);
            }
                        
            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Assign(string pageID, string all, string ids)
        {
            MemberCategories vm = GetModelFromCache<MemberCategories>(pageID);

            Exception ex = MemberCategoriesHelper.UpdateMemberCategories(vm, all, ids);
            SaveModelToCache(vm);   // Save updated data

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        #endregion
    }
}
