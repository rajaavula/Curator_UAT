using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class CompanyRegionInfo
	{
		public int RegionID { get; set; }
		public int CompanyID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public string CopyRegionalEmailTo { get; set; }
		public string SalesSupportEmailAddress { get; set; }
		public string PurchasingDeptEmailAddress { get; set; }
		public string EmailServer { get; set; }
		public string EmailUsername { get; set; }
		public string EmailPassword { get; set; }
		public string EmailFromEmail { get; set; }
		public string EmailFromName { get; set; }

		public CompanyRegionInfo(){}

		public CompanyRegionInfo(DataRow dr)
		{
			RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Notes = Utils.FromDBValue<string>(dr["Notes"]);
			CopyRegionalEmailTo = Utils.FromDBValue<string>(dr["CopyRegionalEmailTo"]);
			SalesSupportEmailAddress = Utils.FromDBValue<string>(dr["SalesSupportEmailAddress"]);
			PurchasingDeptEmailAddress = Utils.FromDBValue<string>(dr["PurchasingDeptEmailAddress"]);
			EmailServer = Utils.FromDBValue<string>(dr["EmailServer"]);
			EmailUsername = Utils.FromDBValue<string>(dr["EmailUsername"]);
			EmailPassword = Utils.FromDBValue<string>(dr["EmailPassword"]);
			EmailFromEmail = Utils.FromDBValue<string>(dr["EmailFromEmail"]);
			EmailFromName = Utils.FromDBValue<string>(dr["EmailFromName"]);
		}
	}
}
