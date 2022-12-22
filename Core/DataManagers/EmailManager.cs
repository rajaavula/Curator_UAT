using System;
using System.Data;
using System.Data.SqlClient;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class EmailManager
	{
		public static int? InsertEmail(EmailInfo info)
		{
			if (info == null || string.IsNullOrEmpty(info.To)) return null;

			return InsertEmail(info.To, info.CC, info.BCC, info.ReplyTo, info.Subject, info.Body, info.CompanyID, info.RegionID, info.ReferenceID, info.ReferenceNotes, info.EmailType, info.UserID);
		}

		public static int? InsertEmail(string to, string subject, string body, int companyID, int regionID, string emailType, int userID)
		{
			return InsertEmail(to, null, null,null, subject, body, companyID, regionID, null, null, emailType, userID);
		}

		public static int? InsertEmail(string to, string cc, string bcc, string replyTo, string subject, string body, int companyID, int regionID, string emailType, int userID)
		{
			return InsertEmail(to, cc, bcc, replyTo, subject, body, companyID, regionID, null, null, emailType, userID);
		}

		public static int? InsertEmail(string to, string cc, string bcc, string replyTo, string subject, string body, int? companyID, int? regionID, int? referenceID, string referenceNotes, string emailType, int? userID)
		{
			try
			{
				if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(body)) return null;

				DB db = new DB(App.CuratorDBConn);

				DataTable dtResult = db.QuerySP("CreateEmail", new[]
				{
					new SqlParameter("@To", Utils.ToDBValue(to)),
					new SqlParameter("@From", "support@cortex.co.nz"), // EDIT FOR CLIENT
					new SqlParameter("@CC", Utils.ToDBValue(cc)),
					new SqlParameter("@BCC", Utils.ToDBValue(bcc)),
					new SqlParameter("@ReplyTo", Utils.ToDBValue(replyTo)),
					new SqlParameter("@Subject", Utils.ToDBValue(subject)),
					new SqlParameter("@Body", Utils.ToDBValue(body)),
					new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
					new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
					new SqlParameter("@ReferenceID", Utils.ToDBValue(referenceID)),
					new SqlParameter("@ReferenceNotes", Utils.ToDBValue(referenceNotes)),
					new SqlParameter("@EmailType", Utils.ToDBValue(emailType)),
					new SqlParameter("@UserID", Utils.ToDBValue(userID))
				});

				if (db.Success == false) throw new Exception(db.ErrorMessage);

				if (dtResult == null || dtResult.Rows.Count < 1)
				{
					throw new Exception("No data returned from stored procedure CreateEmail.");
				}

				object dbResult = dtResult.Rows[0][0];

				if (dbResult == DBNull.Value) return null;

				return Utils.FromDBValue<int>(dbResult);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return null;
		}

        public static int? InsertEmailObject(EmailObjectInfo info)
        {
			return InsertEmailObject(info.EmailID, info.Name, info.Bytes, info.ContentType, info.RegionID, info.CompanyID);
        }

        public static int? InsertEmailObject(int emailID, string name, byte[] bytes, string contentType, int regionID, int companyID)
		{
			try
			{
				if (bytes == null || bytes.Length < 1) return null;

				DB db = new DB(App.CuratorDBConn);

				DataTable dtResult = db.QuerySP("CreateEmailObject", new[]
				{
					new SqlParameter("@EmailID", Utils.ToDBValue(emailID)),
					new SqlParameter("@Name", Utils.ToDBValue(name)),
					new SqlParameter("@Bytes", Utils.ToDBValue(bytes)),
					new SqlParameter("@ContentType", Utils.ToDBValue(contentType)),
					new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
					new SqlParameter("@CompanyID", Utils.ToDBValue(companyID))
				});

				if (db.Success == false) throw new Exception(db.ErrorMessage);

				if (dtResult == null || dtResult.Rows.Count < 1)
				{
					throw new Exception("No data returned from stored procedure CreateEmailObject.");
				}

				object dbResult = dtResult.Rows[0][0];

				if (dbResult == DBNull.Value) return null;

				return Utils.FromDBValue<int>(dbResult);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return null;
		}
	}
}
