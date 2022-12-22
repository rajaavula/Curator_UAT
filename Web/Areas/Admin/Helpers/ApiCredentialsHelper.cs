using System;
using System.Collections.Generic;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
    public static class ApiCredentialsHelper
    {
        public static ApiCredentialsListEdit CreateModel()
        {
            var vm = new ApiCredentialsListEdit();
            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = GetData();

            return vm;
        }

        public static void UpdateModel(ApiCredentialsListEdit vm, ApiCredentialsListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;
            if (isExporting) return;
            vm.Grids["GrdMain"].Data = ApiCredentialManager.GetSupplierUsers();
        }

        public static GridModel GetGridView(string name, bool exporting, ApiCredentialsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.1", vm.Name);
            grid.Settings.KeyFieldName = "UserKey";
            grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "ApiCredentials", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.RowDblClick = "function (s, e) { Edit(); } ";


            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "UserID";
                s.Caption = grid.Label(300098); // Object ID
                s.Width = 300;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "EntityID";
                s.Caption = grid.Label(300099); // Entity ID
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "DisplayName";
                s.Caption = grid.Label(300100); // Display name
                s.Width = 220;                
            });

            return grid;
        }

        public static Exception Save(ApiCredentialsListEdit vm, ApiCredentialInfo info)
        {
            Exception ex = ApiCredentialManager.Save(info);

            vm.Grids["GrdMain"].Data = GetData();

            return ex;
        }

        public static List<ApiCredentialInfo> GetData()
        {
            return ApiCredentialManager.GetSupplierUsers();
        }

        public static ApiCredentialInfo GetApiCredentialInfo(ApiCredentialsListEdit vm, int userKey)
        {
            var data = (List<ApiCredentialInfo>)vm.Grids["GrdMain"].Data;

            return data.Find(x => x.UserKey == userKey);
        }

        public static Exception Delete(int userKey)
        {
            Exception ex = ApiCredentialManager.Delete(userKey);

            return ex;
        }
    }
}
