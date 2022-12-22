
using System;

namespace LeadingEdge.Curator.Core
{
	public class OpenSessionInfo
	{
		public string SessionID { get; set; }
		public DateTime SessionStart { get; set; }
		public UserInfo User { get; set; }
		public string IPAddress { get; set; }

		public string CompanyName
		{
			get
			{
				try
				{
					return App.Companies.Find(x => x.CompanyID == User.CompanyID).Name;
				}
				catch (Exception)
				{
					
					return "*** UNKNOWN ***";
				}
			}
		}

		public OpenSessionInfo(SessionInfo SI)
		{
			SessionID = SI.SessionID;
			SessionStart = DateTime.Now;
			User = SI.User;
			IPAddress = SI.IPAddress;
		}
	}
}
