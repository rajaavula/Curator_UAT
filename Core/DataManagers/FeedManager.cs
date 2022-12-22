using Cortex.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class FeedManager
    {
		public static List<FeedInfo> GetUsedFeeds()
		{
			var db = new DB(App.ProductsDBConn);

			var dt = db.QuerySP("CURATOR_GetUsedFeeds");

			return (from dr in dt.AsEnumerable() select new FeedInfo(dr)).ToList();
        }

        public static List<ValueDescription> GetUsedFeedsList(bool nullOption)
        {
            var list = new List<ValueDescription>();

            if (nullOption) list.Add(new ValueDescription(null, null));

            var feeds = GetUsedFeeds().OrderBy(x => x.FeedName).ToList();

            list.AddRange(from c in feeds select new ValueDescription(c.FeedKey, c.FeedName));

            return list;
        }

        public static List<FeedInfo> GetMemberFeeds(int? storeID)
        {
            try
            {
                DB db = new DB(App.ProductsDBConn);

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                    
                };

                DataTable dt = db.QuerySP("CURATOR_GetMemberFeeds", parameters);

                if (!db.Success || db.RowCount == 0) return new List<FeedInfo>();

                return new List<FeedInfo>(from dr in dt.AsEnumerable() select new FeedInfo(dr));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return new List<FeedInfo>();
        }
      
        public static Exception UpdateMemberFeeds(int storeID, string feedKeys)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@FeedKeys", Utils.ToDBValue(feedKeys))       
            };

            var db = new DB(App.ProductsDBConn);

            db.QuerySP("CURATOR_UpdateMemberFeeds", sqlParameters);

            return db.DBException;
        }

        public static List<MemberFeedByStoreInfo> GetMemberFeedsByStore()
        {
            var db = new DB(App.ProductsDBConn);         
            var dt = db.QuerySP("CURATOR_GetMemberFeedsByStore");
            
            return (from dr in dt.AsEnumerable() select new MemberFeedByStoreInfo(dr)).ToList();
        }

        public static string SaveFeed(FeedInfo info)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@FeedID", Utils.ToDBValue(info.FeedKey)),
                new SqlParameter("@IncludeZeroStock", Utils.ToDBValue(info.IncludeZeroStock)),
                new SqlParameter("@PushToSupplierEmail", Utils.ToDBValue(info.PushToSupplierEmail))
            };

            db.QuerySP("CURATOR_SaveFeed", parameters);

            return db.Success == false ? db.ErrorMessage : string.Empty;
        }
    }
}