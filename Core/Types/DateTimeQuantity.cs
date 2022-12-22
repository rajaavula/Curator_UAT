using System;

namespace LeadingEdge.Curator.Core
{
	public class DateTimeQuantity
	{
		public DateTime Date { get; set; }
		public decimal Quantity { get; set; }

		public DateTimeQuantity()
		{
		}

		public DateTimeQuantity(DateTime date, decimal quantity)
		{
			Date = date;
			Quantity = quantity;
		}
	}
}
