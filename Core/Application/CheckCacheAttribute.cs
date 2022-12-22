using System;
using System.Web;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Core
{
	public class CheckCacheAttribute : ActionFilterAttribute
	{
		public SessionInfo SI
		{
			get
			{
				HttpContext context = HttpContext.Current;
				if (context == null || context.Session == null) return null;
				return (SessionInfo)context.Session["sSessionInfo"];
			}
			set
			{
				HttpContext context = HttpContext.Current;
				if (context == null || context.Session == null) return;
				context.Session["sSessionInfo"] = value;
			}
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
			{
				base.OnActionExecuted(filterContext);
				return;
			}

			HttpRequestBase request = filterContext.HttpContext.Request;

			if (request.RequestType != "POST" || request.IsAjaxRequest())
			{
				base.OnActionExecuted(filterContext);
				return;
			}

			if (filterContext.Exception == null || filterContext.ExceptionHandled)
			{
				base.OnActionExecuted(filterContext);
				return;
			}

			string pageID = Convert.ToString(request.Form["PageID"]);

			if (SI == null || SI.Data == null || SI.Data.ContainsLike(pageID) == false)
			{
				filterContext.ExceptionHandled = true;
			}

			base.OnActionExecuted(filterContext);
		}
	}
}
