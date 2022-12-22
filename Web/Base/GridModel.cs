using System.Web;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public class GridModel
	{
		public SessionInfo SI
		{
			get { return (SessionInfo)HttpContext.Current.Session["sSessionInfo"]; }
			set { HttpContext.Current.Session["sSessionInfo"] = value; }
		}

		public GridViewSettings Settings { get; set; }

		public string Name
		{
			get { return Settings.Name; }
			set { Settings.Name = value; }
		}

		public object Data { get; set; }

		public GridModel()
		{
			Settings = new GridViewSettings();
		}

		public string Label(int placeholderID)
		{
			var label = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

			// For grids we will capitalise the text
			var gridLabel = label.Clone();
			gridLabel.LabelText = gridLabel.LabelText.ToUpper();

			return gridLabel.Format(SI.LabelEditMode);
		}
	}
}
