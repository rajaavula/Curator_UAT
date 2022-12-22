using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class MemberProductsController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            MemberProducts vm = MemberProductsHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpGet]
        public ActionResult ListEdit()
        {
            MemberProducts vm = MemberProductsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult ListPreview(string pageID, int id)
        {
            MemberProducts cached = GetModelFromCache<MemberProducts>(pageID);

            object obj = MemberProductsHelper.GetPreviewData(cached, id);

            return SendAsJson(obj);
        }

        [HttpPost]
        public ActionResult ListEdit(MemberProducts vm)
        {
            MemberProducts cached = GetModelFromCache<MemberProducts>(vm.PageID);
            MemberProductsHelper.UpdateModel(vm, cached, IsExporting);

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

        [AjaxOnly]
        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 4)
            {
                vm.StoreID = int.Parse(args[0]);
                vm.FeedKey = int.Parse(args[1]);
                vm.SelectedCategories = args[2];
                if (vm.SelectedCategories.StartsWith(",") || vm.SelectedCategories == string.Empty) vm.SelectedCategories = null;
                vm.SelectedBrands = args[3];
                if (vm.SelectedBrands.StartsWith(",") || vm.SelectedBrands == string.Empty) vm.SelectedBrands = null;
                vm.Grids[name].Data = MemberProductsHelper.GetMemberProductsData(vm);

                SaveModelToCache(vm);
            }

            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<ProductInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = MemberProductsHelper.GetMemberProductsData(vm);

                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult GrdAvailableCallback(string pageID)
        {
            string name = CallbackID;
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 4)
            {
                vm.StoreID = int.Parse(args[0]);
                vm.FeedKey = int.Parse(args[1]);
                vm.SelectedCategories = args[2];
                if (vm.SelectedCategories.StartsWith(",") || vm.SelectedCategories == string.Empty) vm.SelectedCategories = null;
                vm.SelectedBrands = args[3];
                if (vm.SelectedBrands.StartsWith(",") || vm.SelectedBrands == string.Empty) vm.SelectedBrands = null;
                vm.Grids[name].Data = MemberProductsHelper.GetAvailableProductsData(vm);

                SaveModelToCache(vm);
            }

            // If sorting and we have max rows then we need to go back to the DB
            var data = (IEnumerable<ProductInfo>)vm.Grids[name].Data;
            if (args.Count == 0 && DevExpressHelper.CallbackArgument.Contains("SORT") && data.Count() == App.MaxProductsRows)
            {
                vm.Grids[name].Data = MemberProductsHelper.GetAvailableProductsData(vm);

                SaveModelToCache(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult MemberFeedCallback(string pageID, int storeID)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            vm.StoreID = storeID;
            vm.Feeds = MemberProductsHelper.GetMemberFeeds(vm);
            if (vm.Feeds.Count > 1) vm.FeedKey = vm.Feeds.First().FeedKey;  // Should always be at least one aside from the null entry
            vm.MemberFeedModel = MemberProductsHelper.GetMemberFeedComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("ComboBoxPartial", vm.MemberFeedModel);
        }

        [AjaxOnly]
        public ActionResult MemberCategoryCallback(string pageID, int feedKey)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            vm.FeedKey = feedKey;
            vm.MemberCategoriesByFeed = MemberProductsHelper.GetMemberCategoriesByFeed(vm);
            if (vm.MemberCategoriesByFeed.Count > 1)
            {
                vm.SelectedCategories = Convert.ToString(vm.MemberCategoriesByFeed[1].CategoryKey);
            }
            vm.MemberCategoryModel = MemberProductsHelper.GetMemberCategoryComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("CheckComboListBoxPartial", vm.MemberCategoryModel); // This partial is just the listbox part
        }

        [AjaxOnly]
        public ActionResult BrandCallback(string pageID, int feedKey)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            vm.FeedKey = feedKey;

            vm.BrandByFeed = MemberProductsHelper.GetBrandsByFeed();
            if (vm.BrandByFeed.Count > 1)
            {
                vm.SelectedBrands = Convert.ToString(vm.BrandByFeed[1].BrandKey);
            }
            vm.BrandModel = MemberProductsHelper.GetBrandComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("CheckComboListBoxPartial", vm.BrandModel); // This partial is just the listbox part
        }

        [AjaxOnly]
        public ActionResult GetSelectedProducts(string pageID, string ids, int pricingRule, decimal priceValue, bool retailRounding)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            Exception ex = MemberProductsHelper.Save(vm, ids, pricingRule, priceValue, retailRounding);
            SaveModelToCache(vm);   // Save updated data

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        [AjaxOnly]
        public ActionResult SaveProductTags(string pageID, string ids, string productTags, bool defaultProductTags)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            Exception ex = MemberProductsHelper.SaveProductTags(vm, ids, productTags, defaultProductTags);
            SaveModelToCache(vm);   // Save updated data

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        [AjaxOnly]
        public ActionResult GetTagsforProduct(string pageID, int id)
        {
            MemberProducts cached = GetModelFromCache<MemberProducts>(pageID);
            object obj = MemberProductsHelper.GetTagsforProduct(cached, id);
            return SendAsJson(obj);
        }

        [AjaxOnly]
        public ActionResult UpdateSelection(string pageID, string all, string ids)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);

            Exception ex = MemberProductsHelper.UpdateProductSelection(vm, all, ids);
            SaveModelToCache(vm);   // Save updated data

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        [HttpPost]
        public ActionResult BulkPRImportUpload(string pageID)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);
            MemberProductsHelper.BulkPRImport(vm);
            SaveModelToCache(vm);
            return null;
        }

        [HttpGet]
        public ActionResult BulkPRExportDownload(string pageID)
        {
            MemberProducts vm = GetModelFromCache<MemberProducts>(pageID);
            byte[] ExportPRData = MemberProductsHelper.GetBulkPRExportDownload(vm);
            return File(ExportPRData, "application/xlsx", "ExportGrid.xlsx");
        }
    }
}
