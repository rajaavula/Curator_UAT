using System;
using System.Collections.Generic;
using System.Globalization;

namespace LeadingEdge.Curator.Core
{
	public class SessionInfo
	{
		public string SessionID { get; set; }
		public UserInfo User { get; set; }
		public CompanyInfo Company { get { return App.Companies.Find(x => x.CompanyID == User.CompanyID); } }
		public UserGroupInfo UserGroup { get; set; }
		public List<CompanyRegionInfo> UserRegions { get; set; }
		public CompanyRegionInfo CurrentRegion { get { return UserRegions.Find(x => x.RegionID == User.RegionID); } }
		public string IPAddress { get; set; }
		public string Referrer { get; set; }
		public SessionData Data { get; set; }
		public List<ActivePage> ActivePages { get; set; }
		public int LastPageID { get; set; }
		public LanguageInfo Language { get; set; }
		public CultureInfo Culture { get; set; }
		public bool LabelEditMode { get; set; }
		public List<FormInfo> PermittedForms { get; set; }
		public List<FormInfo> Bookmarks { get; set; }
		public List<UserGroupPermissionInfo> UserGroupPermissions { get; set; }
		public int VisitorLogID { get; set; }
		public int CustomerID { get; set; }
		public Dictionary<string, string> Layout { get; set; }// grid layout
		public bool MenuExpanded { get; set; }

		public SessionInfo()
		{
			UserRegions = new List<CompanyRegionInfo>();
			Data = new SessionData();
			ActivePages = new List<ActivePage>();
			PermittedForms = new List<FormInfo>();
			Bookmarks = new List<FormInfo>();
			UserGroupPermissions = new List<UserGroupPermissionInfo>();
			Layout = new Dictionary<string, string>();
		}

		public string GetDateTimeFormat(string originalFormat)
		{
			string strDateFormat = "dd/MM/yyyy";

			if (String.IsNullOrEmpty(originalFormat)) originalFormat = "{0:dd/MM/yyyy}";

			bool bIncludeHours = originalFormat.Contains("HH");
			bool bIncludeMinutes = originalFormat.Contains("mm");
			bool bIncludeSeconds = originalFormat.Contains("ss");

			if (bIncludeHours) strDateFormat += " HH";
			if (bIncludeMinutes) strDateFormat += ":mm";
			if (bIncludeSeconds) strDateFormat += ":ss";

			return ("{0:" + strDateFormat + "}");
		}

		public string FormatDateTime(string originalFormat, DateTime? value)
		{
			if (value.HasValue == false || value == DateTime.MinValue) return null;

			string strCellDateTimeFormat = GetDateTimeFormat(originalFormat);

			return String.Format(strCellDateTimeFormat, value);
		}
	}
}
