using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Core.Application;

namespace LeadingEdge.Curator.Web
{
	//[HandleError, CheckCache]
	public class BaseController : Controller
	{
		public SessionInfo SI
		{
			get
			{
				if (HttpContext == null || HttpContext.Session == null) return null;
				return (SessionInfo)HttpContext.Session["sSessionInfo"];
			}
			set
			{
				if (HttpContext == null || HttpContext.Session == null) return;
				HttpContext.Session["sSessionInfo"] = value;
			}
		}

		public string CallbackID { get { return Request.Params["DXCallbackName"]; } }

		public string CallbackArgument
		{
			get
			{
				string callbackArg = DevExpressHelper.CallbackArgument;
				if (String.IsNullOrEmpty(callbackArg)) return null;

				string[] args = callbackArg.Split(';');
				if (args.Length < 2) return null;

				return args[1];
			}
		}

		public string ExportedGridName
		{
			get
			{
				try
				{
					return Request.Form.AllKeys.ToList().Find(x => x.StartsWith("ExportButton.")).Split('.')[1];
				}
				catch (Exception)
				{
					return String.Empty;
				}
			}
		}

		public bool IsExporting { get { return PostedBy("ExportButton"); } }

		public bool IsPrinting { get { return PostedBy("PrintButton"); } }

		public string UserHistory { get; set; }

		public T GetModelFromCache<T>(string pageID) where T : class
		{
			return SI.Data.Get<T>(U.GetModelID(typeof(T).Name, pageID));
		}

		public object GetModelFromCache(string pageID, string modelname)
		{
			return SI.Data.Get(U.GetModelID(modelname, pageID));
		}
		public object GetModelFromCache(string modelID)
		{
			return SI.Data.Get(modelID);
		}

		public void SaveModelToCache(BaseModel vm)
		{
			SI.Data.Set(vm.ModelID, vm);
		}

		public bool PostedBy(string id)
		{
			try {
                var keys = Request.Form.AllKeys.ToList().OrderBy(s => s);
                var valid = Request.Form.AllKeys.ToList().Exists(x => x.StartsWith(id));
                return Request.Form.AllKeys.ToList().Exists(x => x.StartsWith(id));
            }
			catch (Exception) { return false; }
		}

		public bool IsPostBack
		{
			get { return System.Web.HttpContext.Current.Request.HttpMethod == "POST"; }
		}

		public string GetCustomCallbackArg()
		{
			List<string> values = GetCustomCallbackArgs(null);

			return values.Count > 0 ? values[0] : String.Empty;
		}

		public List<string> GetCustomCallbackArgs(string separator)
		{
			List<string> result = new List<string>();

			string cbArgs = DevExpressHelper.CallbackArgument;

			int cbIndex = cbArgs.IndexOf("CUSTOMCALLBACK", StringComparison.Ordinal);

			if (cbIndex < 0) return result;

			cbArgs = cbArgs.Substring(cbIndex, (cbArgs.Length - 1) - cbIndex);
			var cbValues = cbArgs.Split('|');
			if (cbValues.Length < 1) return result;

			if (string.IsNullOrEmpty(separator))
			{
				result.Add(cbValues[1]);

				return result;
			}

			result = Regex.Split(cbValues[1], separator).ToList();

			return result;
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (Request == null || Request.IsAjaxRequest())
			{
				base.OnAuthorization(filterContext);
				return;
			}

			var authentication = HttpContext.GetOwinContext().Authentication;

			try
			{
				// If we have a session then continue;
				if (SI != null) return; // jumps to finally section.

				// Try to find user ID from authentication cookie
				var userID = SecurityHelper.GetUserId(authentication);
				if (userID == null) return;

				// Try to find user in database
				var user = UserManager.GetLoggedInUser(userID.Value);
				if (user == null) throw new Exception("Authentication: Could not find user in database.");

				// Attempt to create session
				if (!App.CreateSession(user, out string error)) throw new Exception("Authentication: Could not create session.");

				// If there was an error creating the session
				if (!string.IsNullOrEmpty(error)) throw new Exception("Authentication: Error creating session");
			}
			catch (Exception ex)
			{
				Log.Error(ex);

				SecurityHelper.CookieLogout(authentication);
			}
			finally
			{
				base.OnAuthorization(filterContext);
            }
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				base.OnActionExecuted(filterContext);
				return;
			}

			HttpRequestBase request = filterContext.HttpContext.Request;

			if (request.IsAjaxRequest())
			{
				base.OnActionExecuted(filterContext);
				return;
			}

			#region Update cookie

			if (!IsPostBack && Request.Cookies != null && String.IsNullOrEmpty(App.CookieName) == false && SI != null)
			{
				try
				{
					HttpCookie cookie = Request.Cookies[App.CookieName];

					if (cookie != null)
					{
						cookie.Expires = DateTime.MaxValue;
						cookie.HttpOnly = true; // prevent any attacker from accessing the cookie with client side scripts

						if (HttpContext.Request.IsSecureConnection)
						{
							cookie.Secure = true; // prevent surf jacking
						}
						HttpContext.Response.Cookies.Add(cookie);
					}
				}
				catch
				{
					// No action
				}
			}

			#endregion

			// Do default behaviour
			base.OnActionExecuted(filterContext);
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.ExceptionHandled)
			{
				filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
				return;
			}

			Log.Error(filterContext.Exception);

