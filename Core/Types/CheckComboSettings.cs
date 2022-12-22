using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Core
{
    public class CheckComboSettings
	{
		public DropDownEditSettings DropDownSettings { get; set; }
		public ListBoxSettings ListBoxSettings { get; set; }

		public CheckComboSettings()
		{
			DropDownSettings = new DropDownEditSettings();
			ListBoxSettings = new ListBoxSettings();
		}
	}
}
