using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class CompanyInfo
	{
		public int CompanyID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Live { get; set; }
		public string ApplicationTitle { get; set; }
		public string Theme { get; set; }
		public string CopyEmailTo { get; set; }
		public string SecurityAdminEmail { get; set; }
		public byte[] Logo { get; set; }
		public bool HasLogo { get; set; }
		public int MaximumSessions { get; set; }
		public bool RestrictSessions { get; set; }

		// Used for the Create Company popup
		public string OwnerName { get; set; }
		public string OwnerLogin { get; set; }
		public string OwnerEmail { get; set; }
		public string OwnerPassword { get; set; }

		public CompanyInfo() { }

		public CompanyInfo(DataRow dr)
		{
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			Name = Utils.FromDBValue<string>(dr["Name"]);
			Notes = Utils.FromDBValue<string>(dr["Notes"]);
			Live = Utils.FromDBValue<bool>(dr["Live"]);
			ApplicationTitle = Utils.FromDBValue<string>(dr["ApplicationTitle"]);
			Theme = Utils.FromDBValue<string>(dr["Theme"]);
			CopyEmailTo = Utils.FromDBValue<string>(dr["CopyEmailTo"]);
			SecurityAdminEmail = Utils.FromDBValue<string>(dr["SecurityAdminEmail"]);
			MaximumSessions = Utils.FromDBValue<int>(dr["MaximumSessions"]);
			RestrictSessions = Utils.FromDBValue<bool>(dr["RestrictSessions"]);
			Logo = Utils.FromDBValue<byte[]>(dr["Logo"]);
			HasLogo = Utils.FromDBValue<bool>(dr["HasLogo"]);
		}
	}
}
