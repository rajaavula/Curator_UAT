using Cortex.Utilities;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class CategoriesHelper
    {
        public static CategoriesListEdit CreateModel()
        {
            var vm = new CategoriesListEdit();

            vm.Categories = GetData();
            vm.ParentCategoryModel = GetParentCategoryComboBoxSettings(vm);
            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = vm.Categories;

            return vm;
        }

        public static void UpdateModel(CategoriesListEdit vm, CategoriesListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;
            vm.Categories = cached.Categories;
            vm.ParentCategoryModel = cached.ParentCategoryModel;

            if (isExporting) return;

            vm.Categories = GetData();
            vm.Grids["GrdMain"].Data = vm.Categories;
        }

        private static GridModel GetGridView(string name, bool exporting, CategoriesListEdit vm)
        {
            var grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.3", vm.Name);
            grid.Settings.KeyFieldName = "CategoryKey";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "Categories", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.EndCallback = "function(s,e) { GrdMain_EndCallback(s, e); }";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Name";
                s.Caption = grid.Label(200967); // Category name
                s.Width = 220;
                s.SortIndex = 0;
                s.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Description";
                s.Caption = grid.Label(200948); // Category description
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "Level";
                s.Caption = grid.Label(200996); // Level
                s.Width = 80;
                s.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Level1CategoryName";
                s.Caption = grid.Label(200708); // Level 1 Category name
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Level2CategoryName";
                s.Caption = grid.Label(200982); // Level 2 Category name
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Level3CategoryName";
                s.Caption = grid.Label(200984); // Level 3 Category name
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Level4CategoryName";
                s.Caption = grid.Label(200994); // Level 4 Category name
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Level5CategoryName";
                s.Caption = grid.Label(300018); // Level 5 Category name
                s.Width = 220;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "CreatedDate";
                s.Caption = grid.Label(200651); // Created date
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.Width = 220;              
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "ModifiedDate";
                s.Caption = grid.Label(200987); // Modified date
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.Width = 220;
            });

            return grid;
        }

        public static ComboBoxModel GetParentCategoryComboBoxSettings(CategoriesListEdit vm)
        {
            ComboBoxModel model = new ComboBoxModel();
            model.Settings = new ComboBoxSettings();

            var data = (from x in vm.Categories.OrderBy(x => x.Description) select new ValueDescription(x.CategoryKey, x.CategoryPath)).ToList();     // Note sorted by category desc not category path
            data.Insert(0, new ValueDescription(null, null));   // Add null option

            model.Data = data;
            model.Value = null;

            var s = model.Settings;
            Setup.ComboBox(s);

            s.Name = "ParentKey";
            s.Properties.ValueField = "Value";
            s.Properties.ValueType = typeof(int?);
            s.Properties.TextField = "Description";
            s.Properties.AllowNull = true;
            s.Properties.CallbackPageSize = 100000;
            s.CallbackRouteValues = new { Area = "Products", Controller = "Categories", Action = "ParentCategoryCallback" };
            s.Properties.ClientSideEvents.BeginCallback = "function(s,e) { ParentCategory_BeginCallback(s,e); }";
            s.Properties.ClientSideEvents.EndCallback = "function(s,e) { ParentCategory_EndCallback(s,e); }";

            return model;
        }

        public static Exception Save(CategoriesListEdit vm, int? categoryKey, string name, string description, int? parentKey)
        {
            var info = new CategoryInfo();
            if (categoryKey.HasValue) info.CategoryKey = categoryKey.Value;
            info.Name = name;
            info.Description = description;
            info.ParentKey = parentKey;

            Exception ex = CategoryManager.Save(categoryKey, info);

            vm.Categories = GetData();
            vm.Grids["GrdMain"].Data = vm.Categories;

            return ex;
        }

        public static Exception Delete(CategoriesListEdit vm, int categoryKey)
        {
            Exception ex = CategoryManager.Delete(categoryKey);
            if (ex == null) vm.Grids["GrdMain"].Data = GetData();

            return ex;
        }

        public static List<CategoryInfo> GetData()
        {
            return CategoryManager.GetCategories().OrderBy(x => x.CategoryPath).ToList();
        }

        public static CategoryInfo GetCategoryInfo(CategoriesListEdit vm, int categoryKey)
        {
            var data = (List<CategoryInfo>)vm.Grids["GrdMain"].Data;

            return data.Find(x => x.CategoryKey == categoryKey);
        }


    }
}