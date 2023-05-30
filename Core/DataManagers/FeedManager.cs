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
        public static List<FeedInfo> GetAllFeeds()
        {
            return GetFeeds(null, null, "DATA");
        }

        public static FeedInfo GetFeedByID(int feedID)
        {
            var data = GetFeeds(feedID, null, "FEED");

            return data.FirstOrDefault();
        }

        public static List<FeedInfo> GetFeedsByMember(int storeID)
        {
            return GetFeeds(null, storeID, "MEMBER");
        }

        private static List<FeedInfo> GetFeeds(int? feedID, int? storeID, string type)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@FeedID", Utils.ToDBValue(feedID)),
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@Type", Utils.ToDBValue(type))
            };

            var dt = db.QuerySP("CURATOR_GetFeeds", parameters);

            return (from dr in dt.AsEnumerable() select new FeedInfo(dr)).ToList();
        }

        public static List<ValueDescription> GetAllFeedsList(bool includeEmpty)
        {
            var list = new List<ValueDescription>();

            if (includeEmpty) list.Add(new ValueDescription(null, null));

            var feeds = GetAllFeeds();

            list.AddRange(from c in feeds select new ValueDescription(c.FeedKey, c.FeedName));

            return list;
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
                new SqlParameter("@IncludeZeroStock", Utils.ToDBValue(info.IncludeZeroStock))
            };

            db.QuerySP("CURATOR_SaveFeed", parameters);

            return db.Success == false ? db.ErrorMessage : string.Empty;
        }
    }
}