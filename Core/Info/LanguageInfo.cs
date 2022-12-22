using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class LanguageInfo
	{
		public string LanguageID { get; set; }
		public string LanguageName { get; set; }
		public string Culture { get; set; }

		public string FlagURL
		{
			get
			{
				return String.Format("/Assets/Img/{0}-flag.png", LanguageID.ToLower());
			}
		}
		
		public LanguageInfo() {}

		public LanguageInfo(DataRow dr)
		{
			LanguageID = Utils.FromDBValue<string>(dr["LanguageID"]);
			LanguageName = Utils.FromDBValue<string>(dr["LanguageName"]);
			Culture = Utils.FromDBValue<string>(dr["Culture"]);
		}
	}
}
