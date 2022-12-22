using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class BrandByFeedInfo
    {
        public int FeedKey { get; set; }
        public string FeedName { get; set; }
        public int BrandKey { get; set; }
        public string BrandName { get; set; }

        public BrandByFeedInfo()
        {
        }

        public BrandByFeedInfo(DataRow dr)
        {
            FeedKey = Utils.FromDBValue<int>(dr["FeedKey"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            BrandKey = Utils.FromDBValue<int>(dr["BrandKey"]);
            BrandName = Utils.FromDBValue<string>(dr["BrandName"]);
        }
    }
}