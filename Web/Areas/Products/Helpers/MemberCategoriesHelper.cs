using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
    public static class MemberCategoriesHelper
    {
        public static MemberCategories CreateModel()
        {
            MemberCategories vm = new MemberCategories();
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

            vm.Categories = GetData(vm.StoreID);

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = vm.Categories;

            return vm;
        }

        public static void UpdateModel(MemberCategories vm, MemberCategories cached, bool isExporting)
        {
            if (isExporting) {
                cached.Grids["GrdMain"].Settings.Columns["ExportButtonColumn"].Visible = false;
                cached.Grids["GrdMain"].Settings.Columns["ExportButtonColumn"].Caption = "";
                return;
            }
            else { 
                vm.Grids["GrdMain"] = GetGridView("GrdMain", false, vm);
                vm.Grids["GrdMain"].Data = GetData(vm.StoreID);
                return;
            }
        }

        public static GridModel GetGridView(string name, bool exporting, MemberCategories vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.20", vm.Name);
            grid.Settings.KeyFieldName = "CategoryKey";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "MemberCategories", Action = "GrdMainCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            grid.Settings.CommandColumn.VisibleIndex = 1;
            grid.Settings.CommandColumn.Caption = "";

            grid.Settings.ClientSideEvents.BeginCallback = "function (s,e) { GrdMain_BeginCallback(s, e, '" + vm.Name + "'); }";
            grid.Settings.ClientSideEvents.EndCallback = "function (s,e) { GrdMain_EndCallback(s, true); }";

            var template = new StringBuilder();
            template.Append("<table cellpadding='0' cellspacing='0'><tr><td>");
            template.AppendFormat("<input type='submit' name='ExportButton.{0}' title='Export to Excel' class=\"far fa-file-excel command-column-button\" value=\"&#xf1c3\"/>", grid.Settings.Name);
            template.Append("</td><td style='padding-left: 6px;'>");
            template.AppendFormat("<i class=\"far fa-history command-column-button\" alt='Reset' title='Reset grid layout' onclick=\"ResetGrid('{0}');\" />", grid.Settings.SettingsCookies.CookiesID);
            template.Append("</td></tr></table>");
            vm.ExportButtonString = template.ToString();

            grid.Settings.CustomJSProperties = (sender, e) =>
            {
                MVCxGridView g = sender as MVCxGridView;
                var selected = new List<int>();

                for (var i = 0; i < g.VisibleRowCount; i++)
                {
                    int id = (int)g.GetRowValues(i, g.KeyFieldName);
                    bool assigned = (bool)g.GetRowValues(i, "MemberSelected");

                    if (!assigned) continue; 

                    selected.Add(id);
                }

                e.Properties["cpSelected"] = selected;
            };
            
            grid.Settings.PreRender = (sender, e) =>
            {
                MVCxGridView gridView = sender as MVCxGridView;
                var selected = vm.Categories.FindAll(x => x.MemberSelected);
                foreach (var category in selected)
                {
                    gridView.Selection.SelectRowByKey(category.CategoryKey);
                }
            };

            grid.Settings.Columns.Add(s =>
            {
                s.Name = "ExportButtonColumn";
                s.Caption = vm.ExportButtonString;
                s.Width = 75;
                s.VisibleIndex = 0;
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            });

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
                s.ColumnType = MVCxGridViewColumnType.TextBox;
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
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "MemberSelected";
                s.Caption = grid.Label(201000); // Is selected?
                s.Width = 120;
            });

            return grid;
        }

        public static void SetExportCommandButton(MemberCategories cached) 
        {
            cached.Grids["GrdMain"].Settings.Columns["ExportButtonColumn"].Visible = true;
            cached.Grids["GrdMain"].Settings.Columns["ExportButtonColumn"].Caption = cached.ExportButtonString;
        }

        public static List<CategoryInfo> GetData(int storeID)
        {
            return CategoryManager.GetMemberCategories(storeID);
        }

        public static Exception UpdateMemberCategories(MemberCategories vm, string allKeys, string categoryKeys)
        {
            if (vm.StoreID == 0) return new Exception("You must have a store ID");

            return CategoryManager.UpdateMemberCategories(vm.StoreID, allKeys, categoryKeys);
        }

        public static List<StoreInfo> GetMemberStores(MemberCategories vm)
        {
            return ProductManager.GetMemberStoreList(vm.SI.User.UserID).OrderBy(x => x.StoreName).ToList();
        }
    }
}