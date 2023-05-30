using Cortex.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class FeedStoreManager
    {
        public static List<FeedStoreInfo> GetAllFeedStores()
        {
            return GetFeedStores(null, null, "DATA");
        }

        public static FeedStoreInfo GetFeedStoreByIDs(int feedID, int storeID)
        {
            var data = GetFeedStores(feedID, storeID, "FEEDSTORE");

            return data.FirstOrDefault();
        }

        private static List<FeedStoreInfo> GetFeedStores(int? feedID, int? storeID, string type)
		{
			var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@FeedID", Utils.ToDBValue(feedID)),
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@Type", Utils.ToDBValue(type))
            };

            var dt = db.QuerySP("CURATOR_GetFeedStores", parameters);

            if (!db.Success)
            {
                Log.Error(db.DBException);

                return new List<FeedStoreInfo>();
            }

            return (from dr in dt.AsEnumerable() select new FeedStoreInfo(dr)).ToList();
        }

        public static Exception SaveFeedStore(FeedStoreInfo info)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@FeedID", Utils.ToDBValue(info.FeedID)),
                new SqlParameter("@StoreID", Utils.ToDBValue(info.StoreID)),
                new SqlParameter("@PushToSupplierEmail", Utils.ToDBValue(info.PushToSupplierEmail)),
                new SqlParameter("@CredentialsProvided", Utils.ToDBValue(info.CredentialsProvided))
            };

            db.QuerySP("CURATOR_SaveFeedStore", parameters);

            if (!db.Success)
            {
                Log.Error(db.DBException);
            }

            return db.DBException;
        }

        public static void SaveFeedStoreSecret(string key, string value)
        {
            
        }
    }
}