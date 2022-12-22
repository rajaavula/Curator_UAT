using System.Web.Mvc;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class UserGroupPermissionsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult List()
		{
			UserGroupPermissionsList vm = UserGroupPermissionsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult List(UserGroupPermissionsList vm)
		{
			UserGroupPermissionsList cached = GetModelFromCache<UserGroupPermissionsList>(vm.PageID);
			UserGroupPermissionsHelper.UpdateModel(cached, vm);
			SaveModelToCache(vm);
			return View(vm);
		}
	}
}
