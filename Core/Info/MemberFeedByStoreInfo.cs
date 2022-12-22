using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class MemberFeedByStoreInfo
    {
        public int FeedKey { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public string FeedName { get; set; }

        public MemberFeedByStoreInfo()
        {
        }

        public MemberFeedByStoreInfo(DataRow dr)
        {
            FeedKey = Utils.FromDBValue<int>(dr["FeedKey"]);
            if (dr.Table.Columns.Contains("StoreName")) StoreName = Utils.FromDBValue<string>(dr["StoreName"]);
            if (dr.Table.Columns.Contains("StoreID")) StoreID = Utils.FromDBValue<int>(dr["StoreID"]);
            if (dr.Table.Columns.Contains("FeedName")) FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
        }
    }
}