using System.Collections.Generic;
using System.Drawing;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class Status
	{
		public Status(int id, string name, string colourHTML)
		{
			ID = id;
			Name = name;
			ColourHTML = colourHTML;
			Colour = ColorTranslator.FromHtml(ColourHTML);
		}

		public int ID { get; private set; }
		public string Name { get; set; }
		public string ColourHTML { get; private set; }
		public Color Colour { get; private set; }
	}
}