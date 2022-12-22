using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cortex.Utilities;
using DevExpress.Data;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class FormEditorHelper
	{
		public static FormEditor CreateModel()
		{
			FormEditor vm = new FormEditor();
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(FormEditor vm, FormEditor cached, bool isExporting)
		{
			vm.Grids = cached.Grids;

			if (isExporting) return;

			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, FormEditor vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.2", vm.Name);
			grid.Settings.KeyFieldName = "FormID";
			grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { GrdMain_FocusedRowChanged(s,e); }";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Form", Action = "GrdMainCallback" };
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Group";
				s.Caption = grid.Label(200430); // Group
				s.Width = 150;
				s.SortOrder = ColumnSortOrder.Ascending;
				s.SortIndex = 0;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Name";
				s.Caption = grid.Label(200016); // Name
				s.Width = 250;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "DisplayOrder";
				s.Caption = grid.Label(200186); // Display order
				s.Width = 100;
				s.SortOrder = ColumnSortOrder.Ascending;
				s.SortIndex = 1;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Area";
				s.Caption = grid.Label(200429); // Area
				s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Controller";
				s.Caption = grid.Label(200486); // Controller
				s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Action";
				s.Caption = grid.Label(200487); // Action
				s.Width = 150;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Parameters";
				s.Caption = grid.Label(200488); // Parameters
				s.Width = 80;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "PlaceholderID";
				s.Caption = grid.Label(200331); // Placeholder ID
				s.Width = 100;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "OwnersOnly";
				s.Caption = grid.Label(200431); // Owner Only
				s.Width = 100;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			return grid;
		}

		public static FormInfo GetForm(FormEditor vm, int formID)
		{
			try
			{
				return ((List<FormInfo>)vm.Grids["GrdMain"].Data).Find(x => x.FormID == formID);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return null;
			}
		}

		public static string Update(FormEditor vm, int? formID, FormInfo info)
		{
			try
			{
				SqlParameter[] parameters = new SqlParameter[]
				{
					new SqlParameter("@FormID", Utils.ToDBValue(formID)),
					new SqlParameter("@Group", Utils.ToDBValue(info.Group)),
					new SqlParameter("@Area", Utils.ToDBValue(info.Area)),
					new SqlParameter("@Controller", Utils.ToDBValue(info.Controller)),
					new SqlParameter("@Action", Utils.ToDBValue(info.Action)),
					new SqlParameter("@Parameters", Utils.ToDBValue(info.Parameters)),
					new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
					new SqlParameter("@PlaceHolderID", Utils.ToDBValue(info.PlaceholderID)),
					new SqlParameter("@DisplayOrder", Utils.ToDBValue(info.DisplayOrder)),
					new SqlParameter("@Notes", Utils.ToDBValue(info.Notes)),
					new SqlParameter("@OwnersOnly", Utils.ToDBValue(info.OwnersOnly))
				};

				DB db = new DB(App.CuratorDBConn);

				db.QuerySP("SaveForm", parameters);

				if (!db.Success) throw db.DBException;

				vm.SI.PermittedForms = FormManager.GetPermittedForms(vm.SI.User.CompanyID, vm.SI.User.UserGroupID);

				vm.Grids["GrdMain"].Data = FormManager.GetForms();
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return ex.Message;
			}

			return String.Empty;
		}

		public static string Delete(FormEditor vm, int formID)
		{
			try
			{
				SqlParameter[] parameters = new[] { new SqlParameter("@FormID", Utils.ToDBValue(formID)) };
				DB db = new DB(App.CuratorDBConn);
				db.QuerySP("DeleteForm", parameters);

				if (db.Success == false) throw db.DBException;

				vm.SI.PermittedForms = FormManager.GetPermittedForms(vm.SI.User.CompanyID, vm.SI.User.UserGroupID);

				vm.Grids["GrdMain"].Data = FormManager.GetForms();
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return ex.Message;
			}

			return String.Empty;
		}

        public static List<FormInfo> GetData(FormEditor vm)
        {
            return FormManager.GetForms();
        }
    }
}
