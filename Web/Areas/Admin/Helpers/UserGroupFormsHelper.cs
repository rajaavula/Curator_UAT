using System.Linq;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Helpers
{
	public static class UserGroupFormsHelper
	{
		public static UserGroupFormsList CreateModel()
		{
			UserGroupFormsList vm = new UserGroupFormsList();
			vm.UserGroups = UserGroupManager.GetUserGroups(vm.SI.User.CompanyID, vm.SI.UserGroup.UserGroupID);

			if (vm.UserGroups != null && vm.UserGroups.Count > 0)
			{
				vm.UserGroupID = vm.UserGroups[0].UserGroupID;
			}

			vm.AssignedForms = FormManager.GetPermittedForms(vm.SI.User.CompanyID, vm.UserGroupID);
			vm.AssignedForms = vm.AssignedForms.FindAll(x => x.OwnersOnly == false);
			vm.AssignedForms = vm.AssignedForms.OrderBy(x => x.Area).ThenBy(x => x.Group).ThenBy(x => x.DisplayOrder).ToList();
			
			vm.AvailableForms = FormManager.GetForms(false, false);
			vm.AvailableForms = vm.AvailableForms.FindAll(x => x.OwnersOnly == false && vm.AssignedForms.Exists(y => y.FormID == x.FormID) == false);
			vm.AvailableForms = vm.AvailableForms.OrderBy(x => x.Area).ThenBy(x => x.Group).ThenBy(x => x.DisplayOrder).ToList();

			vm.FormIDs = string.Join(",", from form in vm.AssignedForms select form.FormID);

			return vm;
		}

		public static void UpdateModel(UserGroupFormsList cached, UserGroupFormsList vm)
		{
			vm.UserGroups = cached.UserGroups;

			if (vm.IsSave)
			{
				FormManager.UpdatePermittedForms(vm.UserGroupID, vm.SI.User.CompanyID, vm.FormIDs);
				if (vm.SI.User.UserGroupID == vm.UserGroupID)
				{
					vm.SI.PermittedForms = FormManager.GetPermittedForms(vm.SI.User.CompanyID, vm.SI.User.UserGroupID);
				}
			}

			vm.AssignedForms = FormManager.GetPermittedForms(vm.SI.User.CompanyID, vm.UserGroupID);
			vm.AssignedForms = vm.AssignedForms.FindAll(x => x.OwnersOnly == false);
			vm.AssignedForms = vm.AssignedForms.OrderBy(x => x.Area).ThenBy(x => x.Group).ThenBy(x => x.DisplayOrder).ToList();

			vm.AvailableForms = FormManager.GetForms(false, false);
			vm.AvailableForms = vm.AvailableForms.FindAll(x => x.OwnersOnly == false && vm.AssignedForms.Exists(y => y.FormID == x.FormID) == false);
			vm.AvailableForms = vm.AvailableForms.OrderBy(x => x.Area).ThenBy(x => x.Group).ThenBy(x => x.DisplayOrder).ToList();

			vm.FormIDs = string.Join(",", from form in vm.AssignedForms select form.FormID);

			vm.IsSave = false;
		}
	}
}