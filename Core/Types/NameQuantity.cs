using System;

namespace LeadingEdge.Curator.Core
{
	public class NameQuantity
	{
		public string Name { get; set; }
		public Decimal Quantity { get; set; }

		public NameQuantity()
		{
		}

		public NameQuantity(string name, Decimal quantity)
		{
			Name = name;
			Quantity = quantity;
		}

	}
}
