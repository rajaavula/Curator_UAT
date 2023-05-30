using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Internal;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class ProductsHelper
    {
        public static ProductsListEdit CreateModel()
        {
            ProductsListEdit vm = new ProductsListEdit();

            vm.SortBy = "SupplierPartNumber";  // On first grid load            
            vm.SortDirection = "ASC";
            vm.Feeds = GetFeeds();
            vm.BrandsBySource = BrandManager.GetBrandsBySource().OrderBy(x => x.MainSource).ToList();
            vm.FeedKey = vm.Feeds.First().FeedKey;
            vm.Brands = GetBrands(vm);
            vm.Categories = GetCategories(vm);

            if (vm.Brands.Count > 1) vm.BrandKey = vm.Brands.First(x => x.BrandKey != null).BrandKey;  // Should always be at least one aside from the null entry

            vm.BrandModel = GetBrandComboBoxSettings(vm);

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            return vm;
        }

        public static void UpdateModel(ProductsListEdit vm, ProductsListEdit cached, bool isExporting)
        {
            vm.SortBy = cached.SortBy;
            vm.SortDirection = cached.SortDirection;
            vm.Feeds = cached.Feeds;
            vm.BrandsBySource = cached.BrandsBySource;
            vm.Brands = GetBrands(vm);
            vm.Categories = GetCategories(vm);
            vm.Grids = cached.Grids;

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetData(vm);
        }

        public static GridModel GetGridView(string name, bool exporting, ProductsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.17", vm.Name);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "Products", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.EndCallback = "function (s, e) { Get(); }";

            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<ProductInfo>)vm.Grids["GrdMain"].Data;                

                foreach (var col in mvCxGridView.Columns)
                {
                    // if (col.)
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
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierPartNumber";
                s.Caption = grid.Label(200965); // SKU
                s.Width = 180;
                s.SortIndex = 0;                            // Default vm.SortBy
                s.SortOrder = ColumnSortOrder.Ascending;    // Default vm.SortDirection
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ShortDescription";
                s.Caption = grid.Label(200970); // Product name;    
                s.Width = 350;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Brand";
                s.Caption = grid.Label(200618); // Brand;    
                s.Width = 220;
            });
            
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "CategoryName";
                s.Caption = grid.Label(200967); // Category name;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category1Name";
                s.Caption = grid.Label(200973); // LE category 1;
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category2Name";
                s.Caption = grid.Label(200974); // LE category 2;
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category3Name";
                s.Caption = grid.Label(200975); // LE category 3;
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category4Name";
                s.Caption = grid.Label(200976); // LE category 4;
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category5Name";
                s.Caption = grid.Label(300017); // LE category 5;
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "LongDescription";
                s.Caption = grid.Label(200970); // Long description;    
                s.Width = 350;
                s.Visible = false;              // Only visible on grid export
            });
                        
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProductTags";
                s.Caption = grid.Label(200977); // Product Tags;
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "RecommendedRetailPrice";
                s.Caption = grid.Label(200993); // RRP; 
                s.Width = 80;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "MemberRecommendedRetailPrice";
                s.Caption = grid.Label(200979); // MRRP; 
                s.Width = 80;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "BuyPrice";
                s.Caption = grid.Label(200978); // Buy price;
                s.Width = 80;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "Markup";
                s.Caption = grid.Label(200980); // Markup;
                s.Width = 80;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StockOnHand";
                s.Caption = grid.Label(200981); // SOH;
                s.Width = 80;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IncludeZeroStock";
                s.Caption = grid.Label(300110); // Include zero stock;
                s.Width = 80;
            });

            grid.Settings.Settings.ShowFooter = true;
            string preview = U.RenderPartialToString("~/Areas/Products/Views/Products/ListPreview.ascx", grid);
            grid.Settings.SetFooterRowTemplateContent(preview);
            grid.Settings.Settings.ShowFooter = true;

            return grid;
        }

        public static List<ProductInfo> GetData(ProductsListEdit vm)
        {
            string feedName = vm.Feeds.Find(x => x.FeedKey == vm.FeedKey).FeedName;
            string brandName = null;
            if (vm.Brands.Count > 0) brandName = vm.Brands.Find(x => x.BrandKey == vm.BrandKey).BrandName;

            return ProductManager.GetProducts(feedName, brandName, vm.SortBy, vm.SortDirection);
        }

        public static List<FeedInfo> GetFeeds()
        {
            return FeedManager.GetAllFeeds();
        }

        public static List<BrandInfo> GetBrands(ProductsListEdit vm)
        {
            string feedName = vm.Feeds.Find(x => x.FeedKey == vm.FeedKey).FeedName;
            var filteredBrands = vm.BrandsBySource.FindAll(x => x.MainSource == feedName);
            filteredBrands.Insert(0, new BrandBySourceInfo() { BrandKey = null, BrandName = null, MainSource = feedName });
            if (filteredBrands.Count > 1) vm.BrandKey = filteredBrands.First(x => x.BrandKey != null).BrandKey;

            return (from x in filteredBrands select new BrandInfo { BrandKey = x.BrandKey, BrandName = x.BrandName }).OrderBy(x => x.BrandName).ToList();
        }

        public static ComboBoxModel GetBrandComboBoxSettings(ProductsListEdit vm)
        {
            ComboBoxModel model = new ComboBoxModel();
            model.Settings = new ComboBoxSettings();
            model.Data = vm.Brands;
            model.Value = vm.BrandKey;

            var s = model.Settings;
            Setup.ComboBox(s);

            s.Name = "BrandKey";
            s.Properties.ValueField = "BrandKey";
            s.Properties.ValueType = typeof(int?);
            s.Properties.TextField = "BrandName";
            s.Width = Unit.Pixel(200);
            s.Properties.AllowNull = true;
            s.Properties.CallbackPageSize = 100;
            s.CallbackRouteValues = new { Area = "Products", Controller = "Products", Action = "BrandCallback" };
            s.Properties.ClientSideEvents.BeginCallback = "function(s,e) { Brand_BeginCallback(s,e); }";
            s.Properties.ClientSideEvents.EndCallback = "function(s,e) { Brand_EndCallback(s,e); }";

            return model;
        }

        public static Exception Save(ProductsListEdit vm, ProductInfo info)
        {
            Exception ex = ProductManager.Save(info);

            vm.Products = GetData(vm);
            vm.Grids["GrdMain"].Data = vm.Products;

            return ex;
        }

        public static object GetPreviewData(ProductsListEdit vm, int id)
        {
            try
            {
                var info = ((List<ProductInfo>)vm.Grids["GrdMain"].Data).Find(x => x.ProductID == id);

                return new
                {
                    LongDescription = info.LongDescription,
                    Category1Name = info.Category1Name,
                    Category2Name = info.Category2Name,
                    Category3Name = info.Category3Name,
                    Category4Name = info.Category4Name,
                    Category5Name = info.Category5Name

                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<CategoryInfo> GetCategories(ProductsListEdit vm)
        {
            return CategoryManager.GetCategories().OrderBy(x => x.CategoryPath).ToList();
        }

        public static ProductInfo GetProduct(ProductsListEdit vm, int id)
        {
            return ((List<ProductInfo>)vm.Grids["GrdMain"].Data).Find(x => x.ProductID == id);
        }

        public static ProductInfo GetProductInfo(ProductsListEdit vm, int id)
        {
            var data = (List<ProductInfo>)vm.Grids["GrdMain"].Data;

            return data.Find(x => x.ProductID == id);
        }

    }
}

