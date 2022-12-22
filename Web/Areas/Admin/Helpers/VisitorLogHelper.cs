using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class VisitorLogHelper
	{
		public static VisitorLogList CreateModel()
		{
			VisitorLogList vm = new VisitorLogList();
			vm.FromDate = DateTime.Today.AddMonths(-3);
			vm.ToDate = DateTime.Today;
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(VisitorLogList vm, VisitorLogList cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			if (isExporting) return;
			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, VisitorLogList vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.2", vm.Name);
			grid.Settings.KeyFieldName = "VisitorLogID";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "VisitorLog", Action = "GrdMainCallback" };

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.DateEdit;
				s.FieldName = "DateTime";
				s.Caption = grid.Label(200197); // Session start
				s.Width = 150;
				s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
				s.SortIndex = 0;
				s.SortOrder = ColumnSortOrder.Descending;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Name";
				s.Caption = grid.Label(200016); // Name
				s.Width = 200;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Position";
				s.Caption = grid.Label(200198); // Position
				s.Width = 200;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "UserGroup";
				s.Caption = grid.Label(200202); // User group
				s.Width = 200;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Region";
				s.Caption = grid.Label(200021); //Region
				s.Width = 200;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.DateEdit;
				s.FieldName = "SessionEnd";
				s.Caption = grid.Label(200480); //Session end
				s.Width = 150;
				s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			return grid;
		}

		public static List<VisitorLogInfo> GetData(VisitorLogList vm)
		{
			return VisitorLogManager.GetVisitorLogs(vm.SI.User.CompanyID, vm.FromDate, vm.ToDate, vm.SI.UserGroup.IsOwner);
		}
	}
}