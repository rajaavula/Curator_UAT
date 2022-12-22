using System;

namespace LeadingEdge.Curator.Core
{
	public class IdDescription
	{
		public Guid? Id { get; set; }
		public string Name { get; set; }

		public IdDescription()
		{
		}

		public IdDescription(Guid? id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}