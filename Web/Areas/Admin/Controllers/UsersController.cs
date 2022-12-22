using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
    public class UsersController : AuthorizeController
    {
        #region Users List

        [HttpGet]
        public ActionResult List()
        {
            UsersList vm = UsersListHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult List(UsersList vm)
        {
            UsersList cached = GetModelFromCache<UsersList>(vm.PageID);
            UsersListHelper.UpdateModel(vm, cached, IsExporting);
            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);
            return View(vm);
        }

        [AjaxOnly]
        public ActionResult ListPreview(string pageID, int id)
        {
            UsersList cached = GetModelFromCache<UsersList>(pageID);
            object obj = UsersListHelper.GetPreviewData(cached, id);
            return SendAsJson(obj);
        }

        [AjaxOnly]
        public ActionResult Delete(string pageID, int id)
        {
            UsersList vm = GetModelFromCache<UsersList>(pageID);
            string error = UsersListHelper.Delete(vm, id);
            return SendAsJson(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            UsersList vm = GetModelFromCache<UsersList>(pageID);
            vm.Grids[name].Data = UsersListHelper.GetData(vm);
            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        #endregion

        #region Users Edit

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            UsersEdit vm = UsersEditHelper.CreateModel(id);

            if (TempData["Success"] != null)
            {
                vm.Success = (bool)TempData["Success"];
            }

            SaveModelToCache(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(UsersEdit vm)
        {
            vm.Regions = CheckBoxListExtension.GetSelectedValues<int>("Regions_CheckBoxList");
            UsersEdit cached = GetModelFromCache<UsersEdit>(vm.PageID);
            UsersEditHelper.UpdateModel(vm, cached);
            SaveModelToCache(vm);

            if (vm.Exception == null)
            {
                TempData["Success"] = true;
                return RedirectToAction("Edit", "Users", new { id = vm.UserID });
            }

            return View(vm);
        }

        #endregion
    }
}
