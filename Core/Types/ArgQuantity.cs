using System;

namespace LeadingEdge.Curator.Core
{
	public class ArgQuantity
	{
		public string Arg { get; set; }
		public Decimal Quantity { get; set; }

		public ArgQuantity()
		{
		}

		public ArgQuantity(string arg, Decimal quantity)
		{
			Arg = arg;
			Quantity = quantity;
		}

	}
}
