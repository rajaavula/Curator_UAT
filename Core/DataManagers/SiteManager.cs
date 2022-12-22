using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class SiteManager
	{
		public static List<SiteInfo> GetUserSites(int companyID, int regionID)
		{
			return GetSites(companyID, regionID, null);
        }

		public static List<SiteInfo> GetSites(int companyID, int regionID)
		{
			return GetSites(companyID, regionID, null);
		}

		public static SiteInfo GetSite(int companyID, int regionID, int siteID)
		{
			SiteInfo info = GetSites(companyID, regionID, siteID).FirstOrDefault();
			return info;
		}

		private static List<SiteInfo> GetSites(int companyID, int regionID, int? siteID)
		{
			
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@SiteID", Utils.ToDBValue(siteID)),
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID))
			};

			var dt = db.QuerySP("GetSites", parameters);

			return (from dr in dt.AsEnumerable() select new SiteInfo(dr)).ToList();
			
		}
		public static Exception SaveSite(SiteInfo info)
		{

			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
					new SqlParameter("@SiteID", Utils.ToDBValue(info.SiteID)),
					new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
					new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
					new SqlParameter("@CreatedByID", Utils.ToDBValue(info.CreatedByID)),
					new SqlParameter("@ModifiedByID", Utils.ToDBValue(info.ModifiedByID)),
					new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
					new SqlParameter("@Address1", Utils.ToDBValue(info.Address1)),
					new SqlParameter("@Address2", Utils.ToDBValue(info.Address2)),
					new SqlParameter("@Suburb", Utils.ToDBValue(info.Suburb)),
					new SqlParameter("@City", Utils.ToDBValue(info.City)),
					new SqlParameter("@AreaCode", Utils.ToDBValue(info.AreaCode)),
					new SqlParameter("@Latitude", Utils.ToDBValue(info.Latitude)),
					new SqlParameter("@Longitude", Utils.ToDBValue(info.Longitude)),
			};

			DataTable dt = db.QuerySP("SaveSite", parameters);

			if (db.Success == false) return db.DBException;

			if (info.SiteID < 0)
			{
				if (db.RowCount != 1) return new Exception("Unable to get new site ID");

				info.SiteID = Utils.FromDBValue<int>(dt.Rows[0][0]);
			}

			return null;

		}
	}
}
