using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System.Collections.Generic;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class ProductDashboardHelper
    {
        public static ProductDashboard CreateModel()
        {
            ProductDashboard vm = new ProductDashboard();
            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = GetProductDashboardItems();
            return vm;
        }

        public static void UpdateModel(ProductDashboard vm, ProductDashboard cached, bool isExporting)
        {
            vm.Grids = cached.Grids;
            if (isExporting) return;
            vm.Grids["GrdMain"].Data = GetProductDashboardItems();
        }

        public static GridModel GetGridView(string name, bool exporting, ProductDashboard vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.2", vm.Name);
            grid.Settings.KeyFieldName = "SupplierName";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "ProductDashboard", Action = "GrdMainCallback" };
           
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SupplierName";
                s.Caption = grid.Label(300102); // Supplier name
                s.Width = 250;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "UpdatedRecordCount";
                s.Caption = grid.Label(300149); // Updated items
                s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "CreatedRecordCount";
                s.Caption = grid.Label(300150); // Created items
                s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "DeactivatedRecordCount";
                s.Caption = grid.Label(300151); // Deactivated items
                s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "PMLastUpdated";
                s.Caption = grid.Label(300152); // PM Last Updated
                s.Width = 200;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.SortIndex = 0;
                s.SortOrder = ColumnSortOrder.Descending;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "NetSuiteUpdates";
                s.Caption = grid.Label(300153); // NetSuite updates
                s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "NSLastUpdated";
                s.Caption = grid.Label(300154); // NS Last updated
                s.Width = 200;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            return grid;
        }

        public static List<ProductDashboardInfo> GetProductDashboardItems()
        {
            return ProductDashboardManager.GetProductDashboardItems();
        }
    }
}