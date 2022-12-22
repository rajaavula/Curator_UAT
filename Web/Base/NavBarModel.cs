using System.Web;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public class NavBarModel
	{
		public SessionInfo SI
		{
			get { return (SessionInfo)HttpContext.Current.Session["sSessionInfo"]; }
			set { HttpContext.Current.Session["sSessionInfo"] = value; }
		}

		public NavBarSettings Settings { get; set; }

		public string Name
		{
			get { return Settings.Name; }
			set { Settings.Name = value; }
		}

		public object Data { get; set; }

		public NavBarModel()
		{
			Settings = new NavBarSettings();
		}

		public string Label(int placeholderID)
		{
			var label = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

			return label.Format(SI.LabelEditMode);
		}
	}
}
