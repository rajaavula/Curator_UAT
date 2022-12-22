using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class CompanyRegionManager
	{
		public static List<CompanyRegionInfo> GetCompanyRegions(int companyID)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", DBNull.Value),
				new SqlParameter("@UserID", DBNull.Value)
			};

			DataTable dt = db.QuerySP("GetCompanyRegions", parameters);

			if (!db.Success || db.RowCount == 0) return new List<CompanyRegionInfo>();

			return (from dr in dt.AsEnumerable() select new CompanyRegionInfo(dr)).ToList();
		}

		public static CompanyRegionInfo GetCompanyRegion(int companyID, int regionID)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@UserID", DBNull.Value)
			};

			DataTable dt = db.QuerySP("GetCompanyRegions", parameters);

			if (!db.Success || db.RowCount != 1) return null;

			return new CompanyRegionInfo(dt.Rows[0]);
		}

		public static List<CompanyRegionInfo> GetCompanyRegionsByUser(int companyID, int userID)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", DBNull.Value),
				new SqlParameter("@UserID", Utils.ToDBValue(userID))
			};

			DataTable dt = db.QuerySP("GetCompanyRegions", parameters);

			if (!db.Success || db.RowCount == 0) return new List<CompanyRegionInfo>();

			return (from dr in dt.AsEnumerable() select new CompanyRegionInfo(dr)).ToList();
		}

		public static Exception Save(int? regionID, CompanyRegionInfo info)
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
				new SqlParameter("@Notes", Utils.ToDBValue(info.Notes)),
				new SqlParameter("@CopyRegionalEmailTo", Utils.ToDBValue(info.CopyRegionalEmailTo)),
				new SqlParameter("@SalesSupportEmailAddress", Utils.ToDBValue(info.SalesSupportEmailAddress)),
				new SqlParameter("@PurchasingDeptEmailAddress", Utils.ToDBValue(info.PurchasingDeptEmailAddress)),
				new SqlParameter("@EmailServer", Utils.ToDBValue(info.EmailServer)),
				new SqlParameter("@EmailUsername", Utils.ToDBValue(info.EmailUsername)),
				new SqlParameter("@EmailPassword", Utils.ToDBValue(info.EmailPassword)),
				new SqlParameter("@EmailFromEmail", Utils.ToDBValue(info.EmailFromEmail)),
				new SqlParameter("@EmailFromName", Utils.ToDBValue(info.EmailFromName))
			};

			db.QuerySP("SaveCompanyRegion", parameters);

			return db.DBException;
		}

		public static Exception Delete(int companyID, int regionID)
		{
			var db = new DB(App.CuratorDBConn);
			var parameters = new[]
			{
			    new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID))
			};

			db.QuerySP("DeleteCompanyRegion", parameters);

			return db.DBException;
		}
	}
}
