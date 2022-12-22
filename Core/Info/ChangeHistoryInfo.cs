using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class ChangeHistoryInfo
	{
		public int ChangeID { get; set; }
		public int CompanyID { get; set; }
		public int RegionID { get; set; }
		public string ReferenceType { get; set; }
		public int ReferenceID { get; set; }
		public int UserID { get; set; }
		public string UserName { get; set; }
		public string LanguageID { get; set; }
		public DateTime Time { get; set; }
		public int PlaceholderID { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public string Details { get; set; }

		public string FormattedDetail
		{
			get
			{
				if (string.IsNullOrEmpty(Details) == false) return Details;

				string waschangedfrom = App.GetLabel(CompanyID, LanguageID, 200959).LabelText;
				string to = App.GetLabel(CompanyID, LanguageID, 200960).LabelText;
				string wasremoved = App.GetLabel(CompanyID, LanguageID, 200963).LabelText;
				string wasadded = App.GetLabel(CompanyID, LanguageID, 200964).LabelText;
				string property = App.GetLabel(CompanyID, LanguageID, PlaceholderID).LabelText;

				if (string.IsNullOrEmpty(OldValue)) 
				{
					return string.Format("{0} '{1}' {2}", property, NewValue, wasadded);
				}

				if (string.IsNullOrEmpty(NewValue)) 
				{
					return string.Format("{0} '{1}' {2}", property, OldValue, wasremoved);
				}

				// Assume changed from -> to
				return string.Format("{0} {1} '{2}' {3} '{4}'", property, waschangedfrom, OldValue, to, NewValue);
			}
		}

		public ChangeHistoryInfo(){}

		public ChangeHistoryInfo(DataRow dr)
		{
			ChangeID = Utils.FromDBValue<int>(dr["ChangeID"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
			ReferenceType = Utils.FromDBValue<string>(dr["ReferenceType"]);
			ReferenceID = Utils.FromDBValue<int>(dr["ReferenceID"]);
			UserID = Utils.FromDBValue<int>(dr["UserID"]);
			LanguageID = Utils.FromDBValue<string>(dr["LanguageID"]);
			UserName = Utils.FromDBValue<string>(dr["UserName"]);
			Time = U.GetLocalTime(Utils.FromDBValue<DateTime>(dr["Time"]));
			PlaceholderID = Utils.FromDBValue<int>(dr["DescriptionPlaceholderID"]);
			OldValue = Utils.FromDBValue<string>(dr["OldValue"]);
			NewValue = Utils.FromDBValue<string>(dr["NewValue"]);
		}
	}
}
