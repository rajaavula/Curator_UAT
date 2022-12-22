using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting.Native;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class ProductDashboardController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ProductDashboard()
        {
            ProductDashboard vm = ProductDashboardHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult ProductDashboard(ProductDashboard vm)
        {
            ProductDashboard cached = GetModelFromCache<ProductDashboard>(vm.PageID);
            ProductDashboardHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            ProductDashboard vm = GetModelFromCache<ProductDashboard>(pageID);
            SaveModelToCache(vm);
            vm.Grids[name].Data = ProductDashboardHelper.GetProductDashboardItems();

            return PartialView("GridViewPartial", vm.Grids[name]);

        }
    }
}