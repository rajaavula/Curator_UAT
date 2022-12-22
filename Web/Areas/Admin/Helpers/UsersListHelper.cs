using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public class UsersListHelper
	{
		public static UsersList CreateModel()
		{
			UsersList vm = new UsersList();
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(UsersList vm, UsersList cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			if (isExporting) return;
			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, UsersList vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.3", vm.Name);
			grid.Settings.KeyFieldName = "UserID";
            grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Users", Action = "GrdMainCallback" };
			grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
			grid.Settings.ClientSideEvents.RowDblClick = "function (s, e) { Edit(); } ";
			grid.Settings.ClientSideEvents.EndCallback = "function (s, e) { Get(); }";//to retain values below the grid

			// Set the default filter on the grid to only show Enabled users
			grid.Settings.PreRender = (sender, e) => 
            {
                MVCxGridView gv = (MVCxGridView)sender;
                gv.FilterExpression = "[Enabled] == 'true'";                         
            };

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "UserGroupName";
				s.Caption = grid.Label(200202); // User Group
				s.Width = 200;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Name";
				s.Caption = grid.Label(200016); // Name
				s.Width = 200;
				s.SortIndex = 0;
				s.SortOrder = ColumnSortOrder.Ascending;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Email";
				s.Caption = grid.Label(200022); // Email
				s.Width = 300;
            });
						
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "SalesRep";
				s.Caption = grid.Label(200660); // Sales rep?
				s.Width = 100;
            });
			
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "Enabled";
				s.Caption = grid.Label(200398); // Account Enabled?
				s.Width = 100;
            });
			
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Position";
				s.Caption = grid.Label(200198); // Position
				s.Width = 200;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Mobile";
				s.Caption = grid.Label(200105); // Mobile
				s.Width = 150;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Telephone";
				s.Caption = grid.Label(200102); // Telephone
				s.Width = 150;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "NewProductNotifications";
				s.Caption = grid.Label(300093); // New product notifications
				s.Width = 100;
            });
			
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "ChangedProductNotifications";
				s.Caption = grid.Label(300094); // Changed product notifications
				s.Width = 100;
            });
			
			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "DeactivatedProductNotifications";
				s.Caption = grid.Label(300095); // Deactivated product notifications
				s.Width = 100;
            });
			
			return grid;
		}

		public static object GetPreviewData(UsersList vm, int id)
		{
			try
			{
				var info = ((List<UserInfo>)vm.Grids["GrdMain"].Data).Find(x => x.UserID == id);

				return new
				{
					Name = info.Name ?? String.Empty,
					Login = info.Login ?? String.Empty,
					Telephone = info.Telephone ?? string.Empty,
					Fax = info.Fax ?? String.Empty,
					info.Enabled
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return null;
		}

		public static string Delete(UsersList vm, int id)
		{
			Exception ex = UserManager.Delete(vm.SI.User.CompanyID, id);
			if (ex != null) return ex.Message;

			vm.Grids["GrdMain"].Data = GetData(vm);
			return String.Empty;
		}

		public static List<UserInfo> GetData(UsersList vm)
		{
			return UserManager.GetUsers(vm.SI.User.CompanyID, null, vm.SI.UserGroup.UserGroupID, null, null, "General");
		}
	}
}