using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Orders
{
	public class OrdersAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Orders"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("Orders_edit", "Orders/{controller}/{action}/{id}", new { area = "Orders", action = "Edit" });
			context.MapRoute("Orders_new", "Orders/{controller}/New", new { area = "Orders", action = "Edit" });
			context.MapRoute("Orders_default", "Orders/{controller}/{action}", new { area = "Orders", action = "List" });
			context.MapRoute("Orders_short", "Orders/{controller}", new { area = "Orders", action = "List" });
		}
	}
}
