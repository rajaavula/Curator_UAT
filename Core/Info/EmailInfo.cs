using System;
using System.Data;

namespace LeadingEdge.Curator.Core
{
	public class EmailInfo
	{
		public int EmailID { get; set; }
		public string To { get; set; }
		public string From { get; set; }
		public string CC { get; set; }
		public string BCC { get; set; }
		public string ReplyTo { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public int? CompanyID { get; set; }
		public int? RegionID { get; set; }
		public int? ReferenceID { get; set; }
		public string ReferenceNotes { get; set; }
		public string EmailType { get; set; }
		public int? UserID { get; set; }
		public DateTime Queued { get; set; }
		public DateTime? Sent { get; set; }

		public EmailInfo(){}

		public EmailInfo(DataRow dr)
		{
			//EmailID = Utils.From
		}
	}
}
