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
    public class ERPIntegrationsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ListEdit()
        {
            var vm = ERPIntegrationsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(ERPIntegrationsListEdit vm)
        {
            var cached = GetModelFromCache<ERPIntegrationsListEdit>(vm.PageID);
            ERPIntegrationsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);

            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, Guid id)
        {
            var cached = GetModelFromCache<ERPIntegrationsListEdit>(pageID);

            var info = ERPIntegrationsHelper.GetVendor(cached, id);

            return SendAsJson(info);
        }

        [AjaxOnly]
        public ActionResult Save(string pageID, VendorInfo info)
        {
            var cached = GetModelFromCache<ERPIntegrationsListEdit>(pageID);

            string error = ERPIntegrationsHelper.Update(cached, info);

            SaveModelToCache(cached);

            return Content(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            var name = CallbackID;
            var vm = GetModelFromCache<ERPIntegrationsListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                bool refresh = bool.Parse(args[0]);
                if (refresh)    // We may have manually removed some rows from the grid data so no need to go to DB
                {
                    vm.Grids[name].Data = ERPIntegrationsHelper.GetData(vm);
                    SaveModelToCache(vm);
                }
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        public ActionResult GrdProductsCallback(string pageID)
        {
            var name = CallbackID;
            var vm = GetModelFromCache<ERPIntegrationsListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 2)
            {
                bool refresh = bool.Parse(args[0]);
                if (refresh)    // We may have manually removed some rows from the grid data so no need to go to DB
                {
                    vm.VendorID = Guid.Parse(args[1]);
                    vm.Grids[name].Data = ERPIntegrationsHelper.GetProducts(vm);
                    SaveModelToCache(vm);
                }
            }

            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<ERPIntegrationProductInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = ERPIntegrationsHelper.GetProducts(vm);
                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        public ActionResult Send(string pageID, string productIDs)
        {
            var vm = GetModelFromCache<ERPIntegrationsListEdit>(pageID);

            var ex = ERPIntegrationsHelper.Send(vm, productIDs);
            string error = string.Empty;
            if (ex != null && ex.Message != null) error = "There was an unexpected error flagging the selected products to be sent to NetSuite and Magento.";

            return SendAsJson(error);
        }
    }
}
