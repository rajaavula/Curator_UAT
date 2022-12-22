using System;
using System.Web.Mvc;
using System.Drawing;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Helpers;
using LeadingEdge.Curator.Web.Admin.Models;

namespace LeadingEdge.Curator.Web.Areas.Admin.Controllers
{
    public class StoresController : AuthorizeController
    {
        # region ListEdit

        [HttpGet]
        public ActionResult ListEdit()
        {
            StoreListEdit vm = StoresHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, int id)
        {
            StoreListEdit vm = GetModelFromCache<StoreListEdit>(pageID);
            StoreInfo info = StoresHelper.GetStoreInfo(vm, id);

            return SendAsJson(info);
        }

        [HttpGet]
        public ActionResult New()
        {
            StoreListEdit vm = StoresHelper.CreateModel();
            SaveModelToCache(vm);
            return View("ListEdit");
        }

        [HttpPost]
        public ActionResult ListEdit(StoreListEdit vm)
        {
            StoreListEdit cached = GetModelFromCache<StoreListEdit>(vm.PageID);
            StoresHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult Save(string pageID, StoreInfo info)
        {
            StoreListEdit vm = GetModelFromCache<StoreListEdit>(pageID);

            Exception ex = StoresHelper.Save(vm, info);
            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            StoreListEdit vm = GetModelFromCache<StoreListEdit>(pageID);

            vm.Grids[name].Data = StoresHelper.GetData();
            SaveModelToCache(vm);

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Delete(string pageID, int id)
        {
            StoreListEdit vm = GetModelFromCache<StoreListEdit>(pageID);
            Exception ex = StoresHelper.Delete(vm, id);

            string error = string.Empty;
            if (ex != null && ex.Message != null) error = ex.Message;

            return SendAsJson(error);
        }

        [HttpPost]
        public ActionResult StoreLogoUpload()
        {
            UploadControlValidationSettings validationSettings = StoresHelper.GetStoreLogoValidationSettings();
            UploadControlExtension.GetUploadedFiles("uplStoreLogo", validationSettings, StoresHelper.StoreLogo_FileUploadComplete);
            return null;
        }

        [HttpGet]
        public ActionResult StoreLogoPreview()
        {
            byte[] logoBytes = StoresHelper.StoreLogoPreview;

            if (logoBytes == null || logoBytes.Length < 1)
            {
                logoBytes = new byte[] { 0 };
            }
            else 
            {
                logoBytes = U.ResizeImage(logoBytes, new Size(285, 200));
            }

            return File(logoBytes, "image/jpg");
        }

        #endregion
    }
}
