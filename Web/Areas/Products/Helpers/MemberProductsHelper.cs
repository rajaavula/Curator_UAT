using Cortex.Utilities;
using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.IO;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class MemberProductsHelper
    {
        public static MemberProducts CreateModel()
        {
            MemberProducts vm = new MemberProducts();
            vm.MemberStoreList = GetMemberStores(vm);
            if (vm.MemberStoreList != null && vm.MemberStoreList.Count > 0) vm.StoreID = vm.MemberStoreList.First().StoreID;
            else
            {
                StoreInfo storeInfo = new StoreInfo();
                storeInfo.StoreID = 0;
                storeInfo.StoreName = "No stores assigned";
                vm.MemberStoreList.Add(storeInfo);
                vm.StoreID = vm.MemberStoreList.First().StoreID;
            }
            vm.SortBy = "SupplierPartNumber";  // On first grid load            
            vm.SortDirection = "ASC";

            vm.Feeds = FeedManager.GetMemberFeeds(vm.StoreID).OrderBy(x => x.FeedName).ToList();
            if (vm.Feeds != null && vm.Feeds.Count > 0) vm.FeedKey = vm.Feeds.First().FeedKey;
            else
            {
                FeedInfo feedInfo = new FeedInfo();
                feedInfo.FeedKey = 0;
                feedInfo.FeedName = "No feeds assigned";
                vm.Feeds.Add(feedInfo);
                vm.FeedKey = vm.Feeds.First().FeedKey;
            }

            vm.MemberFeedsByStore = GetMemberFeedsByStore(vm).OrderBy(x => x.StoreName).ToList();

            vm.MemberFeedModel = GetMemberFeedComboBoxSettings(vm);
            vm.Categories = GetMemberCategories(vm);
            vm.BrandByFeed = GetBrandsByFeed();
            vm.MemberCategoriesByFeed = GetMemberCategoriesByFeed(vm);
            vm.MemberCategoryModel = GetMemberCategoryComboBoxSettings(vm);
            vm.BrandModel = GetBrandComboBoxSettings(vm);
            vm.BulkPRImportUploadControlSettings = GetBulkPRImportUploadSettings();
            vm.BulkPRImportPopUploadSettings = GetBulkPRImportPopupSettings();
            vm.ProductTag = GetTagsforProduct(vm, vm.ProductID);
            vm.MemberProductTags = GetMemberProductTags(vm);

            vm.Grids.Add("GrdMain", GetProductsGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = new List<ProductInfo>();

            vm.Grids.Add("GrdAvailable", GetAvailableProductsGridView("GrdAvailable", false, vm));
            vm.Grids["GrdAvailable"].Data = new List<ProductInfo>();

            return vm;
        }

        public static void UpdateModel(MemberProducts vm, MemberProducts cached, bool isExporting)
        {
            vm.SortBy = cached.SortBy;
            vm.SortDirection = cached.SortDirection;
            vm.Grids = cached.Grids;
            vm.Feeds = cached.Feeds;
            vm.Categories = cached.Categories;
            vm.MemberStoreList = cached.MemberStoreList;
            vm.MemberFeedsByStore = cached.MemberFeedsByStore;
            vm.MemberFeedModel = GetMemberFeedComboBoxSettings(vm);
            vm.MemberCategoriesByFeed = cached.MemberCategoriesByFeed;
            vm.MemberCategoryModel = GetMemberCategoryComboBoxSettings(vm);
            vm.BrandByFeed = cached.BrandByFeed;
            vm.BrandModel = GetBrandComboBoxSettings(vm);
            vm.ProductTag = GetTagsforProduct(vm, vm.ProductID);
            vm.BulkPRImportUploadControlSettings = cached.BulkPRImportUploadControlSettings;
            vm.BulkPRImportPopUploadSettings = cached.BulkPRImportPopUploadSettings;

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetMemberProductsData(vm);
            vm.Grids["GrdAvailable"].Data = GetAvailableProductsData(vm);
        }

        public static GridModel GetProductsGridView(string name, bool exporting, MemberProducts vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.9", vm.Name, false);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "MemberProducts", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.EndCallback = "function (s, e) { Get(); }";
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;

            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<ProductInfo>)vm.Grids["GrdMain"].Data;
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

            grid.Settings.HtmlDataCellPrepared = (sender, e) =>
            {
                if (e.DataColumn.FieldName == "NegativeProfit" && (bool)e.CellValue)
                {
                    e.Cell.ControlStyle.BackColor = ColorTranslator.FromHtml(App.GlobalThemeBaseColor);
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
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
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
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category2Name";
                s.Caption = grid.Label(200974); // LE category 2;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category3Name";
                s.Caption = grid.Label(200975); // LE category 3;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category4Name";
                s.Caption = grid.Label(200976); // LE category 4;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category5Name";
                s.Caption = grid.Label(300017); // LE category 5;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ProductTags";
                s.Caption = grid.Label(200977); // Product Tags;
                s.Width = 300;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "BuyPrice";
                s.Caption = grid.Label(200978); // Buy price;
                s.Width = 120;
                s.PropertiesEdit.DisplayFormatString = "n2";
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
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StockOnHand";
                s.Caption = grid.Label(200981); // SOH;
                s.Width = 120;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "PricingRuleText";
                s.Caption = grid.Label(201033); // Pricing rule;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "PriceValue";
                s.Caption = grid.Label(201039); // Price value;
                s.Width = 120;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "RetailRounding";
                s.Caption = grid.Label(201040); // Retail Rounded?;
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "NewRRP";
                s.Caption = grid.Label(201036); // new RRP;
                s.Width = 120;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "MarkUpPercentage";
                s.Caption = grid.Label(200980); // Markup;
                s.Width = 120;
                s.PropertiesEdit.DisplayFormatString = "{0:n1}%";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "NegativeProfit";
                s.Caption = grid.Label(300020); // Negative?;
                s.Width = 120;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "MemberPriceModifiedDate";
                s.Caption = grid.Label(201038); // Modified date;
                s.Width = 150;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;

            });

            grid.Settings.Settings.ShowFooter = true;
            string preview = U.RenderPartialToString("~/Areas/Products/Views/MemberProducts/ListPreview.ascx", grid);
            grid.Settings.SetFooterRowTemplateContent(preview);

            return grid;
        }

        public static GridModel GetAvailableProductsGridView(string name, bool exporting, MemberProducts vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.6", vm.Name, false);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "MemberProducts", Action = "GrdAvailableCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            grid.Settings.ClientSideEvents.BeginCallback = "function (s,e) { GrdAvailable_BeginCallback(s, e, '" + vm.Name + "'); }";
            grid.Settings.ClientSideEvents.EndCallback = "function (s,e) { GrdAvailable_EndCallback(s, true); }";

            grid.Settings.BeforeColumnSortingGrouping = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                var data = (List<ProductInfo>)vm.Grids["GrdAvailable"].Data;
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

            grid.Settings.CustomJSProperties = (sender, e) =>
            {
                MVCxGridView g = sender as MVCxGridView;
                var selected = new List<int>();

                for (var i = 0; i < g.VisibleRowCount; i++)
                {
                    int id = (int)g.GetRowValues(i, g.KeyFieldName);
                    bool assigned = (bool)g.GetRowValues(i, "Selected");

                    if (!assigned) continue;

                    selected.Add(id);
                }

                e.Properties["cpSelected"] = selected;
            };

            grid.Settings.PreRender = (sender, e) =>
            {
                MVCxGridView gridView = sender as MVCxGridView;
                var data = (List<ProductInfo>)vm.Grids["GrdAvailable"].Data;
                if (data == null) return;

                var selected = data.FindAll(x => x.Selected);
                foreach (var product in selected)
                {
                    gridView.Selection.SelectRowByKey(product.ProductID);
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
                s.SortIndex = 0;                            // Default vm.SortBy
                s.SortOrder = ColumnSortOrder.Ascending;    // Default vm.SortDirection
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ShortDescription";
                s.Caption = grid.Label(200970); // Product name   
                s.Width = 350;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "Selected";
                s.Caption = grid.Label(300086); // Selected    
                s.Width = 100;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Brand";
                s.Caption = grid.Label(200618); // Brand;
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
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
                s.Caption = grid.Label(200973); // LE category 1
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category2Name";
                s.Caption = grid.Label(200974); // LE category 2
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category3Name";
                s.Caption = grid.Label(200975); // LE category 3
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category4Name";
                s.Caption = grid.Label(200976); // LE category 4
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Category5Name";
                s.Caption = grid.Label(300017); // LE category 5
                s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            return grid;
        }

        public static CheckComboModel GetMemberCategoryComboBoxSettings(MemberProducts vm)
        {
            var model = new CheckComboModel();

            Setup.CheckBoxCombo(model.Settings, "CategoryKey");

            // UI
            var dropdown = model.Settings.DropDownSettings;
            dropdown.Width = Unit.Pixel(150);

            var listbox = model.Settings.ListBoxSettings;

            listbox.CallbackRouteValues = new { Area = "Products", Controller = "MemberProducts", Action = "MemberCategoryCallback" };
            listbox.Properties.ClientSideEvents.BeginCallback = "function(s,e) { MemberCategory_BeginCallback(s,e); }";
            listbox.Properties.ClientSideEvents.EndCallback = "function(s,e) { MemberCategory_EndCallback(s,e); }";
            listbox.Properties.ClientSideEvents.Init = "function(s,e) { MemberCategory_EndCallback(s,e); }";


            // Data          
            var currentFeedCategories = vm.MemberCategoriesByFeed.FindAll(x => x.FeedKey == vm.FeedKey).Select(x => x.CategoryKey).ToArray();

            var data = (from x in vm.Categories.FindAll(x => x.CategoryKey.In(currentFeedCategories)).OrderBy(x => x.Description)
                        select new ListEditItem(x.CategoryPath, x.CategoryKey)).ToList();     // Note sorted by category desc not name

            data.Insert(0, new ListEditItem("ALL", null));   // Add null option

            if (data.Count > 1)
            {
                vm.SelectedCategories = Convert.ToString(data[1].Value);
                model.Settings.ListBoxSettings.SelectedIndex = 1;
            }
            else
            {
                model.Settings.ListBoxSettings.SelectedIndex = 0;
            }

            model.Data = data;

            return model;
        }

        public static CheckComboModel GetBrandComboBoxSettings(MemberProducts vm)
        {
            var model = new CheckComboModel();

            Setup.CheckBoxCombo(model.Settings, "Brand");

            // UI
            var dropdown = model.Settings.DropDownSettings;
            dropdown.Width = Unit.Pixel(150);

            var listbox = model.Settings.ListBoxSettings;

            listbox.CallbackRouteValues = new { Area = "Products", Controller = "MemberProducts", Action = "BrandCallback" };
            listbox.Properties.ClientSideEvents.BeginCallback = "function(s,e) { Brand_BeginCallback(s,e); }";
            listbox.Properties.ClientSideEvents.EndCallback = "function(s,e) { Brand_EndCallback(s,e); }";
            listbox.Properties.ClientSideEvents.Init = "function(s,e) { Brand_EndCallback(s,e); }";


            // Data          
            var data = (from x in vm.BrandByFeed.FindAll(x => x.FeedKey == vm.FeedKey).OrderBy(x => x.BrandName)
                        select new ListEditItem(x.BrandName, x.BrandKey)).ToList();

            data.Insert(0, new ListEditItem("ALL", null));   // Add null option

            if (data.Count > 1)
            {
                vm.SelectedBrands = Convert.ToString(data[1].Value);
                model.Settings.ListBoxSettings.SelectedIndex = 1;
            }
            else
            {
                model.Settings.ListBoxSettings.SelectedIndex = 0;
            }

            model.Data = data;

            return model;
        }

        public static ComboBoxModel GetMemberFeedComboBoxSettings(MemberProducts vm)
        {
            ComboBoxModel model = new ComboBoxModel();
            model.Settings = new ComboBoxSettings();
            model.Data = vm.Feeds;
            model.Value = vm.FeedKey;

            var s = model.Settings;
            Setup.ComboBox(s);

            s.Name = "FeedKey";
            s.Properties.ValueField = "FeedKey";
            s.Properties.ValueType = typeof(int?);
            s.Properties.TextField = "FeedName";
            s.Width = Unit.Pixel(150);
            s.Properties.AllowNull = true;
            s.Properties.CallbackPageSize = 100;
            s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeMemberFeed(); }";
            s.CallbackRouteValues = new { Area = "Products", Controller = "MemberProducts", Action = "MemberFeedCallback" };
            s.Properties.ClientSideEvents.BeginCallback = "function(s,e) { MemberFeed_BeginCallback(s, e); }";
            s.Properties.ClientSideEvents.EndCallback = "function(s,e) { MemberFeed_EndCallback(s, e); }";

            return model;
        }

        public static UploadControlSettings GetBulkPRImportUploadSettings()
        {
            UploadControlSettings s = new UploadControlSettings();
            Setup.UploadControl(s, "uplBulkPRImport");

            s.CallbackRouteValues = new { area = "Products", controller = "MemberProducts", action = "BulkPRImportUpload" };

            s.ClientSideEvents.FilesUploadStart = "function(s,e) { BulkPRImportStart(s,e); }";
            s.ClientSideEvents.FileUploadComplete = "function(s,e) { BulkPRImportComplete(s,e); }";
            s.ClientSideEvents.TextChanged = "function(s,e) { BulkPRImportTextChanged(s,e); }";

            UploadControlValidationSettings vs = GetBulkPRImportValidationSettings();
            s.ValidationSettings.Assign(vs);

            return s;
        }

        public static UploadControlValidationSettings GetBulkPRImportValidationSettings()
        {
            UploadControlValidationSettings s = new UploadControlValidationSettings();
            s.AllowedFileExtensions = new[] { ".xlsx" };
            s.MaxFileSize = 20971520;
            s.ShowErrors = false;
            return s;
        }

        public static void BulkPRImport(MemberProducts vm)
        {
            HttpContext.Current.Session["MemberProducts_Model"] = vm;
            UploadControlValidationSettings validationSettings = MemberProductsHelper.GetBulkPRImportValidationSettings();
            UploadControlExtension.GetUploadedFiles("uplBulkPRImport", validationSettings, MemberProductsHelper.BulkPRImport_FileUploadComplete);
        }

        public static void BulkPRImport_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            MemberProducts vm = (MemberProducts)HttpContext.Current.Session["MemberProducts_Model"];
            List<ProductInfo> feedProducts = ProductManager.GetMemberProducts(vm.StoreID, vm.FeedKey, null, null, vm.SortBy, vm.SortDirection);
            List<int> productIDs;

            e.IsValid = true;
            e.ErrorText = String.Empty;

            try
            {
                Stream filestream = Utils.ConvertByteArrayToStream(e.UploadedFile.FileBytes);
                ExcelPackage excel = new ExcelPackage();
                excel.Load(filestream);

                DataTable dt = U.ExcelWorksheetToDataTable(excel, true);
                if (dt == null) throw new Exception("Error opening spreadsheet. Please check the spreadsheet.");

                if (dt.Rows.Count <= 0) return; // Empty spreadsheet

                List<string> headings = new List<string>()
                {
                "Product ID","Supplier Part Number","Pricing Rule","Price Value","Retail Rounding","Default Product Tags","Product Tags"
                };

                List<string> columns = (from DataColumn dc in dt.Columns select dc.ColumnName).ToList();
                if (headings.Count() != columns.Count())
                {
                    e.ErrorText = "Please ensure the spreadsheet has the correct number of columns";
                    return;
                }

                foreach (string columnName in headings)
                {
                    if (!columns.Contains(columnName))
                    {
                        e.ErrorText = "Please ensure the spreadsheet has the correct headings";
                        return;
                    }
                }

                List<string> errors = ValidateBulkPRImport(vm, dt, feedProducts);

                if (errors.Count < 1)
                {
                    Exception ex = ImportBulkPRs(vm, dt, out productIDs);
                    if (ex != null) errors.Add(ex.Message);

                    feedProducts = ProductManager.GetMemberProducts(vm.StoreID, vm.FeedKey, null, null, vm.SortBy, vm.SortDirection);
                    List<ProductInfo> FeedProductsFiltered = feedProducts.Where(x => productIDs.Any(y => y == x.ProductID)).ToList();
                    vm.Grids["GrdMain"].Data = FeedProductsFiltered;
                }

                if (errors.Count > 0)
                {
                    e.IsValid = false;
                    e.ErrorText = String.Join(Environment.NewLine, errors);
                    return;
                }
            }
            catch
            {
                e.IsValid = false;
                e.ErrorText = "Error opening spreadsheet. Please check the spreadsheet.";
            }
            finally
            {
                HttpContext.Current.Session.Remove("MemberProducts_Model");
            }
        }

        private static List<string> ValidateBulkPRImport(MemberProducts vm, DataTable dt, List<ProductInfo> feedProducts)
        {
            int lineNumber = 1;
            List<string> priceRules = new List<string>() { "No pricing", "New RRP", "RRP adjustment $", "RRP adjustment %", "Buy price adjustment $", "Buy price adjustment %" };

            List<string> errors = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                lineNumber++;
                try
                {
                    string productID = Utils.FromDBValue<string>(dr["Product ID"]);
                    string supplierPartNumber = Utils.FromDBValue<string>(dr["Supplier Part Number"]);
                    string pricingRule = Utils.FromDBValue<string>(dr["Pricing Rule"]);
                    string priceValue = Utils.FromDBValue<string>(dr["Price Value"]);
                    string retailRounding = Utils.FromDBValue<string>(dr["Retail Rounding"]);
                    string defaultProductTags = Utils.FromDBValue<string>(dr["Default Product Tags"]);
                    string productTags = Utils.FromDBValue<string>(dr["Product Tags"]);

                    if (String.IsNullOrEmpty(productID))
                    {
                        errors.Add(String.Format("(Line number: {0}) Product ID cannot be empty.", lineNumber));
                    }

                    if (String.IsNullOrEmpty(supplierPartNumber))
                    {
                        errors.Add(String.Format("(Line number: {0}) Supplier Part Number cannot be empty.", lineNumber));
                    }
                    else
                    {
                        var skuMatches = from x in feedProducts.FindAll(x => x.SupplierPartNumber == supplierPartNumber)
                                         select new string(x.SupplierPartNumber.ToCharArray()).ToList();
                        if (skuMatches.Count() <= 0)
                        {
                            errors.Add(String.Format("(Line number: {0}) This SKU can not be found for this store/feed.", lineNumber));
                        }
                    }

                    if (String.IsNullOrEmpty(pricingRule))
                    {
                        errors.Add(String.Format("(Line number: {0}) Pricing Rule cannot be empty.", lineNumber));
                    }
                    else
                    {
                        if (!priceRules.Contains(pricingRule)) //If not found in the list of rules
                        {
                            errors.Add(String.Format("(Line number: {0}) Pricing Rule not valid.", lineNumber));
                        }
                    }

                    if (String.IsNullOrEmpty(priceValue))
                    {
                        errors.Add(String.Format("(Line number: {0}) Price cannot be empty.", lineNumber));
                    }
                    else
                    {
                        decimal priceCast;
                        if (decimal.TryParse(priceValue, out priceCast) == false)
                        {
                            errors.Add(String.Format("(Line number: {0}) Invalid Price Value: {1}. Must be a number.", lineNumber, priceValue));
                        }
                        if (priceCast <= 0)
                        {
                            errors.Add(String.Format("(Line number: {0}) Invalid Price Value: {1}. Must be greater than 0.", lineNumber, priceValue));
                        }
                    }

                    if (String.IsNullOrEmpty(retailRounding))
                    {
                        errors.Add(String.Format("(Line number: {0}) Retail Rounding cannot be empty.", lineNumber));
                    }
                    else
                    {
                        if (retailRounding != "False" && retailRounding != "True")
                        {
                            errors.Add(String.Format("(Line number: {0}) Retail Rounding must be either TRUE or FALSE.", lineNumber));
                        }
                    }

                    if (String.IsNullOrEmpty(defaultProductTags))
                    {
                        errors.Add(String.Format("(Line number: {0}) Default Product Tags cannot be empty.", lineNumber));
                    }
                    else
                    {
                        if (defaultProductTags != "False" && defaultProductTags != "True")
                        {
                            errors.Add(String.Format("(Line number: {0}) Default Product Tags must be either TRUE or FALSE.", lineNumber));
                        }
                    }

                    if (defaultProductTags == "True")
                    {
                        if (!String.IsNullOrEmpty(productTags))
                        {
                            errors.Add(String.Format("(Line number: {0}) Product Tags cannot be set if Default Product Tags are TRUE.", lineNumber));
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(productTags))
                        {
                            try
                            {
                                string[] commaSep = productTags.Split(',');
                                if (commaSep.Contains(""))
                                {
                                    errors.Add(String.Format("(Line number: {0}) Product Tags are not formatted correctly.", lineNumber));
                                }

                                for (int i = 0; i < commaSep.Length; i++)
                                {
                                    string tag = commaSep[i];
                                    if (tag[0] == ' ' || tag[tag.Length - 1] == ' ')
                                    {
                                        errors.Add(String.Format("(Line number: {0}) Product Tags are not formatted correctly.", lineNumber));
                                    }
                                }
                            }
                            catch
                            {
                                errors.Add(String.Format("(Line number: {0}) Product Tags are not formatted correctly.", lineNumber));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorText = String.Format("(Line number: {0}) There was an error importing the price rule data. Please check the spreadsheet.", lineNumber);
                    Log.Write(ex.Message);
                    errors.Add(errorText);
                }
            }
            return errors;
        }

        private static Exception ImportBulkPRs(MemberProducts vm, DataTable dt, out List<int> productIDs)
        {
            productIDs = new List<int>();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string productID = Utils.FromDBValue<string>(dr["Product ID"]);
                    string supplierPartNumber = Utils.FromDBValue<string>(dr["Supplier Part Number"]);
                    string pricingRule = Utils.FromDBValue<string>(dr["Pricing Rule"]);
                    string priceValue = Utils.FromDBValue<string>(dr["Price Value"]);
                    string retailRounding = Utils.FromDBValue<string>(dr["Retail Rounding"]);
                    string defaultProductTags = Utils.FromDBValue<string>(dr["Default Product Tags"]);
                    string productTags = Utils.FromDBValue<string>(dr["Product Tags"]);

                    if (productTags == null)
                    {
                        productTags = ""; //Cell in excel that is empty can be returned as null, need to set to empty string to process.
                    }

                    if (pricingRule == "No pricing") pricingRule = "0";
                    if (pricingRule == "New RRP") pricingRule = "1";
                    if (pricingRule == "RRP adjustment $") pricingRule = "2";
                    if (pricingRule == "RRP adjustment %") pricingRule = "3";
                    if (pricingRule == "Buy price adjustment $") pricingRule = "4";
                    if (pricingRule == "Buy price adjustment %") pricingRule = "5";

                    Exception ex = ProductManager.SaveMemberPricing(vm.StoreID, productID, Convert.ToInt32(pricingRule), Convert.ToDecimal(priceValue), Convert.ToBoolean(retailRounding), vm.SI.User.UserID);
                    if (ex != null) throw ex;

                    ex = ProductManager.SaveProductTags(vm.StoreID, productID, productTags, Convert.ToBoolean(defaultProductTags), vm.SI.User.UserID);
                    if (ex != null) throw ex;

                    productIDs.Add(Convert.ToInt32(productID));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ex;
            }

            return null;
        }

        public static PopupControlSettings GetBulkPRImportPopupSettings()
        {
            PopupControlSettings s = new PopupControlSettings();
            Setup.PopupControl(s);
            s.Name = "ppBulkPRImportResult";
            s.HeaderText = "Bulk Price Rule Import Errors";
            s.Modal = true;
            s.ControlStyle.CssClass = "cortex-popup";
            s.Width = Unit.Pixel(600);
            s.ControlStyle.HorizontalAlign = HorizontalAlign.Left;
            return s;
        }

        public static byte[] GetBulkPRExportDownload(MemberProducts vm)
        {
            var ExportData = (List<ProductInfo>)vm.Grids["GrdMain"].Data;

            DataTable ExportTable = new DataTable();
            ExportTable.Columns.Add("Product ID", Type.GetType("System.Int32"));
            ExportTable.Columns.Add("Supplier Part Number", Type.GetType("System.String"));
            ExportTable.Columns.Add("Pricing Rule", Type.GetType("System.String"));
            ExportTable.Columns.Add("Price Value", Type.GetType("System.Decimal"));
            ExportTable.Columns.Add("Retail Rounding", Type.GetType("System.Boolean"));
            ExportTable.Columns.Add("Default Product Tags", Type.GetType("System.Boolean"));
            ExportTable.Columns.Add("Product Tags", Type.GetType("System.String"));

            DataRow ExportTableRow;
            if (ExportData != null)  //Will raise exception if there is no data in grid
            {
                foreach (var x in ExportData)
                {
                    ExportTableRow = ExportTable.NewRow();
                    ExportTableRow["Product ID"] = x.ProductID;
                    ExportTableRow["Supplier Part Number"] = x.SupplierPartNumber;

                    string priceRule = "No pricing";
                    if (x.PricingRule == 0) priceRule = "No pricing";
                    if (x.PricingRule == 1) priceRule = "New RRP";
                    if (x.PricingRule == 2) priceRule = "RRP adjustment $";
                    if (x.PricingRule == 3) priceRule = "RRP adjustment %";
                    if (x.PricingRule == 4) priceRule = "Buy price adjustment $";
                    if (x.PricingRule == 5) priceRule = "Buy price adjustment %";

                    ExportTableRow["Pricing Rule"] = priceRule;
                    ExportTableRow["Price Value"] = x.PriceValue;
                    ExportTableRow["Retail Rounding"] = x.RetailRounding;
                    ExportTableRow["Default Product Tags"] = x.DefaultProductTags;
                    ExportTableRow["Product Tags"] = x.ProductTags;
                    ExportTable.Rows.Add(ExportTableRow);
                }
            }

            byte[] ExcelData;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ExportPR");
                if (ExportData != null)
                {
                    ws.Column(1).Width = 12;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 30;
                    ws.Column(4).Width = 12;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 18;
                    ws.Column(7).Width = 60;
                    ws.Column(7).Style.WrapText = true;
                    ws.Column(2).Style.Numberformat.Format = "@"; //Format as Text
                    ws.Column(7).Style.Numberformat.Format = "@";

                    ws.Cells["A1"].LoadFromDataTable(ExportTable, true);

                    var val = ws.DataValidations.AddListValidation("C2:C" + Convert.ToString((ExportTable.Rows.Count + 1)));
                    val.ShowErrorMessage = true;
                    val.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                    val.ErrorTitle = "Incorrect Value";
                    val.Error = "Please choose a valid Price Rule";
                    val.Formula.Values.Add("No pricing");
                    val.Formula.Values.Add("New RRP");
                    val.Formula.Values.Add("RRP adjustment $");
                    val.Formula.Values.Add("RRP adjustment %");
                    val.Formula.Values.Add("Buy price adjustment $");
                    val.Formula.Values.Add("Buy price adjustment %");
                    val.AllowBlank = false;
                }

                MemoryStream ms = new MemoryStream();
                pck.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);

                byte[] buffer = new byte[(int)ms.Length];
                buffer = ms.ToArray();
                ExcelData = buffer;
            }
            return ExcelData;
        }

        public static List<FeedInfo> GetMemberFeeds(MemberProducts vm)
        {
            string storeName = vm.MemberStoreList.Find(x => x.StoreID == vm.StoreID).StoreName;
            var filteredFeeds = vm.MemberFeedsByStore.FindAll(x => x.StoreName == storeName);
            if (filteredFeeds.Count > 1) vm.FeedKey = filteredFeeds.First().FeedKey;

            return (from x in filteredFeeds select new FeedInfo { FeedKey = x.FeedKey, FeedName = x.FeedName }).OrderBy(x => x.FeedName).ToList();
        }

        public static List<ProductInfo> GetMemberProductsData(MemberProducts vm)
        {
            return ProductManager.GetMemberProducts(vm.StoreID, vm.FeedKey, vm.SelectedCategories, vm.SelectedBrands, vm.SortBy, vm.SortDirection);
        }

        public static List<ProductInfo> GetAvailableProductsData(MemberProducts vm)
        {
            return ProductManager.GetAvailableMemberProducts(vm.StoreID, vm.FeedKey, vm.SelectedCategories, vm.SelectedBrands, vm.SortBy, vm.SortDirection);
        }

        public static List<BrandByFeedInfo> GetBrandsByFeed()
        {
            return BrandManager.GetBrandsByFeed().ToList();
        }

        public static List<MemberFeedByStoreInfo> GetMemberFeedsByStore(MemberProducts vm)
        {

            return FeedManager.GetMemberFeedsByStore();
        }

        public static List<MemberCategoryByFeedInfo> GetMemberCategoriesByFeed(MemberProducts vm)
        {
            return CategoryManager.GetMemberCategoriesByFeed(vm.StoreID, vm.FeedKey);
        }

        public static List<CategoryInfo> GetMemberCategories(MemberProducts vm)
        {
            return CategoryManager.GetMemberCategories(vm.StoreID).OrderBy(x => x.CategoryPath).ToList();
        }

        public static List<ProductInfo> GetMemberProducts(int storeID, int feedKey, string selectedCategories, string selectedBrands, string sortBy, string sortDirection)
        {
            return ProductManager.GetMemberProducts(storeID, feedKey, selectedCategories, selectedBrands, sortBy, sortDirection);
        }

        public static List<ProductInfo> GetAvailableMemberProducts(int storeID, int feedKey, string selectedCategories, string selectedBrands, string sortBy, string sortDirection)
        {
            return ProductManager.GetAvailableMemberProducts(storeID, feedKey, selectedCategories, selectedBrands, sortBy, sortDirection);
        }

        public static List<TagInfo> GetMemberProductTags(MemberProducts vm)
        {
            return ProductManager.GetProductTags(vm.StoreID).OrderBy(x => x.ProductTags).ToList();
        }

        public static List<TagInfo> GetTagsforProduct(MemberProducts vm, int id)
        {
            return ProductManager.GetTagsforProduct(vm.StoreID, id);
        }

        public static ProductInfo GetMemberProductInfo(MemberProducts vm, int id)
        {
            var data = (List<ProductInfo>)vm.Grids["GrdMain"].Data;

            return data.Find(x => x.ProductID == id);
        }

        public static Exception Save(MemberProducts vm, string productIDList, int pricingRule, decimal priceValue, bool retailRounding)
        {
            if (vm.StoreID == 0) return new Exception("User does not belong to a store");

            Exception ex = ProductManager.SaveMemberPricing(vm.StoreID, productIDList, pricingRule, priceValue, retailRounding, vm.SI.User.UserID);

            if (ex == null)
            {
                // We can assume that these were updated 

                List<ProductInfo> list = (List<ProductInfo>)vm.Grids["GrdMain"].Data;
                int[] ids = productIDList.Split(',').Select(int.Parse).ToArray();
                list.RemoveAll(x => x.ProductID.In(ids));
                vm.Grids["GrdMain"].Data = GetMemberProductsData(vm);
            }

            return ex;
        }

        public static Exception SaveProductTags(MemberProducts vm, string productIDList, string productTags, bool defaultProductTags)
        {
            if (vm.StoreID == 0) return new Exception("User does not belong to a store");

            Exception ex = ProductManager.SaveProductTags(vm.StoreID, productIDList, productTags, defaultProductTags, vm.SI.User.UserID);

            if (ex == null)
            {
                // We can assume that these were updated 

                List<ProductInfo> list = (List<ProductInfo>)vm.Grids["GrdMain"].Data;
                int[] ids = productIDList.Split(',').Select(int.Parse).ToArray();
                list.RemoveAll(x => x.ProductID.In(ids));

                vm.Grids["GrdMain"].Data = GetMemberProductsData(vm);
            }

            return ex;
        }

        public static object GetPreviewData(MemberProducts vm, int id)
        {
            try
            {
                var info = ((List<ProductInfo>)vm.Grids["GrdMain"].Data).Find(x => x.ProductID == id);

                return new
                {
                    PricingRuleText = info.PricingRuleText,
                    PriceValue = info.PriceValue,
                    RetailRounding = info.RetailRounding,
                    NewRRP = info.NewRRP,
                    ProductTags = $"{info.ProductTags}",   // Convert null to string.Empty
                    DefaultProductTags = info.DefaultProductTags
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static List<StoreInfo> GetMemberStores(MemberProducts vm)
        {
            return ProductManager.GetMemberStoreList(vm.SI.User.UserID).OrderBy(x => x.StoreName).ToList();
        }

        public static Exception UpdateProductSelection(MemberProducts vm, string allKeys, string productIDs)
        {
            if (vm.StoreID == 0) return new Exception("You must have a store ID");

            return ProductManager.UpdateMemberProducts(vm.StoreID, allKeys, productIDs);
        }
    }
}

