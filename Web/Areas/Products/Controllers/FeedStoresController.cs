using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class FeedStoresController : AuthorizeController
    {
        [HttpGet]
        public ActionResult ListEdit()
        {
            FeedStoresListEdit vm = FeedStoresHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(FeedStoresListEdit vm)
        {
            FeedStoresListEdit cached = GetModelFromCache<FeedStoresListEdit>(vm.PageID);
            FeedStoresHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        public ActionResult GrdMainCallback(string pageID)
        {
            string name = CallbackID;
            FeedStoresListEdit vm = GetModelFromCache<FeedStoresListEdit>(pageID);

            var args = GetCustomCallbackArgs("~");
            if (args.Count == 1)
            {
                bool refresh = bool.Parse(args[0]);
                if (refresh)
                {
                    vm.Grids[name].Data = FeedStoresHelper.GetData();
                    SaveModelToCache(vm);
                }
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult Update(string pageID, FeedStoreInfo info)
        {
            var cached = GetModelFromCache<FeedStoresListEdit>(pageID);

            var error = FeedStoresHelper.Save(cached, info);

            return Content(error);
        }

        [AjaxOnly]
        public ActionResult Detail(string pageID, string id)
        {
            var cached = GetModelFromCache<FeedStoresListEdit>(pageID);

            var detail = FeedStoresHelper.GetDetail(cached, id);

            var result = detail.RemoveSecrets();

            return SendAsJson(result);
        }

        [AjaxOnly]
        public ActionResult Credentials(string pageID, string id)
        {
            var cached = GetModelFromCache<FeedStoresListEdit>(pageID);

            var detail = FeedStoresHelper.GetCredentials(cached, id);

            var result = detail.RemoveSecrets();

            return SendAsJson(result);
        }
    }
}