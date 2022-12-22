namespace LeadingEdge.Curator.Core
{
	public class OrderStatus
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public OrderStatus()
		{
		}

		public OrderStatus(int id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}