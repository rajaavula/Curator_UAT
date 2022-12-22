using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class ChangeHistoryManager
	{
		public static List<ChangeHistoryInfo> GetChangeHistory(int companyID, int regionID, string referenceType, int referenceID, int userID)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@ReferenceType", Utils.ToDBValue(referenceType)),
				new SqlParameter("@ReferenceID", Utils.ToDBValue(referenceID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID))
			};

			DataTable dt = db.QuerySP("GetChangeHistory", parameters);

			return new List<ChangeHistoryInfo>(from dr in dt.AsEnumerable() select new ChangeHistoryInfo(dr));
		}

		public static Exception SaveChangeHistory(ChangeHistoryInfo info)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
				new SqlParameter("@ReferenceType", Utils.ToDBValue(info.ReferenceType)),
				new SqlParameter("@ReferenceID", Utils.ToDBValue(info.ReferenceID)),
				new SqlParameter("@UserID", Utils.ToDBValue(info.UserID)),
				new SqlParameter("@PlaceholderID", Utils.ToDBValue(info.PlaceholderID)),
				new SqlParameter("@OldValue", Utils.ToDBValue(info.OldValue)),
				new SqlParameter("@NewValue", Utils.ToDBValue(info.NewValue))
			};

			db.QuerySP("SaveChangeHistory", parameters);

			return db.DBException;
		}

        public static List<ChangeHistoryInfo> GetChangeHistoryProductsDB(int companyID, int regionID, string referenceType, int referenceID, int userID)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
                new SqlParameter("@ReferenceType", Utils.ToDBValue(referenceType)),
                new SqlParameter("@ReferenceID", Utils.ToDBValue(referenceID)),
                new SqlParameter("@UserID", Utils.ToDBValue(userID))
            };

            DataTable dt = db.QuerySP("GetChangeHistory", parameters);

            return new List<ChangeHistoryInfo>(from dr in dt.AsEnumerable() select new ChangeHistoryInfo(dr));
        }
    }
}