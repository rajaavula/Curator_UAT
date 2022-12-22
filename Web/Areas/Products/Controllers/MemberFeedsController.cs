using System;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Helpers;
using LeadingEdge.Curator.Web.Products.Models;

namespace LeadingEdge.Curator.Web.Areas.Products.Controllers
{
    public class MemberFeedsController : AuthorizeController
    {
             
        [HttpGet]
        public ActionResult ListEdit()
        {
            MemberFeeds vm = MemberFeedsHelper.CreateModel();
            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ListEdit(MemberFeeds vm)
        {
            MemberFeeds cached = GetModelFromCache<MemberFeeds>(vm.PageID);
            MemberFeedsHelper.UpdateModel(vm, cached); 
            SaveModelToCache(vm);
            return View(vm);
        } 

    }
}
