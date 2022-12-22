using System;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
    public class ApiCredentialsController : AuthorizeController
    {
        # region ListEdit
        [HttpGet]
        public ActionResult ListEdit()
        {
            ApiCredentialsListEdit vm = ApiCredentialsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            ApiCredentialsListEdit vm = GetModelFromCache<ApiCredentialsListEdit>(pageID);
            ApiCredentialInfo info = ApiCredentialsHelper.GetApiCredentialInfo(vm, id);

            return SendAsJson(info);
        }

        [HttpGet]
        public ActionResult New()
        {
            ApiCredentialsListEdit vm = ApiCredentialsHelper.CreateModel();
            SaveModelToCache(vm);
            return View("ListEdit");
        }

        [HttpPost]
        public ActionResult ListEdit(ApiCredentialsListEdit vm)
        {
            ApiCredentialsListEdit cached = GetModelFromCache<ApiCredentialsListEdit>(vm.PageID);
            ApiCredentialsHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult Save(string pageID, ApiCredentialInfo info)
        {
            ApiCredentialsListEdit vm = GetModelFromCache<ApiCredentialsListEdit>(pageID);

            Exception ex = ApiCredentialsHelper.Save(vm, info);
            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            ApiCredentialsListEdit vm = GetModelFromCache<ApiCredentialsListEdit>(pageID);

            vm.Grids[name].Data = ApiCredentialsHelper.GetData();
            SaveModelToCache(vm);

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Delete(int id)
        {
            Exception ex = ApiCredentialsHelper.Delete(id);

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }
        #endregion
    }
}
