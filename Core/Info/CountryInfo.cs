using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class CountryInfo
	{
		public int CountryID { get; set; }
		public string ISOCode { get; set; }
		public string Name { get; set; }

		public CountryInfo(){}

		public CountryInfo(DataRow dr)
		{
			CountryID = Utils.FromDBValue<int>(dr["CountryID"]);
			ISOCode = Utils.FromDBValue<string>(dr["ISOCode"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
		}
	}
}
