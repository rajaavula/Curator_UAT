using System;

namespace LeadingEdge.Curator.Core
{
	public class Month
	{
		public Month(DateTime d)
		{
			Start = new DateTime(d.Year, d.Month, 1);
		}

		public DateTime Start { get; set; }

		public DateTime End { get { return Start.AddMonths(1).AddDays(-1); } }

		public string Description
		{
			get { return string.Format("{0:MMM yyyy}", Start); }
		}

		public string MonthOnlyDescription
		{
			get { return string.Format("{0:MMMM}", Start); }
		}
	}
}
