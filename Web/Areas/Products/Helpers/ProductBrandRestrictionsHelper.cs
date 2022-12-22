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
    public class ProductBrandRestrictionsHelper
    {
        public static ProductBrandRestrictionsList CreateModel()
        {
            var vm = new ProductBrandRestrictionsList();

            vm.SortBy = "SupplierPartNumber";  // On first grid load
            vm.SortDirection = "ASC";
            vm.Brands = GetBrands();
            vm.Catalogs = GetCatalogs();

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = new List<ProductBrandRestrictionInfo>();

            return vm;
        }

        public static void UpdateModel(ProductBrandRestrictionsList vm, ProductBrandRestrictionsList cached, bool isExporting)
        {
            vm.Brands = cached.Brands;
            vm.Catalogs = cached.Catalogs;
            vm.Grids = cached.Grids;

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetData(vm);
        }

        public static GridModel GetGridView(string name, bool exporting, ProductBrandRestrictionsList vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.3", vm.Name, false);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "ProductBrandRestrictions", Action = "GrdMainCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            grid.Settings.SettingsPager.PageSizeItemSettings.Items = new string[] {"25", "50", "100", "1000"};

            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<ProductBrandRestrictionInfo>)vm.Grids["GrdMain"].Data;

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
                s.FieldName = "SupplierName";
                s.Caption = grid.Label(300102); // SupplierName
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
                s.FieldName = "SupplierSubCategory1";
                s.Caption = grid.Label(200971); // Sub-category 1
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierSubCategory2";
                s.Caption = grid.Label(200972); // Sub-category 2
                s.Width = 220;
                s.EditFormSettings.Visible = DefaultBoolean.False;
            });

            return grid;
        }

        public static List<ProductBrandRestrictionInfo> GetData(ProductBrandRestrictionsList vm)
        {
            string brandName = vm.Brands.Find(x => x.BrandKey == vm.BrandKey).BrandName;

            return ProductManager.GetProductBrandRestrictions(brandName, vm.SortBy, vm.SortDirection);
        }

        public static List<BrandInfo> GetBrands()
        {
            return BrandManager.GetBrands().OrderBy(x => x.BrandName).ToList();
        }

        public static List<CatalogInfo> GetCatalogs()
        {
            return CatalogManager.GetCatalogs().OrderBy(x => x.Catalog).ToList();
        }

        public static Exception Assign(ProductBrandRestrictionsList vm, int catalogKey, string productIDList)
        {
            string catalogName = vm.Catalogs.Find(x => x.CatalogID == catalogKey).Catalog;

            Exception ex = ProductManager.AssignCatalogToProducts(catalogName, productIDList, vm.SI.Company.CompanyID, vm.SI.CurrentRegion.RegionID, vm.SI.User.UserID);

            if (ex == null)
            {
                // We can assume that these were updated and remove locally from our list

                List<ProductBrandRestrictionInfo> list = (List<ProductBrandRestrictionInfo>)vm.Grids["GrdMain"].Data;
                int[] ids = productIDList.Split(',').Select(int.Parse).ToArray();

                list.RemoveAll(x => x.ProductID.In(ids));
            }

            return ex;
        }
    }
}