			base.OnException(filterContext);
		}

		public ActionResult BaseGridViewCallback(string ModelName, string PageID)
		{
			// We can now use this for all the standard grids
			// Make sure you pass in the model name as the last paramater in the Setup.Grid call
			string name = CallbackID;

			var vm = (BaseModel)GetModelFromCache(PageID, ModelName);

			return PartialView("GridViewPartial", vm.Grids[name]);
		}

		public ActionResult ExportGridView(GridModel grid)
		{
			return ExportGridView(ExportType.Xlsx, grid);
		}

		public ActionResult ExportGridView(ExportType type, GridModel grid)
		{
			if (grid == null) return new EmptyResult();

			return ExportGridView(type, grid.Settings, grid.Data, null);
		}

		public ActionResult ExportGridView(ExportType type, GridViewSettings settings, object data, object options)
		{
			int rowCount = 0;

			if (data != null)
			{
				if (data.GetType() == typeof(DataTable))
				{
					rowCount = ((DataTable)data).Rows.Count;
				}
				else
				{
					ICollection collection = (data as ICollection);
					if (collection != null) rowCount = collection.Count;
				}
			}


			if (rowCount > App.MaxXlsExportRows) type = ExportType.Csv;

			string filename = string.Format("Export_{0:HHmmss}.{1}", DateTime.Now, type);

			ActionResult file = new EmptyResult();

			try
			{
				if (type == ExportType.Xls)
				{
					XlsExportOptionsEx exportOptions = new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG };

					file = GridViewExtension.ExportToXls(settings, data, filename, exportOptions);
				}

				if (type == ExportType.Xlsx)
				{
					XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG };

					file = GridViewExtension.ExportToXlsx(settings, data, filename, exportOptions);
				}

				if (type == ExportType.Csv)
				{
					/*
					CsvExportOptionsEx exportOptions = new CsvExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG };

					file = GridViewExtension.ExportToCsv(settings, data, filename, exportOptions);
					*/

					file = new CSVResult(filename, data);
				}

				if (type == ExportType.Pdf)
				{
					PdfExportOptions exportOptions = new PdfExportOptions();

					file = GridViewExtension.ExportToPdf(settings, data, filename, exportOptions);
				}

				if (type == ExportType.Rtf)
				{
					PdfExportOptions exportOptions = new PdfExportOptions();

					file = GridViewExtension.ExportToPdf(settings, data, filename, exportOptions);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return file;
		}

		public ActionResult KeepAlive(string id)
		{
			bool success = App.KeepAlive(SI, id);

			return SendAsJson(new { OK = success });
		}

		[Authorize, HttpGet]
		public ActionResult LabelEditMode()
		{
			SI.LabelEditMode = (!SI.LabelEditMode);
			return Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
		}

		[ValidateInput(false)]
		public ActionResult UpdateLabel(int placeHolderID, string labelText, string toolTipText)
		{
			LabelInfo info = App.Labels.Find(x => x.PlaceholderID == placeHolderID && x.LanguageID == SI.Language.LanguageID && x.CompanyID == SI.User.CompanyID);

			string error;

			if (info != null)
			{
				info.LabelText = labelText;
				info.ToolTipText = toolTipText;
				error = LabelManager.Save(info.LabelID, info);
			}
			else
			{
				info = new LabelInfo();
				info.CompanyID = SI.User.CompanyID;
				info.LanguageID = SI.Language.LanguageID;
				info.PlaceholderID = placeHolderID;
				info.LabelText = labelText;
				info.ToolTipText = toolTipText;
				error = LabelManager.Save(null, info);

				if (String.IsNullOrEmpty(error))
				{
					info = LabelManager.GetLabel(SI.User.CompanyID, placeHolderID, SI.Language.LanguageID);
					if (info != null) App.Labels.Add(info);
				}
			}

			return SendAsJson(error);
		}

		public ActionResult GetLabel(int placeholderID)
		{
			LabelInfo labelInfo = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

			return SendAsJson(labelInfo);
		}

		public ActionResult AddQuickLink(int id)
		{
			if (id < 1) return SendAsJson(new { OK = false });

			bool success = App.AddQuicklink(SI.User.CompanyID, SI.User.UserID, id);

			if (success) SI.Bookmarks = App.GetBookmarks(SI);

			return SendAsJson(new { OK = success });
		}

		public ActionResult RemoveQuickLink(int id)
		{
			if (id < 1) return SendAsJson(new { OK = false });

			bool success = App.DeleteQuicklink(SI.User.CompanyID, SI.User.UserID, id);

			if (success) SI.Bookmarks = App.GetBookmarks(SI);

			return SendAsJson(new { OK = success });
		}

		public ActionResult JsonResult(object data)
		{
			string strJson = string.Empty;
			if (data == null) return Json(strJson, JsonRequestBehavior.AllowGet);

			var dateTimeConverter = new IsoDateTimeConverter
			{
				DateTimeFormat = "yyyy-MM-ddTHH:mm:ss"
			};

			strJson = JsonConvert.SerializeObject(data, dateTimeConverter);
			return Json(strJson, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Error()
		{
			return View();
		}

		protected JsonResult SendAsJson(object data)
		{
			var jsonResult = Json(data, JsonRequestBehavior.AllowGet);

			jsonResult.MaxJsonLength = int.MaxValue;

			return jsonResult;
		}

		public ActionResult GridReset(string modelID)
		{
			BaseModel vm = (BaseModel)GetModelFromCache(modelID);
			foreach (KeyValuePair<string, GridModel> grid in vm.Grids)
			{
                List<string> keysToRemove = SI.Layout.Keys.Where(k => k.StartsWith(vm.Name + grid.Value.Name)).ToList();
                foreach (string key in keysToRemove)
                {
                    SI.Layout.Remove(key);
                }
            }

			return Content("OK");
		}
	}
}
