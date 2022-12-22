using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class MemberCategoryByFeedInfo
    {
        public int FeedKey { get; set; }
        public string FeedName { get; set; }
        public int CategoryKey { get; set; }

        public MemberCategoryByFeedInfo()
        {
        }

        public MemberCategoryByFeedInfo(DataRow dr)
        {
            FeedKey = Utils.FromDBValue<int>(dr["FeedKey"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            CategoryKey = Utils.FromDBValue<int>(dr["CategoryKey"]);
        }
    }
}