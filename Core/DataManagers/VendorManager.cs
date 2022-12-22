using Cortex.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class VendorManager
    {
        public static List<VendorInfo> GetVendors()
        {
            return GetVendors("DATA");
        }

		private static List<VendorInfo> GetVendors(string type)
		{
			var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@Type", Utils.ToDBValue(type))
            };

			var dt = db.QuerySP("CURATOR_GetVendors", parameters);

			return (from dr in dt.AsEnumerable() select new VendorInfo(dr)).ToList();
        }

        public static Exception UpdateVendor(VendorInfo info)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@VendorID", Utils.ToDBValue(info.VendorID)),
                new SqlParameter("@MagentoID", Utils.ToDBValue(info.MagentoID)),
                new SqlParameter("@MagentoEnabled", Utils.ToDBValue(info.MagentoEnabled)),
                new SqlParameter("@NetSuiteInternalID", Utils.ToDBValue(info.NetSuiteInternalID)),
                new SqlParameter("@NetSuiteEntityID", Utils.ToDBValue(info.NetSuiteEntityID)),
                new SqlParameter("@NetSuiteCode", Utils.ToDBValue(info.NetSuiteCode)),
                new SqlParameter("@NetSuiteName", Utils.ToDBValue(info.NetSuiteName)),
                new SqlParameter("@NetSuiteEnabled", Utils.ToDBValue(info.NetSuiteEnabled))
            };

            db.QuerySP("CURATOR_UpdateVendor", parameters);

            return db.DBException;
        }
    }
}
