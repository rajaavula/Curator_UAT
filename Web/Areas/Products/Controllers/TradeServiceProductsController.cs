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
    public class TradeServiceProductsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            TradeServiceProducts vm = TradeServiceProductsHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpGet]
        public ActionResult ListEdit()
        {
            TradeServiceProducts vm = TradeServiceProductsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(TradeServiceProducts vm)
        {
            TradeServiceProducts cached = GetModelFromCache<TradeServiceProducts>(vm.PageID);
            TradeServiceProductsHelper.UpdateModel(vm, cached, IsExporting);
            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);
            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            TradeServiceProducts vm = GetModelFromCache<TradeServiceProducts>(pageID);
            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                vm.CategoryKey = (args[0] == "" ? (int?)null : int.Parse(args[0]));
               
            }
            vm.Grids[name].Data = TradeServiceProductsHelper.GetData(vm);

            SaveModelToCache(vm);


            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<TradeServiceInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = TradeServiceProductsHelper.GetData(vm);

                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);


        }
    }
}
