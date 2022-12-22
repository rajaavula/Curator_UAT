using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class CompanyRegionsHelper
	{
		public static CompanyRegionsListEdit CreateModel()
		{
			var vm = new CompanyRegionsListEdit();
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(CompanyRegionsListEdit vm, CompanyRegionsListEdit cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			if (isExporting) return;
			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, CompanyRegionsListEdit vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.4", vm.Name);
			grid.Settings.KeyFieldName = "RegionID";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "CompanyRegions", Action = "GrdMainCallback" };            
			grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Name";
				s.Caption = grid.Label(200016); // Name
				s.Width = 220;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "CopyRegionalEmailTo";
				s.Caption = grid.Label(200467); // Copy Email To
				s.Width = 300;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });


			return grid;
		}

		public static CompanyRegionInfo GetCompanyRegionInfo(CompanyRegionsListEdit vm, int regionID)
		{
			var data = (List<CompanyRegionInfo>)vm.Grids["GrdMain"].Data;

			return data.Find(x => x.RegionID == regionID);
		}

		public static string Update(CompanyRegionsListEdit vm, int? regionID, CompanyRegionInfo info)
		{
			info.CompanyID = vm.SI.User.CompanyID;
			Exception ex = CompanyRegionManager.Save(regionID, info);
			if (ex != null) return ex.Message;

			vm.Grids["GrdMain"].Data = CompanyRegionManager.GetCompanyRegions(info.CompanyID);

			if (vm.SI.UserRegions.Exists(x => x.RegionID == regionID)) vm.SI.UserRegions = CompanyRegionManager.GetCompanyRegionsByUser(vm.SI.User.CompanyID, vm.SI.User.UserID);


			return String.Empty;
		}

		public static string Delete(CompanyRegionsListEdit vm, int regionID)
		{
			if (vm.SI.CurrentRegion.RegionID == regionID) return "You cannot delete the region you are currently in.";

			Exception ex = CompanyRegionManager.Delete(vm.SI.User.CompanyID, regionID);

			if (ex != null) return ex.Message;

			vm.Grids["GrdMain"].Data = GetData(vm);

			return String.Empty;
		}
		public static List<CompanyRegionInfo> GetData(CompanyRegionsListEdit vm)
		{
			return CompanyRegionManager.GetCompanyRegions(vm.SI.User.CompanyID);
		}
	}
}