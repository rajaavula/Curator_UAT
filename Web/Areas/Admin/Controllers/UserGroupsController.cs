using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class UserGroupsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult ListEdit()
		{
			UserGroupsListEdit vm = UserGroupsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult ListEdit(UserGroupsListEdit vm)
		{
			UserGroupsListEdit cached = GetModelFromCache<UserGroupsListEdit>(vm.PageID);
			UserGroupsHelper.UpdateModel(vm, cached, IsExporting);
			if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
			SaveModelToCache(vm);
			return View(vm);
		}

		[AjaxOnly]
		public ActionResult Detail(string pageID, int id)
		{
			UserGroupsListEdit vm = GetModelFromCache<UserGroupsListEdit>(pageID);
			UserGroupInfo info = UserGroupsHelper.GetUserGroupInfo(vm, id);
			return SendAsJson(info);
		}

		[AjaxOnly]
		public ActionResult Save(string pageID, int? userGroupID, UserGroupInfo info)
		{
			UserGroupsListEdit vm = GetModelFromCache<UserGroupsListEdit>(pageID);
			string error = UserGroupsHelper.Update(vm, userGroupID, info);
			return SendAsJson(error);
		}

		[AjaxOnly]
		public ActionResult Delete(string pageID, int id)
		{
			UserGroupsListEdit vm = GetModelFromCache<UserGroupsListEdit>(pageID);
			string error = UserGroupsHelper.Delete(vm, id);
			return SendAsJson(error);
		}
		public ActionResult GrdMainCallback(string pageID)
		{
			string name = CallbackID;
			UserGroupsListEdit vm = GetModelFromCache<UserGroupsListEdit>(pageID);
			vm.Grids[name].Data =UserGroupsHelper.GetData(vm);
			return PartialView("GridViewPartial", vm.Grids[name]);
		}


	}
}
