using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class CompanyManager
	{
		public static List<CompanyInfo> GetCompanies()
		{
			return GetCompanies(false);
		}

		public static List<CompanyInfo> GetCompanies(bool includeLogo)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@CompanyID", DBNull.Value),
				new SqlParameter("@IncludeLogo", Utils.ToDBValue(includeLogo))
			};

			DataTable dt = db.QuerySP("GetCompanies", parameters);
			return (from dr in dt.AsEnumerable() select new CompanyInfo(dr)).ToList();
		}

		public static CompanyInfo GetCompany(int companyID, bool includeLogo)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters = new[] 
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@IncludeLogo", Utils.ToDBValue(includeLogo))
			};

			DataTable dt = db.QuerySP("GetCompanies", parameters);

			if (!db.Success || db.RowCount != 1) return null;

			return new CompanyInfo(dt.Rows[0]);
		}

		public static Exception Save(CompanyInfo info)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
				new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
				new SqlParameter("@Notes", Utils.ToDBValue(info.Notes)),
				new SqlParameter("@Live", Utils.ToDBValue(info.Live)),
				new SqlParameter("@ApplicationTitle", Utils.ToDBValue(info.ApplicationTitle)),
				new SqlParameter("@Theme", Utils.ToDBValue(info.Theme)),
				new SqlParameter("@CopyEmailTo", Utils.ToDBValue(info.CopyEmailTo)),
				new SqlParameter("@SecurityAdminEmail", Utils.ToDBValue(info.SecurityAdminEmail)),
				new SqlParameter("@MaximumSessions", Utils.ToDBValue(info.MaximumSessions)),
				new SqlParameter("@RestrictSessions", Utils.ToDBValue(info.RestrictSessions)),
				new SqlParameter("@Logo", Utils.ToDBValue(info.Logo)) { SqlDbType = SqlDbType.Image }
			};

			db.QuerySP("SaveCompany", parameters);

			return db.DBException;
		}

		public static Exception CreateCompany(CompanyInfo info)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyName", Utils.ToDBValue(info.Name)),
				new SqlParameter("@OwnerName", Utils.ToDBValue(info.OwnerName)),
				new SqlParameter("@OwnerLogin", Utils.ToDBValue(info.OwnerLogin)),
				new SqlParameter("@OwnerEmail", Utils.ToDBValue(info.OwnerEmail)),
				new SqlParameter("@OwnerPassword", Utils.ToDBValue(info.OwnerPassword))
			};

			db.QuerySP("CreateCompany", parameters);

			return db.DBException;
		}
	}
}
