using System;
using System.Collections.Generic;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web;
using System.Drawing;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
    public static class StoresHelper
    {
        public static byte[] StoreLogoPreview
        {
            get { return (byte[])HttpContext.Current.Session["sStoreLogoPreview"]; }
            set { HttpContext.Current.Session["sStoreLogoPreview"] = value; }
        }

        public static StoreListEdit CreateModel()
        {
            StoreListEdit vm = new StoreListEdit();

            vm.StoreLogoUploadControlSettings = GetStoreLogoUploadSettings();

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            vm.Stores = StoreManager.GetStores();
            vm.Grids["GrdMain"].Data = vm.Stores;

            return vm;
        }

        public static void UpdateModel(StoreListEdit vm, StoreListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;

            vm.StoreLogoUploadControlSettings = cached.StoreLogoUploadControlSettings;
            vm.Logo = StoreLogoPreview;

            if (isExporting) return;
            vm.Grids["GrdMain"].Data = StoreManager.GetStores();
        }

        public static GridModel GetGridView(string name, bool exporting, StoreListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.83", vm.Name);
            grid.Settings.KeyFieldName = "StoreID";
            grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Stores", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.RowDblClick = "function (s, e) { Edit(); } ";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreName";
                s.Caption = grid.Label(201025); // Name
                s.Width = 180;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "ShopifyID";
                s.Caption = grid.Label(201005); // Shopify ID
                s.Width = 120;
                s.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreUrl";
                s.Caption = grid.Label(300067); // Store URL
                s.Width = 220;                
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreApiKey";
                s.Caption = grid.Label(300066); // Store API key
                s.Width = 250;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StorePassword";
                s.Caption = grid.Label(300068); // Store password
                s.Width = 280;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreSharedSecret";
                s.Caption = grid.Label(300069); // Store shared secret
                s.Width = 280;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "EnableAutomaticNetSuiteUpdate";
                s.Caption = grid.Label(300183); // Automatic NetSuite updates
                s.Width = 230;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "DoNotUpdateRRP";
                s.Caption = grid.Label(300207); // Do not update RRP
                s.Width = 180;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "DoNotUpdateCostPrice";
                s.Caption = grid.Label(300208); // Do not update cost price
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "DoNotUpdateInventory";
                s.Caption = grid.Label(300209); // Do not update inventory
                s.Width = 200;
            });

            return grid;
        }

        public static UploadControlSettings GetStoreLogoUploadSettings()
        {
            UploadControlSettings s = new UploadControlSettings();
            Setup.UploadControl(s, "uplStoreLogo");
            s.CallbackRouteValues = new { area = "Admin", controller = "Stores", action = "StoreLogoUpload" };
            s.ClientSideEvents.FileUploadComplete = "function(s,e) { LogoUploadComplete(s,e); }";

            UploadControlValidationSettings vs = GetStoreLogoValidationSettings();
            s.ValidationSettings.Assign(vs);

            return s;
        }

        public static UploadControlValidationSettings GetStoreLogoValidationSettings()
        {
            UploadControlValidationSettings s = new UploadControlValidationSettings();
            s.AllowedFileExtensions = new[] { ".png", ".jpg" };
            s.MaxFileSize = 20971520;
            return s;
        }

        public static void StoreLogo_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (!e.UploadedFile.IsValid) return;

            StoreLogoPreview = e.UploadedFile.FileBytes;
        }

        public static Exception Save(StoreListEdit vm, StoreInfo info)
        {
            info.Logo = StoreLogoPreview;
            Exception ex = StoreManager.Save(info);

            vm.Stores = GetData();
            vm.Grids["GrdMain"].Data = vm.Stores;

            return ex;
        }

        public static List<StoreInfo> GetData()
        {
            return StoreManager.GetStores();
        }

        public static StoreInfo GetStoreInfo(StoreListEdit vm, int StoreID)
        {
            var data = (List<StoreInfo>)vm.Grids["GrdMain"].Data;

            StoreInfo store = data.Find(x => x.StoreID == StoreID);

            StoreLogoPreview = store.Logo;

            return store;
        }

        public static Exception Delete(StoreListEdit vm, int storeId)
        {
            Exception ex = StoreManager.DeleteStore(storeId);
            if (ex == null) vm.Grids["GrdMain"].Data = GetData();

            return ex;
        }
    }
}
