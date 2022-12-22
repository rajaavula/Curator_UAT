using System.Collections.Generic;

namespace LeadingEdge.Curator.Core
{
	public static class OrderStatuses
	{
		public static OrderStatus AwaitingFulfillment = new OrderStatus(1, "Awaiting fulfillment");
		public static OrderStatus PartiallyShipped = new OrderStatus(2, "Partially shipped");
		public static OrderStatus SentToSupplier = new OrderStatus(3, "Sent to supplier");
		public static OrderStatus Fulfilled = new OrderStatus(4, "Fulfilled");
		public static OrderStatus Cancelled = new OrderStatus(5, "Cancelled");
		public static OrderStatus Backordered = new OrderStatus(6, "Backordered");

		public static List<OrderStatus> Statuses = new List<OrderStatus> { AwaitingFulfillment, PartiallyShipped, SentToSupplier, Fulfilled, Cancelled, Backordered };
	}
}