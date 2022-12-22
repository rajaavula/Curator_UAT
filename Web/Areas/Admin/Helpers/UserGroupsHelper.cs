using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using DevExpress.Web;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class UserGroupsHelper
	{
		public static UserGroupsListEdit CreateModel()
		{
			UserGroupsListEdit vm = new UserGroupsListEdit();
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));
			vm.Grids["GrdMain"].Data = GetData(vm);
			return vm;
		}

		public static void UpdateModel(UserGroupsListEdit vm, UserGroupsListEdit cached, bool isExporting)
		{
			vm.Grids = cached.Grids;

			if (isExporting) return;

			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		public static GridModel GetGridView(string name, bool exporting, UserGroupsListEdit vm)
		{
			GridModel grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.5", vm.Name);
			grid.Settings.KeyFieldName = "UserGroupID";
			grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "UserGroups", Action = "GrdMainCallback" };

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Name";
				s.Caption =  grid.Label(200342); // Name
				s.Width = 180;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "Description";
				s.Caption = grid.Label(200196); // Description
				s.Width = 280;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "IsOwner";
				s.Caption = grid.Label(200343); // Owner
				s.Width = 100;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.CheckBox;
				s.FieldName = "IsWorker";
				s.Caption = grid.Label(200808); // Worker
				s.Width = 100;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			return grid;
		}

		public static UserGroupInfo GetUserGroupInfo(UserGroupsListEdit vm, int userGroupID)
		{
			var data = (List<UserGroupInfo>)vm.Grids["GrdMain"].Data;
			
			return data.Find(x => x.UserGroupID == userGroupID);
		}

		public static string Update(UserGroupsListEdit vm, int? userGroupID, UserGroupInfo info)
		{
			info.CompanyID = vm.SI.User.CompanyID;

			Exception ex = UserGroupManager.Save(userGroupID, info);
			if (ex != null) return ex.Message;

			vm.Grids["GrdMain"].Data = UserGroupManager.GetUserGroups(info.CompanyID, vm.SI.UserGroup.UserGroupID);

			return String.Empty;
		}

		public static string Delete(UserGroupsListEdit vm, int userGroupID)
		{
			Exception ex = UserGroupManager.Delete(vm.SI.User.CompanyID, userGroupID);
			if (ex != null) return ex.Message;

			vm.Grids["GrdMain"].Data = UserGroupManager.GetUserGroups(vm.SI.User.CompanyID, vm.SI.UserGroup.UserGroupID);

			return String.Empty;
		}

		public static List<UserGroupInfo> GetData(UserGroupsListEdit vm)
		{
			return UserGroupManager.GetUserGroups(vm.SI.User.CompanyID, vm.SI.UserGroup.UserGroupID);
		}
	}
}
