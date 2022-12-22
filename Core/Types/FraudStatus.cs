namespace LeadingEdge.Curator.Core
{
	public class FraudStatus
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public FraudStatus()
		{
		}

		public FraudStatus(int id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}