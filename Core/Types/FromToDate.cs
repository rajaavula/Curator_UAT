using System;

namespace LeadingEdge.Curator.Core
{
	public class FromToDate
	{
		public int FromToDateID {get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public FromToDate()
		{
		}

		public FromToDate(int fromToDateID, string description, DateTime fromDate, DateTime toDate)
		{
			FromToDateID = fromToDateID;
            Description = description;
            FromDate = fromDate;
            ToDate = toDate;
		}
	}
}