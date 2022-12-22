using System;

namespace LeadingEdge.Curator.Core
{
	public class ActivePage
	{
		public string ModelID { get; set; }
		public DateTime Timestamp { get; set; }

		public ActivePage(string modelID)
		{
			ModelID = modelID;
			Timestamp = DateTime.Now;
		}
	}
}
