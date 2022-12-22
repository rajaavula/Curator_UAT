using System.Web.Mvc;

namespace LeadingEdge.Curator.Web.Areas.Products
{
    public class ProductsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Products"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Products_default", "Products/{controller}/{action}", new { area = "Products", action = "List" });
            context.MapRoute("Products_short", "Products/{controller}", new { area = "Products", action = "List" });
        }
    }
}
