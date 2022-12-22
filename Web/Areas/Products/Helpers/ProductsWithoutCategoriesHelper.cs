using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class ProductsWithoutCategoriesHelper
    {
        public static ProductsWithoutCategoriesList CreateModel()
        {
            var vm = new ProductsWithoutCategoriesList();

            vm.SortBy = "SupplierPartNumber";  // On first grid load            
            vm.SortDirection = "ASC";
            vm.Categories = GetCategories();
            vm.Feeds = GetFeeds();

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            return vm;
        }

        public static void UpdateModel(ProductsWithoutCategoriesList vm, ProductsWithoutCategoriesList cached, bool isExporting)
        {
            vm.Categories = cached.Categories;
            vm.Feeds = cached.Feeds;
            vm.Grids = cached.Grids;

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetData(vm);
        }

        public static GridModel GetGridView(string name, bool exporting, ProductsWithoutCategoriesList vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.5", vm.Name, false);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "ProductsWithoutCategories", Action = "GrdMainCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;            
            grid.Settings.SettingsPager.PageSizeItemSettings.Items = new string[] {"25", "50", "100", "1000"};

            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<ProductsWithoutCategoriesInfo>)vm.Grids["GrdMain"].Data;

                foreach (var col in mvCxGridView.Columns)
                {
                    if (col.GetType() != typeof(MVCxGridViewColumn)) continue;

                    var column = (MVCxGridViewColumn)col;

                    if (column.SortIndex == 0)
                    {
                        vm.SortBy = column.FieldName;
                        vm.SortDirection = (column.SortOrder == ColumnSortOrder.Ascending ? "ASC" : "DESC");               
                        break;
                    }
                }
            };
            
            grid.Settings.Columns.Add(s =>
            {
                s.Name = "ExportColumn";
                s.Width = 70;
                s.Caption = Setup.GetGridViewExportHeaderTemplate(grid.Settings.Name, grid.Settings.SettingsCookies.CookiesID, null);
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierPartNumber";
                s.Caption = grid.Label(200965); // SKU
                s.Width = 180;
                s.SortIndex = 0;
                s.SortOrder = ColumnSortOrder.Ascending;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ShortDescription";
                s.Caption = grid.Label(200970); // Product name;    
                s.Width = 350;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "LongDescription";
                s.Caption = grid.Label(200970); // Long description
                s.Width = 350;
                s.Visible = false;              // Only visible on grid export
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Brand";
                s.Caption = grid.Label(200618); // Brand  
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierCategory";
                s.Caption = grid.Label(200967); // Category name    
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierSubcategory1";
                s.Caption = grid.Label(200971); // Sub-category 1
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierSubcategory2";
                s.Caption = grid.Label(200972); // Sub-category 2
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsTVSeries";
                s.Caption = grid.Label(300044); // TV series?
                s.Width = 115;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            return grid;
        }

        public static List<ProductsWithoutCategoriesInfo> GetData(ProductsWithoutCategoriesList vm)
        {
            string feedName = vm.Feeds.Find(x => x.FeedKey == vm.FeedKey).FeedName;

            return ProductManager.GetProductsWithoutCategories(feedName, vm.SortBy, vm.SortDirection);
        }

        public static List<CategoryInfo> GetCategories()
        {
            return CategoryManager.GetCategories().OrderBy(x => x.CategoryPath).ToList();
        }

        public static List<FeedInfo> GetFeeds()
        {
            return FeedManager.GetUsedFeeds().OrderBy(x => x.FeedName).ToList();
        }

        public static Exception Assign(ProductsWithoutCategoriesList vm, int categoryID, string productIDList)
        {
            Exception ex = ProductManager.AssignCategoryToProducts(categoryID, productIDList, vm.SI.Company.CompanyID, vm.SI.CurrentRegion.RegionID, vm.SI.User.UserID);

            if (ex == null)
            {
                // We can assume that these were updated and remove locally from our list

                List<ProductsWithoutCategoriesInfo> list = (List<ProductsWithoutCategoriesInfo>)vm.Grids["GrdMain"].Data;
                int[] ids = productIDList.Split(',').Select(int.Parse).ToArray();

                list.RemoveAll(x => x.ProductID.In(ids));
            }

            return ex;
        }
    }
}

