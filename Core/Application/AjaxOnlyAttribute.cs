using System.Web;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Core
{
	public class AjaxOnlyAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!filterContext.HttpContext.Request.IsAjaxRequest())
			{
				throw new HttpException(404, "Action Not Found");
			}
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext){}
	}
}
