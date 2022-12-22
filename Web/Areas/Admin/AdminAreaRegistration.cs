using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Admin"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("Admin_edit", "Admin/{controller}/{action}/{id}", new { area = "Admin", action = "Edit" });
			context.MapRoute("Admin_new", "Admin/{controller}/New", new { area = "Admin", action = "Edit" });
			context.MapRoute("Admin_default", "Admin/{controller}/{action}", new { area = "Admin", action = "List" });
			context.MapRoute("Admin_short", "Admin/{controller}", new { area = "Admin", action = "List" });
		}
	}
}
