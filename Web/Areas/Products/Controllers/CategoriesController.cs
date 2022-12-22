using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class CategoriesController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ListEdit()
        {
            CategoriesListEdit vm = CategoriesHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(CategoriesListEdit vm)
        {
            CategoriesListEdit cached = GetModelFromCache<CategoriesListEdit>(vm.PageID);
            CategoriesHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);

            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            CategoriesListEdit vm = GetModelFromCache<CategoriesListEdit>(pageID);
            CategoryInfo info = CategoriesHelper.GetCategoryInfo(vm, id);

            return SendAsJson(info);
        }

        [HttpGet]
        public ActionResult New()
        {
            CategoriesListEdit vm = CategoriesHelper.CreateModel();
            SaveModelToCache(vm);

            return View("ListEdit");
        }

        [AjaxOnly]
        public ActionResult Save(string pageID, int? categoryKey, string name, string description, int? parentKey)
        {
            CategoriesListEdit vm = GetModelFromCache<CategoriesListEdit>(pageID);

            Exception ex = CategoriesHelper.Save(vm, categoryKey, name, description, parentKey);
            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        [AjaxOnly]
        public ActionResult Delete(string pageID, int id)
        {
            CategoriesListEdit vm = GetModelFromCache<CategoriesListEdit>(pageID);
            Exception ex = CategoriesHelper.Delete(vm, id);

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            CategoriesListEdit vm = GetModelFromCache<CategoriesListEdit>(pageID);
            vm.Grids[name].Data = CategoriesHelper.GetData();

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        public ActionResult ParentCategoryCallback(string pageID)
        {
            CategoriesListEdit vm = GetModelFromCache<CategoriesListEdit>(pageID);
                        
            vm.ParentCategoryModel = CategoriesHelper.GetParentCategoryComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("ComboBoxPartial", vm.ParentCategoryModel);
        }
    }
}

