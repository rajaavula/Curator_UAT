using System;
using System.Collections.Generic;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
    public static class CompanyListHelper
    {
        public static CompanyList CreateModel()
        {
            CompanyList vm = new CompanyList();
            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
            vm.Grids["GrdMain"].Data = CompanyManager.GetCompanies();
            vm.NewCompanyPopupSettings = GetNewCompanyPopupSettings(vm);
            return vm;
        }

        public static void UpdateModel(CompanyList vm, CompanyList cached, bool isExporting)
        {
            vm.Grids = cached.Grids;
            if (isExporting) return;
            vm.Grids["GrdMain"].Data = CompanyManager.GetCompanies();

            vm.NewCompanyPopupSettings = cached.NewCompanyPopupSettings;
        }

        public static GridModel GetGridView(string name, bool exporting, CompanyList vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.8", vm.Name);
            grid.Settings.KeyFieldName = "CompanyID";
            grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Companies", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.RowDblClick = "function (s, e) { Edit(); } ";
            grid.Settings.ClientSideEvents.EndCallback = "function (s, e) { Get(); }";//to retain values below the grid

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "Name";
                s.Caption = grid.Label(200020); // Name
                s.Width = 180;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "HasLogo";
                s.Caption = grid.Label(200444); // Has Logo?
                s.Width = 110;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Settings.ShowFooter = true;

            string preview = U.RenderPartialToString("~/Areas/Admin/Views/Companies/ListPreview.ascx", grid);

            grid.Settings.SetFooterRowTemplateContent(preview);

            return grid;
        }

        public static object GetPreviewData(CompanyList vm, int id)
        {
            try
            {
                var info = ((List<CompanyInfo>)vm.Grids["GrdMain"].Data).Find(x => x.CompanyID == id);

                return new
                {
                    info.Name,
                    info.Live,
                    info.Theme,
                    info.Notes
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static PopupControlSettings GetNewCompanyPopupSettings(CompanyList vm)
        {
            PopupControlSettings s = new PopupControlSettings();
            Setup.PopupControl(s);
            s.Name = "ppNewCompany";
            s.HeaderText = vm.LabelText(300007); // New Company
            s.Width = 450;
            s.Modal = true;
            s.CallbackRouteValues = new { area = "Admin", controller = "Companies", action = "ListNew", pageID = vm.PageID };
            s.LoadContentViaCallback = LoadContentViaCallback.None;
            s.ClientSideEvents.BeginCallback = "function(s,e) { ppNewCompany_BeginCallback(s,e); }";
            s.ClientSideEvents.EndCallback = "function(s,e) { ppNewCompany_EndCallback(s,e); }";
            return s;
        }

        public static Exception CreateCompany(CompanyList vm, CompanyInfo info)
        {
            Exception ex = CompanyManager.CreateCompany(info);
            if (ex != null) return ex;

            App.Companies = CompanyManager.GetCompanies(true);
            vm.Grids["GrdMain"].Data = App.Companies;

            return null;
        }
        public static List<CompanyInfo> GetData(CompanyList vm)
        {
            return CompanyManager.GetCompanies();
        }
    }
}
