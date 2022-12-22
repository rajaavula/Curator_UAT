using System.Collections.Generic;

namespace LeadingEdge.Curator.Core
{
	public static class FraudStatuses
	{
		public static FraudStatus All = new FraudStatus(1, "All");
		public static FraudStatus Complete = new FraudStatus(2, "Complete");
		public static FraudStatus Incomplete = new FraudStatus(3, "Incomplete");

		public static List<FraudStatus> Statuses = new List<FraudStatus> { All, Complete, Incomplete };
	}
}