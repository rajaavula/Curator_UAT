using DevExpress.Web;
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
    public class ProductBrandRestrictionsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            ProductBrandRestrictionsList vm = ProductBrandRestrictionsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult List(ProductBrandRestrictionsList vm)
        {
            ProductBrandRestrictionsList cached = GetModelFromCache<ProductBrandRestrictionsList>(vm.PageID);
            ProductBrandRestrictionsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting)
            {
                // Remove column on cached grid and replace afterwards. Changing column to visible=false did not seem to work with the exporter
                string caption = null;
                GridViewColumn col = vm.Grids[ExportedGridName].Settings.Columns["ExportColumn"];
                if (col != null)
                {
                    caption = col.Caption;
                    col.Caption = string.Empty;
                }
                var file = ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
                col.Caption = caption;

                return file;
            }

            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            ProductBrandRestrictionsList vm = GetModelFromCache<ProductBrandRestrictionsList>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 2)
            {
                bool refresh = bool.Parse(args[0]);
                if (refresh)    // We may have manually removed some rows from the grid data so no need to go to DB
                {
                    vm.BrandKey = int.Parse(args[1]);
                    vm.Grids[name].Data = ProductBrandRestrictionsHelper.GetData(vm);
                    SaveModelToCache(vm);
                }
            }

            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<ProductBrandRestrictionInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = ProductBrandRestrictionsHelper.GetData(vm);
                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Assign(string pageID, int catalogKey, string ids)
        {
            ProductBrandRestrictionsList vm = GetModelFromCache<ProductBrandRestrictionsList>(pageID);

            Exception ex = ProductBrandRestrictionsHelper.Assign(vm, catalogKey, ids);
            SaveModelToCache(vm);   // Save updated data

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }
    }
}