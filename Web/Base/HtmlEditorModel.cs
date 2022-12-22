using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public class HtmlEditorModel
	{
		public HtmlEditorSettings Settings { get; set; }

		public HtmlEditorModel()
		{
			Settings = new HtmlEditorSettings();
		}
	}
}
