using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class App
	{
		public static string Theme = "MetropolisBlue";
		public static string GlobalThemeBaseColor = "#DC1E35";
		public static string Version = "1.2.51";

		public static int IncrementalFilteringDelay = 500;
		public static int CallbackPageSize = 200;
		public static int SessionTimeoutMinutes = 30;
		public static string URLRegex = @"http(s) ?://([\w-]+.)+[\w-]+(/[\w- ./?%&=@]*)?";
		public const string EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
		public const string EmailListRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*";
		public const string PasswordRegex = @"^(?=.*[a-zA-Z])(?=.*\d).{8,}$";
		public static Encoding TextEncoding = Encoding.Default;
		public static string ApplicationPath { get; set; }
		public static string TimeZone = "New Zealand Standard Time";
		public const string ErrorPrefix = "LEADING_EDGE_ERROR:";
		public const string PlatformName = "Leading Edge Curator";
		public const string DefaultDatabaseServer = "products-db.database.windows.net";
		public const string DefaultCuratorDatabase = "Curator";
		public const string DefaultProductsDBDatabase = "ProductsDB_UAT";
		public const string DefaultOrdersDBDatabase = "ProductsDB_UAT";
		public const string DefaultSuppliersDBDatabase = "SuppliersDB";
		public const string DefaultApplicationUrl = "https://web-curator-uat.azurewebsites.net";
		public const string CookieName = "Cookie_LEG_Curator2";
		public const int MaxXlsExportRows = 20000;
		public const int MaxProductsRows = 10000;
        

		public static string WebInstrumentationKey { get; set; }
		public static string CuratorDBConn { get; set; }
		public static string ProductsDBConn { get; set; }
		public static string OrdersDBConn { get; set; }
		public static string SuppliersDBConn { get; set; }
		public static string ApplicationUrl { get; set; }
		public static bool IsLive { get; set; }
		public static string ErrorEmailRecipient { get; set; }
		public static bool BypassOpenIdConnect { get; set; }

		public static List<CompanyInfo> Companies { get; set; }
		public static List<CodeDescription> ExportTypes { get; set; }
		public static List<LanguageInfo> Languages { get; set; }
		public static List<LabelInfo> Labels { get; set; }
		public static List<OpenSessionInfo> Sessions { get; set; }

		public static bool LoadConfiguration()
		{
			// Load config
			try
			{
				string filename = String.Format("{0}Configuration.xml", ApplicationPath);
				Configuration c = new Configuration();

				if (!File.Exists(filename))
				{
					c.DatabaseServer = DefaultDatabaseServer;
					c.CuratorDatabase = DefaultCuratorDatabase;
					c.ProductsDBDatabase = DefaultProductsDBDatabase;
					c.OrdersDBDatabase = DefaultOrdersDBDatabase;
					c.SuppliersDBDatabase = DefaultSuppliersDBDatabase;
					c.ApplicationUrl = DefaultApplicationUrl;
					c.IsLive = false;
					c.WebInstrumentationKey = "12053118-fc09-4ad5-af29-d7eba7298406";
					c.ErrorEmailRecipient = "support@cortex.co.nz";

					Utils.Save(c, filename);
				}

				if (!File.Exists(filename)) return false;

				string xml = File.ReadAllText(filename, TextEncoding);
				c = (Configuration)Utils.DeserializeObject(xml, typeof(Configuration));

				CuratorDBConn = string.Format("Server={0};Database={1};Uid=IntegrationsUser;Pwd=8m3B1taF#PX27E8z;", c.DatabaseServer, c.CuratorDatabase);
				ProductsDBConn = string.Format("Server={0};Database={1};Uid=IntegrationsUser;Pwd=8m3B1taF#PX27E8z;", c.DatabaseServer, c.ProductsDBDatabase);
				OrdersDBConn= string.Format("Server={0};Database={1};Uid=IntegrationsUser;Pwd=8m3B1taF#PX27E8z;", c.DatabaseServer, c.OrdersDBDatabase);
				SuppliersDBConn = string.Format("Server={0};Database={1};Uid=IntegrationsUser;Pwd=8m3B1taF#PX27E8z;", c.DatabaseServer, c.SuppliersDBDatabase);
				ApplicationUrl = c.ApplicationUrl;
				IsLive = c.IsLive;
				WebInstrumentationKey = c.WebInstrumentationKey;
				ErrorEmailRecipient = c.ErrorEmailRecipient;

				return true;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return false;
			}
		}

		public static bool CreateSession(UserInfo info, out string error)
		{
			error = String.Empty;
			try
			{
				var SI = new SessionInfo();
				SI.Data.ClearAll();

				// Session ID
				SI.SessionID = HttpContext.Current.Session.SessionID;

				// IP
				SI.IPAddress = HttpContext.Current.Request.UserHostAddress;

				// User info
				if (info == null)
				{
					int userID = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
					info = UserManager.GetUser(null, userID);
				}

				SI.User = info;
				if (SI.User == null) throw new Exception("Unable to load UserInfo");

				// Language
				SI.Language = Languages.Find(x => x.LanguageID == SI.User.LanguageID);
				SI.Culture = new CultureInfo(SI.Language.Culture);

				// Company info
				CompanyInfo company = Companies.Find(x => x.CompanyID == SI.User.CompanyID);
				if (company == null)
				{
					error = "This company cannot use this platform";
					return false;
				}

				if (!company.Live)
				{
					error = "This company has been disabled.<br/>Please contact support";
					return false;
				}

				// Company regions
				SI.UserRegions = CompanyRegionManager.GetCompanyRegionsByUser(SI.User.CompanyID, SI.User.UserID);
				if (SI.UserRegions == null) throw new Exception("Unable to load CompanyRegionInfo list");
				if (SI.UserRegions.Count == 0)
				{
					error = "You do not belong to any regions.<br/>Please contact support.";
					return false;
				}

				// Usergroup
				SI.UserGroup = UserGroupManager.GetUserGroup(SI.User.CompanyID, SI.User.UserGroupID, SI.User.UserGroupID);
				if (SI.UserGroup == null) throw new Exception("Unable to load UserGroupInfo list");

				// Permissions
				SI.PermittedForms = FormManager.GetPermittedForms(SI.User.CompanyID, SI.User.UserGroupID);
				SI.UserGroupPermissions = UserGroupPermissionManager.GetUserGroupPermissions(SI.User.CompanyID, SI.User.UserGroupID);

				// Grid layouts
				SI.Layout = LayoutManager.GetGridLayouts(SI.User.CompanyID, SI.User.RegionID, SI.User.UserID);

				// Menu
				SI.MenuExpanded = true;

				// Check session limit
				if (SI.Company.RestrictSessions && !SI.UserGroup.IsOwner && !U.HasPermission(SI, "IGNORESESSION"))
				{
					int currentSessions = Sessions.Count(x => x.User.CompanyID == SI.User.CompanyID);
					if (currentSessions >= SI.Company.MaximumSessions)
					{
						error = "You have exceeded the maximum amount of licenced sessions.<br/>Please try again later.";
						return false;
					}
				}
				
				// Bookmarks
				SI.Bookmarks = GetBookmarks(SI);

				// Test if any company labels and try and load them if none
				if (!Labels.Exists(x => x.CompanyID == SI.User.CompanyID)) LabelManager.ReloadClientLabels(SI.User.CompanyID);

				// Try and repopulate from cookie
				HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
				if (cookie == null)	// Create a new one
				{
					cookie = new HttpCookie(CookieName);

					cookie.Expires = DateTime.MaxValue;
					cookie.HttpOnly = true; // prevent any attacker from accessing the cookie with client side scripts

					if (HttpContext.Current.Request.IsSecureConnection)
					{
						cookie.Secure = true; // prevent surf jacking
					}

					HttpContext.Current.Response.Cookies.Add(cookie);
				}

				HttpContext.Current.Session["sSessionInfo"] = SI;

				// Add to sessions collection for licencing
				if (!SI.UserGroup.IsOwner && !U.HasPermission(SI, "IGNORESESSION")) Sessions.Add(new OpenSessionInfo(SI));

				// Finally add a visitorlog entry
				VisitorLogManager.SaveVisitorLog(SI);

				return true;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				error = "There was a problem creating your session.<br/>Please contact support.";
				return false;
			}
		}

		public static string GetModelID(string modelname, string pageID)
		{
			return String.Format("{0}_{1}", modelname, pageID);
		}

		public static LabelInfo GetLabel(int companyID, string languageID, int placeholderID)
		{
			try
			{
				// Try cache for current company and language
				LabelInfo label = Labels.Find(x => x.CompanyID == companyID && x.PlaceholderID == placeholderID && x.LanguageID == languageID);
				if (label != null) return label;

				// Try default for the language
				label = Labels.Find(x => x.CompanyID == 0 && x.PlaceholderID == placeholderID && x.LanguageID == languageID);
				if (label != null) return label;

				// Try default in ENGLISH
				label = Labels.Find(x => x.CompanyID == 0 && x.PlaceholderID == placeholderID && x.LanguageID == "ENGLISH");
				if (label != null) return label;

				throw new Exception("Label not found");
			}
			catch
			{
				return new LabelInfo { LabelText = LabelInfo.MissingLabelText, Error = true };
			}
		}

		public static LabelInfo GetDefaultLabel(int placeholderID)
		{
			return GetLabel(0, "ENGLISH", placeholderID);
		}

		public static bool KeepAlive(SessionInfo SI, string modelID)
		{
			try
			{
				HttpContext.Current.Session["Heartbeat"] = DateTime.Now;

				if (SI == null || SI.ActivePages == null || String.IsNullOrEmpty(modelID))
				{
					// throw new Exception("Keep alive failed - No session exists");
					return false;
				}

				// Add/update this page version in our list
				var activepage = SI.ActivePages.Find(x => x.ModelID == modelID);
				if (activepage == null)
				{
					activepage = new ActivePage(modelID);
					SI.ActivePages.Add(activepage);
				}
				else
				{
					activepage.Timestamp = DateTime.Now;
				}

				// Tidy up expired session data
				foreach (var page in SI.ActivePages.FindAll(x => x.Timestamp < DateTime.Now.AddMinutes(-SessionTimeoutMinutes)))
				{
					SI.Data.Remove(page.ModelID);
					SI.ActivePages.Remove(page);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return false;
			}

			return true;
		}

		public static List<FormInfo> GetBookmarks(SessionInfo SI)
		{
			var db = new DB(CuratorDBConn);
			var paramsArray = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(SI.User.CompanyID)),
			    new SqlParameter("@UserID", Utils.ToDBValue(SI.User.UserID))
			};

			var dt = db.QuerySP("GetQuicklinks", paramsArray);

			List<FormInfo> currentQuicklinks = (from DataRow dr in dt.Rows select new FormInfo(dr)).OrderBy(x => x.Area).ThenBy(x => x.Name).ToList();

			var quicklinks = new List<FormInfo>();
			foreach (FormInfo form in currentQuicklinks)
			{
				if (SI.PermittedForms.Exists(x => x.FormID == form.FormID))
				{
					quicklinks.Add(form);
				}
				else
				{
					DeleteQuicklink(SI.User.CompanyID, SI.User.UserID, form.FormID);
				}
			}

			return quicklinks;
		}

		public static bool AddQuicklink(int companyID, int userID, int formID)
		{
			var db = new DB(CuratorDBConn);
			var paramsArray = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@FormID", Utils.ToDBValue(formID))
			};

			db.QuerySP("AddQuicklink", paramsArray);

			return db.Success;
		}

		public static bool DeleteQuicklink(int companyID, int userID, int formID)
		{
			var db = new DB(CuratorDBConn);
			var paramsArray = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@FormID", Utils.ToDBValue(formID))
			};

			db.QuerySP("DeleteQuicklink", paramsArray);

			return db.Success;
		}

		public static void PopulateGlobalLists()
		{
			Companies = CompanyManager.GetCompanies(true);
			Languages = LanguageManager.GetLanguages();
			Labels = LabelManager.GetLabels(0, "DEFAULT"); // Default labels
			Sessions = new List<OpenSessionInfo>();
		
			DefineReportExportTypes();
		}
				

		public static void DefineReportExportTypes()
		{
			ExportTypes = new List<CodeDescription>();
			ExportTypes.Add(new CodeDescription("Pdf", "PDF"));
			ExportTypes.Add(new CodeDescription("Xlsx", "Excel"));
			ExportTypes.Add(new CodeDescription("Docx", "Word"));
			ExportTypes.Add(new CodeDescription("Image", "Image"));
		}

		public static string GetBaseUrl()
		{
			var request = HttpContext.Current.Request;
			var appUrl = HttpRuntime.AppDomainAppVirtualPath;

			if (appUrl != "/") appUrl += "/";

			var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

			return baseUrl;
		}
		public static void RemoveSession(SessionInfo SI)
		{
			if (Sessions != null)
			{
				Sessions.RemoveAll(x => x == null);
			}
			else
			{
				Sessions = new List<OpenSessionInfo>();
			}

			if (SI != null)
			{
				if (SI.User != null)
				{
					LayoutManager.SaveGridLayout(SI.User.CompanyID, SI.User.RegionID, SI.User.UserID, SI.Layout);
				}

				Sessions.RemoveAll(x => x.SessionID == SI.SessionID);
				VisitorLogManager.EndSession(SI.VisitorLogID);
			}
		}
	}
}
