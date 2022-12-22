using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class VisitorLogManager
	{
		public static List<VisitorLogInfo> GetVisitorLogs(int companyID, DateTime fromDate, DateTime toDate, bool showOwners)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@FromDate", Utils.ToDBValue(fromDate)),
				new SqlParameter("@ToDate", Utils.ToDBValue(toDate)),
				new SqlParameter("@ShowOwners", Utils.ToDBValue(showOwners))
			};

			var db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetVisitorLog", parameters);
			return (from dr in dt.AsEnumerable() select new VisitorLogInfo(dr)).ToList();
		}

		public static List<UserLoginHistoryInfo> GetUserLoginHistory(int companyID)
		{
			SqlParameter[] parameters = { new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)) };
			var db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetUserLoginHistory", parameters);
			return (from dr in dt.AsEnumerable() select new UserLoginHistoryInfo(dr)).ToList();
		}

		public static Exception SaveVisitorLog(SessionInfo SI)
		{
			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(SI.User.CompanyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(SI.User.RegionID)),
				new SqlParameter("@UserID", Utils.ToDBValue(SI.User.UserID))
			};

			DB db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("SaveVisitorLog", parameters);

			if (db.Success == false) return db.DBException;
			if (db.RowCount != 1) return new Exception("Unable to get new visitor log ID");

			SI.VisitorLogID = Convert.ToInt32(dt.Rows[0][0]);

			return db.DBException;
		}

		public static Exception EndSession(int visitorlogID)
		{
			var parameters = new[]
			{
				new SqlParameter("@VisitorLogID", Utils.ToDBValue(visitorlogID))
			};

			DB db = new DB(App.CuratorDBConn);
			db.QuerySP("EndSession", parameters);

			return db.DBException;
		}
	}
}