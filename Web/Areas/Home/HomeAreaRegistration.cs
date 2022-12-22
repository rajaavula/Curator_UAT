using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Home
{
	public class HomeAreaRegistration : AreaRegistration
	{
		public override string AreaName => "Home";

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("Error", "Error", new { area = AreaName, controller = "Home", action = "Error" });
			context.MapRoute("Login", "Login", new { area = AreaName, controller = "Home", action = "Login" });
			context.MapRoute("Callback", "Callback/{code}", new { area = AreaName, controller = "Home", action = "Callback", code = UrlParameter.Optional });
			context.MapRoute("Logout", "Logout", new { area = AreaName, controller = "Home", action = "Logout" });
			context.MapRoute("Captcha", "Captcha", new { area = AreaName, controller = "Home", action = "Captcha" });
			context.MapRoute("Document", "Document/{companyID}/{regionID}/{id}", new { area = AreaName, controller = "Home", action = "Document" });
			context.MapRoute("DeleteDocument", "DeleteDocument/{companyID}/{regionID}/{id}/{userID}", new { area = AreaName, controller = "Home", action = "DeleteDocument" });
			context.MapRoute("Contact", "Contact", new { area = AreaName, controller = "Home", action = "Contact" });
			context.MapRoute("Help", "Help", new { area = AreaName, controller = "Home", action = "Help" });
			context.MapRoute("Home", "Home", new { area = AreaName, controller = "Home", action = "Index" });
			context.MapRoute("Home_Default", "Home/{controller}/{action}/{id}", new { area = AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional });
			context.MapRoute("Default", "", new { area = AreaName, controller = "Home", action = "Index" });
		}
	}
}
