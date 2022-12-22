using System.Collections.Generic;
using System.Linq;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class SessionsHelper
	{
		public static SessionsList CreateModel()
		{
			SessionsList vm = new SessionsList();
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(SessionsList vm, SessionsList cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			if (isExporting) return;
			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, SessionsList vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.3", vm.Name);
			grid.Settings.KeyFieldName = "SessionID";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Sessions", Action = "GrdMainCallback" };
						
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.DateEdit;
				s.FieldName = "SessionStart";
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
				s.FieldName = "CompanyName";
				s.Caption = grid.Label(200446); // Company name
				s.Width = 200;
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

			return grid;
		}

		public static List<SessionListInfo> GetData(SessionsList vm)
		{
			return (from o in App.Sessions select new SessionListInfo(o)).ToList();
		}
	}
}