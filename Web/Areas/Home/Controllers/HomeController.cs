using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Home.Helpers;
using LeadingEdge.Curator.Web.Home.Models;
using Microsoft.Owin.Security.Cookies;

namespace LeadingEdge.Curator.Web.Areas.Home.Controllers
{
    public class HomeController : BaseController
    {
        #region Forms Authentication

        /*
        [HttpGet]
        public ActionResult Login()
        {
            HomeLogin vm = LoginHelper.CreateModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Login(HomeLogin vm)
        {
            if (!LoginHelper.FormsLogin(vm)) return View(vm);

            if (!String.IsNullOrEmpty(vm.ReturnUrl))
            {
                return Redirect(vm.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SecurityHelper.Logout();
            if (SI != null && SI.User != null) SI.User = null;
            return RedirectToAction("Login", "Home");
        }
        */

        #endregion

        #region Cookies Authentication

        [HttpGet]
        public ActionResult Login()
        {
            // Redirect to default authorised page if authenticated
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            // Redirect to sign in provider if OpenId Connect enabled
            if (!App.BypassOpenIdConnect)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes().Select(o => o.AuthenticationType).ToArray());
                return new HttpUnauthorizedResult();
            }

            HomeLogin vm = LoginHelper.CreateModel(null);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Callback(string code)
        {
            HomeLogin vm = LoginHelper.CreateModel(code);
            return View("Login", vm);
        }

        [HttpPost]
        public ActionResult Login(HomeLogin vm)
        {
            if (!LoginHelper.CookieLogin(vm, HttpContext.GetOwinContext().Authentication)) return View(vm);

            if (!string.IsNullOrEmpty(vm.ReturnUrl))
            {
                return Redirect(vm.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        [HttpGet]
        public ActionResult Logout()
        {
            SecurityHelper.CookieLogout(HttpContext.GetOwinContext().Authentication);
            if (SI != null && SI.User != null) SI.User = null;

            if (App.BypassOpenIdConnect)
            {
                return RedirectToAction(nameof(Index));
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            HomeContact vm = HomeHelper.CreateContactModel();
            return View(vm);
        }

        [Authorize, HttpGet]
        public FileResult Logo(int? id)
        {
            int companyID = id.HasValue ? id.Value : SI.User.CompanyID;
            CompanyInfo company = App.Companies.Find(x => x.CompanyID == companyID);

            if (company != null && company.Logo != null && company.Logo.Length > 0)
            {
                return File(company.Logo, "image/png");
            }

            return File("/Assets/Img/empty.png", "image/png");
        }

        [Authorize, HttpGet]
        public ActionResult Index()
        {
            HomeIndex vm = HomeHelper.CreateModel();
            SaveModelToCache(vm);
            return View(vm);
        }

        [Authorize, HttpPost]
        public ActionResult Index(HomeIndex vm)
        {
            return View(vm);
        }

        [Authorize, AjaxOnly]
        public ActionResult Details()
        {
            HomeDetails vm = HomeHelper.CreateDetailModel();
            return View(vm);
        }

        [Authorize, AjaxOnly]
        public ActionResult UpdateDetails(HomeDetails vm)
        {
            string error = String.Empty;
            Exception ex = HomeHelper.UpdateDetails(vm);
            if (ex != null) error = ex.Message;
            return Content(error);
        }

        [Authorize, AjaxOnly]
        public ActionResult Regions()
        {
            HomeRegions vm = HomeHelper.CreateRegionsModel();
            return View(vm);
        }

        [Authorize, AjaxOnly]
        public ActionResult ChangeRegion(HomeRegions vm)
        {
            string error = String.Empty;
            Exception ex = HomeHelper.UpdateRegion(vm);
            if (ex != null) error = ex.Message;
            return Content(error);
        }

        [Authorize, AjaxOnly]
        public ActionResult Bookmarks()
        {
            HomeBookmarks vm = HomeHelper.CreateBookmarksModel();
            return View(vm);
        }

        [Authorize]
        public ActionResult Document(int companyID, int regionID, int id)
        {
            DocumentInfo doc = DocumentManager.GetDocument(companyID, regionID, id, true);
            string contentType = U.GetDocumentContentType(doc.Filename);

            return File(doc.Bytes, contentType, doc.Filename);
        }

        [Authorize, AjaxOnly]
        public ActionResult DeleteDocument(int companyID, int regionID, int id, int userID)
        {
            string error = String.Empty;
            Exception ex = DocumentManager.DeleteDocument(companyID, regionID, id, userID);
            if (ex != null) error = ex.Message;
            return Content(error);
        }

        [Authorize, AjaxOnly, NoCache]
        public ActionResult UpdateMenuExpanded(bool menuExpanded)
        {
            SI.MenuExpanded = menuExpanded;
            return Json(new { OK = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize, HttpGet]
        public ActionResult Help()
        {
            string path = Server.MapPath(String.Format("//Assets//Doc//Help.pdf"));
            if (!System.IO.File.Exists(path)) path = "/Assets/Doc/Help.pdf";
            return File(path, "application/pdf");
        }       
    }
}
