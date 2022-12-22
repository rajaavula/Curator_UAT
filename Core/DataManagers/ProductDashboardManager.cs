using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class ProductDashboardManager
	{		
		public static List<ProductDashboardInfo> GetProductDashboardItems()
		{
			DB db = new DB(App.ProductsDBConn);

			DataTable dt = db.QuerySP("CURATOR_GetProductDashboard");
			return (from dr in dt.AsEnumerable() select new ProductDashboardInfo(dr)).ToList();
		}

	}
}
