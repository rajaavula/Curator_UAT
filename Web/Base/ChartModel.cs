using System.Web;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public class ChartModel
	{
		public SessionInfo SI
		{
			get { return (SessionInfo)HttpContext.Current.Session["sSessionInfo"]; }
			set { HttpContext.Current.Session["sSessionInfo"] = value; }
		}

		public ChartControlSettings Settings { get; set; }

		public string Name
		{
			get { return Settings.Name; }
			set { Settings.Name = value; }
		}

		public ChartModel()
		{
			Settings = new ChartControlSettings();
		}

		public string Label(int placeholderID)
		{
			var label = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

			return label.Format(SI.LabelEditMode);
		}
	}
}
