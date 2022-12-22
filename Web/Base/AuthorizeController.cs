using System.Web;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web
{
	[Authorize]
	public class AuthorizeController : BaseController
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				base.OnActionExecuting(filterContext);
				return;
			}

			var authentication = HttpContext.GetOwinContext().Authentication;

			if (SecurityHelper.AuthenticateCookieRequest(authentication) == false)
			{
				SecurityHelper.CookieLogout(authentication);
			}

			base.OnActionExecuting(filterContext);
		}
	}
}