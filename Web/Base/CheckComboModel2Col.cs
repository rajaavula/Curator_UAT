using System.Collections.Generic;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Web
{
	public class CheckComboModel2Col
	{
		public string Name { get; set; }
		public string Text { get; set; }
		public int Width { get; set; }
		public bool ShowValue { get; set; }
		public List<CodeDescription> Data { get; set; }

		public CheckComboModel2Col()
		{
			Data = new List<CodeDescription>();
			ShowValue = true;
		}
	}
}