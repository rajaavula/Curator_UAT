using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using Cortex.Utilities;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class CategoryMappingsHelper
    {
        public static CategoryMappingsListEdit CreateModel()
        {
            var vm = new CategoryMappingsListEdit();

            vm.Feeds = GetFeeds();
            vm.Categories = GetCategories();

            vm.FeedID = vm.Feeds.FirstOrDefault()?.Value;

            vm.ConfirmPopupSettings = GetConfirmPopupSettings(vm);

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            return vm;
        }

        public static void UpdateModel(CategoryMappingsListEdit vm, CategoryMappingsListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;
            vm.Feeds = cached.Feeds;
            vm.Categories = cached.Categories;
            vm.ConfirmPopupSettings = cached.ConfirmPopupSettings;

            if (isExporting) return;
        }

        public static GridModel GetGridView(string name, bool exporting, CategoryMappingsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.6", vm.Name);
            grid.Settings.KeyFieldName = "CategoryMappingID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "CategoryMappings", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "FeedName";
                s.Caption = grid.Label(200966); // Feed
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ManufacturerName";
                s.Caption = grid.Label(200618); // Brand
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category1";
                s.Caption = grid.Label(300111); // Feed category 1
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category2";
                s.Caption = grid.Label(300112); // Feed category 2
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category3";
                s.Caption = grid.Label(300113); // Feed category 3
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "CategoryPath";
                s.Caption = grid.Label(200998); // LE Category
                s.Width = 450;
            });

            return grid;
        }

        public static PopupControlSettings GetConfirmPopupSettings(CategoryMappingsListEdit vm)
        {
            var settings = new PopupControlSettings();

            Setup.PopupControl(settings);

            settings.Name = "ppConfirm";
            settings.HeaderText = vm.LabelText(300114);
            settings.Width = Unit.Pixel(400);
            settings.AllowResize = false;
            settings.ControlStyle.CssClass = "cortex-popup";
            settings.SettingsLoadingPanel.Enabled = false;
            settings.LoadContentViaCallback = LoadContentViaCallback.None;
            settings.CallbackRouteValues = new { area = "Products", controller = "CategoryMappings", action = "ListEditConfirm", vm.PageID};
            settings.ClientSideEvents.BeginCallback = "function(s,e) { ppConfirm_BeginCallback(s, e); }";
            settings.ClientSideEvents.EndCallback = "function(s,e) { ppConfirm_EndCallback(); }";

            return settings;
        }

        public static List<CategoryMappingInfo> GetData(CategoryMappingsListEdit vm)
        {
            return CategoryManager.GetCategoryMappings(vm.FeedID);
        }

        public static List<ValueDescription> GetCategories()
        {
            return CategoryManager.GetCategoriesList(true);
        }

        public static List<ValueDescription> GetFeeds()
        {
            return FeedManager.GetUsedFeedsList(false);
        }

        public static CategoryMappingInfo GetDetail(CategoryMappingsListEdit vm, int id)
        {
            return ((List<CategoryMappingInfo>)vm.Grids["GrdMain"].Data).Find(x => x.CategoryMappingID == id);
        }

        public static string Update(CategoryMappingInfo info) 
        {
            var ex = CategoryManager.UpdateCategoryMapping(info);
            if (ex != null)
            {
                return "There was an error updating this category mapping.";
            }

            return null;
        }

        public static void Check(CategoryMappingsListEdit vm, int id)
        {
            var summary = CategoryManager.GetCategoryMappingSummary(id);

            vm.Products = summary?.Products;
        }
    }
}