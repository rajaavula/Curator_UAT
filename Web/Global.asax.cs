using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Web.Infrastructure.DependencyInjection;
using StructureMap;
using Microsoft.Extensions.Configuration;
using LeadingEdge.Curator.Core.Configurations;

namespace LeadingEdge.Curator.Web
{
	public class MvcApplication : HttpApplication
	{
		private static IConfiguration Configuration { get; set; }

		private static StructureMapDependencyResolver StructureMapResolver { get; set; }

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

		}

		protected void Application_Start()
		{
			SetupConfiguration();

			SetupDependencyResolver();

			App.ApplicationPath = Server.MapPath("~");
			// Load database config
			App.LoadConfiguration();

			Log.Initialise(App.ApplicationPath, App.WebInstrumentationKey, App.PlatformName, "Leading Edge Curator");
			Log.Error(new Exception("Test exception on startup"));
			Log.Archive();

			App.PopulateGlobalLists();

			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			StructureMapResolver.CreateNestedContainer();

			var request = HttpContext.Current.Request;

			if (request.IsSecureConnection.Equals(false) && request.IsLocal.Equals(false))
			{
				Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + request.RawUrl);
			}
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			StructureMapResolver.DisposeNestedContainer();
		}

		protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			DevExpressHelper.Theme = App.Theme;		
			DevExpressHelper.GlobalThemeBaseColor = App.GlobalThemeBaseColor;		
		}

		protected void Session_OnStart(object sender, EventArgs e)
		{
        }

		protected void Session_End(object sender, EventArgs e)
		{
			SessionInfo SI = (SessionInfo)Session["sSessionInfo"];

			App.RemoveSession(SI);
		}

		private void SetupConfiguration()
		{
			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{WebEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
	}

		private void SetupDependencyResolver()
		{
			// Setup StructureMap container for .NET Framework MVC services
			var container = new Container();

			container.Configure(c =>
			{
				c.AddSettings<OpenIdConnectSettings>(Configuration.GetSection("OpenIdConnect"));

				c.AddRegistry(new WebRegistry(Configuration));
			});

			StructureMapResolver = new StructureMapDependencyResolver(container);
			DependencyResolver.SetResolver(StructureMapResolver);
		}
	}
}