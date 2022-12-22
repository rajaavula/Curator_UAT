using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class SiteInfo
	{
		public int SiteID { get; set; }
		public int CompanyID { get; set; }
		public int RegionID { get; set; }
		public DateTime CreatedDate { get; set; }
		public int CreatedByID { get; set; }
		public string CreatedByName { get; set; }
		public DateTime ModifiedDate { get; set; }
		public int ModifiedByID { get; set; }
		public string ModifiedByName { get; set; }
		public string Name { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Suburb { get; set; }
		public string City { get; set; }
		public string AreaCode { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }

		public string FormatedAddress {
			get {
				string address = Address1;
				if (!String.IsNullOrEmpty(Address2)) address = address + ", " + Address2;
				if (!String.IsNullOrEmpty(Suburb)) address = address + ", " + Suburb;
				if (!String.IsNullOrEmpty(City)) address = address + ", " + City;
				if (!String.IsNullOrEmpty(AreaCode)) address = address + ", " + AreaCode;
				return address;
			}
		}

		public SiteInfo()
		{
		}

		public SiteInfo(DataRow dr)
		{
			SiteID			= Utils.FromDBValue<int>(dr["SiteID"]);
			CompanyID		= Utils.FromDBValue<int>(dr["CompanyID"]);
			RegionID		= Utils.FromDBValue<int>(dr["RegionID"]);
			CreatedByID		= Utils.FromDBValue<int>(dr["CreatedByID"]);
			ModifiedByID	= Utils.FromDBValue<int>(dr["ModifiedByID"]);
			Name			= Utils.FromDBValue<string>(dr["Name"]);
			Address1		= Utils.FromDBValue<string>(dr["Address1"]);
			Address2		= Utils.FromDBValue<string>(dr["Address2"]);
			Suburb			= Utils.FromDBValue<string>(dr["Suburb"]);
			City			= Utils.FromDBValue<string>(dr["City"]);
			AreaCode		= Utils.FromDBValue<string>(dr["AreaCode"]);
			ModifiedByName	= Utils.FromDBValue<string>(dr["ModifiedByName"]);
			CreatedByName	= Utils.FromDBValue<string>(dr["CreatedByName"]);
			Latitude		= Utils.FromDBValue<double?>(dr["Latitude"]);
			Longitude		= Utils.FromDBValue<double?>(dr["Longitude"]);
			ModifiedDate	= Utils.FromDBValue<DateTime>(dr["ModifiedDate"]);
			CreatedDate		= Utils.FromDBValue<DateTime>(dr["CreatedDate"]);
		}
	}
}
