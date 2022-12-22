using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web
{
	public class ComboBoxModel
	{
		public ComboBoxSettings Settings { get; set; }
		public object Data { get; set; }
		public object Value { get; set; }

		public ComboBoxModel()
		{
			Settings = new ComboBoxSettings();
		}
	}
}