using System.Data.SqlClient;
using Cortex.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class CatalogManager
    {
		public static List<CatalogInfo> GetCatalogs()
		{			
			var db = new DB(App.ProductsDBConn);
			var dt = db.QuerySP("CURATOR_GetCatalogsFromProduct");

			return (from dr in dt.AsEnumerable() select new CatalogInfo(dr)).ToList();
		}
	}
}