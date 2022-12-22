using System.Web;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public partial class _Default : System.Web.UI.Page
	{
		public void Page_Load(object sender, System.EventArgs e)
		{
			string originalPath = Request.Path;
			HttpContext.Current.RewritePath(Request.ApplicationPath, false);
			IHttpHandler httpHandler = new MvcHttpHandler();
			httpHandler.ProcessRequest(HttpContext.Current);
			HttpContext.Current.RewritePath(originalPath, false);
		}
	}
}