using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class ERPIntegrationsHelper
    {
        public static ERPIntegrationsListEdit CreateModel()
        {
            var vm = new ERPIntegrationsListEdit();

            vm.SortBy = "SupplierPartNumber";  // On first grid load            
            vm.SortDirection = "ASC";

            vm.Grids.Add("GrdMain", GetGrdMainView("GrdMain", false, vm));
            vm.Grids.Add("GrdProducts", GetGrdProductsView("GrdProducts", false, vm));

            return vm;
        }

        public static void UpdateModel(ERPIntegrationsListEdit vm, ERPIntegrationsListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;

            if (isExporting) return;

            vm.Grids["GrdMain"].Data = GetData(vm);
        }

        public static GridModel GetGrdMainView(string name, bool exporting, ERPIntegrationsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.3", vm.Name);
            grid.Settings.KeyFieldName = "VendorID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "ERPIntegrations", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.EndCallback = "function(s,e) { GrdMain_EndCallback(s, e); }";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "VendorName";
                s.Caption = grid.Label(300080); // Vendor Name;
                s.Width = 200;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "MagentoID";
                s.Caption = grid.Label(300073); // Magento ID;
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "MagentoEnabled";
                s.Caption = grid.Label(300074); // Magento Enabled
                s.Width = 150;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NetSuiteInternalID";
                s.Caption = grid.Label(300075); // NetSuite Internal ID;    
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NetSuiteEntityID";
                s.Caption = grid.Label(300076); // NetSuite En ID;    
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NetSuiteCode";
                s.Caption = grid.Label(300077); // NetSuite Code
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NetSuiteName";
                s.Caption = grid.Label(300078); // NetSuite Name
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "NetSuiteEnabled";
                s.Caption = grid.Label(300079); // NetSuite Enabled
                s.Width = 150;
            });

            return grid;
        }

        public static GridModel GetGrdProductsView(string name, bool exporting, ERPIntegrationsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.4", vm.Name);
            grid.Settings.KeyFieldName = "ProductID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "ERPIntegrations", Action = "GrdProductsCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;

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
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "QueuedToNetSuite";
                s.Caption = grid.Label(300084); // Queued To NetSuite;
                s.Width = 150;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "SentToNetSuite";
                s.Caption = grid.Label(300082); // Sent To NetSuite;
                s.Width = 150;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "QueuedToMagento";
                s.Caption = grid.Label(300085); // Queued To Magento;
                s.Width = 150;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "SentToMagento";
                s.Caption = grid.Label(300083); // Sent To Magento;
                s.Width = 150;
                s.PropertiesEdit.Style.HorizontalAlign = HorizontalAlign.Right;
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

            return grid;
        }

        public static List<VendorInfo> GetData(ERPIntegrationsListEdit vm)
        {
            return VendorManager.GetVendors();
        }

        public static List<ERPIntegrationProductInfo> GetProducts(ERPIntegrationsListEdit vm)
        {
            return ProductManager.GetERPIntegrationProducts(vm.VendorID, vm.SortBy, vm.SortDirection);
        }

        public static VendorInfo GetVendor(ERPIntegrationsListEdit vm, Guid vendorID)
        {
            var data = (List<VendorInfo>)vm.Grids["GrdMain"].Data;

            return data.Find(x => x.VendorID == vendorID);
        }

        public static string Update(ERPIntegrationsListEdit vm, VendorInfo info)
        {
            var ex = VendorManager.UpdateVendor(info);
            if (ex != null)
            {
                return "There was an unexpected error updating the vendor's details.";
            }

            vm.Grids["GrdMain"].Data = GetData(vm);

            return null;
        }

        public static Exception Send(ERPIntegrationsListEdit vm, string productIDs)
        {
            var ex = ProductManager.SendSamplesToERPIntegrations(productIDs);

            vm.Grids["GrdProducts"].Data = GetProducts(vm);

            return ex;
        }
    }
}

