using System.Web;
using System.Web.Mvc;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web
{
	public class BaseMasterPage<T> : ViewMasterPage<T> where T : BaseModel
	{
		public SessionInfo SI
		{
			get { return (SessionInfo)HttpContext.Current.Session["sSessionInfo"]; }
			set { HttpContext.Current.Session["sSessionInfo"] = value; }
		}
	}
}