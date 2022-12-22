using System.Collections.Generic;
using DevExpress.Web;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web
{
	public class CheckComboModel
	{
		public List<ListEditItem> Data { get; set; }
		public CheckComboSettings Settings { get; set; }
		public CheckComboModel()
		{
			Data = new List<ListEditItem>();
			Settings = new CheckComboSettings();
		}
	}
}