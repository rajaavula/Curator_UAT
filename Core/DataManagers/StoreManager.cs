
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class StoreManager
    {
        #region Store

        public static List<StoreInfo> GetStores()
        {
            var db = new DB(App.ProductsDBConn);
            var dt = db.QuerySP("CURATOR_GetStores");

            return (from dr in dt.AsEnumerable() select new StoreInfo(dr)).ToList();
        }

        public static StoreInfo GetStore(int storeID)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID))

            };
            var dt = db.QuerySP("CURATOR_GetStore", parameters);

            if (!db.Success || db.RowCount != 1) 
            {
                Log.Error(new Exception(db.ErrorMessage + " could not find store"));
                return new StoreInfo(); 
            }

            return new StoreInfo(dt.Rows[0]);
        }

        public static Exception Save(StoreInfo info)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(info.StoreID)),
                new SqlParameter("@StoreUrl",Utils.ToDBValue(info.StoreUrl)),
                new SqlParameter("@StoreName",Utils.ToDBValue(info.StoreName)),
                new SqlParameter("@StoreApiKey",Utils.ToDBValue(info.StoreApiKey)),
                new SqlParameter("@StorePassword",Utils.ToDBValue(info.StorePassword)),
                new SqlParameter("@StoreSharedSecret",Utils.ToDBValue(info.StoreSharedSecret)),
                new SqlParameter("@ShopifyID",Utils.ToDBValue(info.ShopifyID)),
                new SqlParameter("@Logo", Utils.ToDBValue(info.Logo)) { SqlDbType = SqlDbType.Image }
            };

            db.QuerySP("CURATOR_SaveStore", parameters);

            return db.DBException;
        }

        public static Exception DeleteStore(int storeID)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID))
            };

            db.QuerySP("CURATOR_DeleteStore", parameters);

            return db.DBException;
        }

        #endregion
    }
}