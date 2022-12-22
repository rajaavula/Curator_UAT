using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class EmailObjectInfo
	{
		public int EmailObjectID { get; set; }
		public int EmailID { get; set; }
		public string Name { get; set; }
		public byte[] Bytes { get; set; }
		public string ContentType { get; set; }
		public int RegionID { get; set; }
		public int CompanyID { get; set; }

		public EmailObjectInfo() { }

		public EmailObjectInfo(DataRow dr)
		{
			EmailObjectID = Utils.FromDBValue<int>(dr["EmailObjectID"]);
			EmailID = Utils.FromDBValue<int>(dr["EmailID"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Bytes = Utils.FromDBValue<byte[]>(dr["Bytes"]);
			ContentType = Utils.FromDBValue<string>(dr["ContentType"]);
			RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
		}
	}
}
