using System;

namespace LeadingEdge.Curator.Core
{
	public class SessionListInfo
	{
		public string SessionID { get; set; }
		public string CompanyName { get; set; }
		public DateTime SessionStart { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }
		public string UserGroup { get; set; }

		public SessionListInfo() { }

		public SessionListInfo(OpenSessionInfo info)
		{
			SessionID =info.SessionID;
			CompanyName = info.CompanyName;
			SessionStart = info.SessionStart;
			Name = info.User.Name;
			Position = info.User.Position;
			UserGroup = info.User.UserGroupName;
		}
	}
}
