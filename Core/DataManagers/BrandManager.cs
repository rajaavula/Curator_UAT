using System.Data.SqlClient;
using Cortex.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class BrandManager
    {
		public static List<BrandBySourceInfo> GetBrandsBySource()
		{			
			var db = new DB(App.ProductsDBConn);         
			var dt = db.QuerySP("CURATOR_GetBrandsBySource");

			return (from dr in dt.AsEnumerable() select new BrandBySourceInfo(dr)).ToList();
		}

		public static List<BrandByFeedInfo> GetBrandsByFeed() 
		{
			var db = new DB(App.ProductsDBConn);

			var dt = db.QuerySP("CURATOR_GetBrandsByFeed");

			return (from dr in dt.AsEnumerable() select new BrandByFeedInfo(dr)).ToList();
		}

		public static List<BrandInfo> GetBrands()
		{
			var db = new DB(App.ProductsDBConn);

			var dt = db.QuerySP("CURATOR_GetBrands");

			return (from dr in dt.AsEnumerable() select new BrandInfo(dr)).ToList();
		}
	}
}
