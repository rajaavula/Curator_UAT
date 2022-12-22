using System.Web.Mvc;
using LeadingEdge.Curator.Web.Admin.Models;
using LeadingEdge.Curator.Web.Helpers;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
	public class UserGroupFormsController : AuthorizeController
	{
		[HttpGet]
		public ActionResult List()
		{
			UserGroupFormsList vm = UserGroupFormsHelper.CreateModel();
			SaveModelToCache(vm);
			return View(vm);
		}

		[HttpPost]
		public ActionResult List(UserGroupFormsList vm)
		{
			UserGroupFormsList cached = GetModelFromCache<UserGroupFormsList>(vm.PageID);
			UserGroupFormsHelper.UpdateModel(cached, vm);
			SaveModelToCache(vm);
			return View(vm);
		}
	}
}
