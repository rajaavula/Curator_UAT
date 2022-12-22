using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class ProductsController : AuthorizeController
    {
        # region ListEdit
        [HttpGet]
        public ActionResult ListEdit()
        {
            ProductsListEdit vm = ProductsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(ProductsListEdit vm)
        {
            ProductsListEdit cached = GetModelFromCache<ProductsListEdit>(vm.PageID);
            ProductsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            ProductsListEdit vm = GetModelFromCache<ProductsListEdit>(pageID);
            ProductInfo info = ProductsHelper.GetProductInfo(vm, id);

            return SendAsJson(info);
        }

        public ActionResult Save(string pageID, ProductInfo info)
        {
            ProductsListEdit vm = GetModelFromCache<ProductsListEdit>(pageID);

            Exception ex = ProductsHelper.Save(vm, info);
            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            ProductsListEdit vm = GetModelFromCache<ProductsListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 2)
            {
                vm.FeedKey = int.Parse(args[0]);
                vm.BrandKey = (args[1] == "null" ? (int?)null : int.Parse(args[1]));
                //vm.BrandKey = (args[1] == "null" ? null : int.Parse(args[1]));
                vm.Grids[name].Data = ProductsHelper.GetData(vm);
                SaveModelToCache(vm);
            }

            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<ProductInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = ProductsHelper.GetData(vm);
                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        public ActionResult BrandCallback(string pageID, int feedKey)
        {
            ProductsListEdit vm = GetModelFromCache<ProductsListEdit>(pageID);
            vm.FeedKey = feedKey;
            vm.Brands = ProductsHelper.GetBrands(vm);

            if (vm.Brands.Count > 1) vm.BrandKey = vm.Brands.First(x => x.BrandKey != null).BrandKey;  // Should always be at least one aside from the null entry

            vm.BrandModel = ProductsHelper.GetBrandComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("ComboBoxPartial", vm.BrandModel);
        }

        [AjaxOnly]
        public ActionResult ListPreview(string pageID, int id)
        {
            ProductsListEdit cached = GetModelFromCache<ProductsListEdit>(pageID);
            object obj = ProductsHelper.GetPreviewData(cached, id);
            return SendAsJson(obj);
        }

        #endregion

    }
}
