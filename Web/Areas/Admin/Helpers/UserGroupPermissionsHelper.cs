using System.Linq;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class UserGroupPermissionsHelper
	{
		public static UserGroupPermissionsList CreateModel()
		{
			UserGroupPermissionsList vm = new UserGroupPermissionsList();
			vm.UserGroups = UserGroupManager.GetUserGroups(vm.SI.User.CompanyID, vm.SI.UserGroup.UserGroupID);

			if (vm.UserGroups != null && vm.UserGroups.Count > 0)
			{
				vm.UserGroupID = vm.UserGroups[0].UserGroupID;
			}

			vm.AssignedUserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(vm.SI.User.CompanyID, vm.UserGroupID).OrderBy(x => x.Name).ToList();
			vm.AvailableUserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(vm.SI.User.CompanyID);
			vm.AvailableUserGroupPermissions = vm.AvailableUserGroupPermissions.FindAll(x => vm.AssignedUserGroupPermissions.Exists(y => y.Code == x.Code) == false).OrderBy(x=>x.Name).ToList();
			vm.UserObjectIDs = string.Join(",", from userObject in vm.AssignedUserGroupPermissions select userObject.UserObjectID);

			return vm;
		}

		public static void UpdateModel(UserGroupPermissionsList cached, UserGroupPermissionsList vm )
		{
			vm.UserGroups = cached.UserGroups;

			if (vm.IsSave)
			{
				UserGroupPermissionManager.UpdateUserGroupPermissions(vm.SI.User.CompanyID, vm.UserGroupID, vm.UserObjectIDs);
				if (vm.SI.User.UserGroupID == vm.UserGroupID)
				{
					vm.SI.UserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(vm.SI.User.CompanyID, vm.SI.User.UserGroupID);
				}
			}

			vm.AssignedUserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(vm.SI.User.CompanyID, vm.UserGroupID).OrderBy(x => x.Name).ToList();
			vm.AvailableUserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(vm.SI.User.CompanyID);
			vm.AvailableUserGroupPermissions = vm.AvailableUserGroupPermissions.FindAll(x => vm.AssignedUserGroupPermissions.Exists(y => y.Code == x.Code) == false).OrderBy(x => x.Name).ToList();
			vm.UserObjectIDs = string.Join(",", from userObject in vm.AssignedUserGroupPermissions select userObject.UserObjectID);

			vm.IsSave = false;
		}
	}
}