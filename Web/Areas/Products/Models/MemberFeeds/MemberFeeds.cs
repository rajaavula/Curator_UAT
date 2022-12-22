using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class MemberFeeds : BaseModel
    {
        public int MemberFeedKey { get; set; }
        public Guid FeedID { get; set; }
        public string FeedKeys { get; set; }
        public bool IsSave { get; set; }

        public List<FeedInfo> Feeds { get; set; }
        public List<FeedInfo> AvailableFeeds { get; set; }
        public List<FeedInfo> AssignedFeeds { get; set; }
        public int FeedKey { get;  set; }
        public int StoreID { get; set; }
        public List<StoreInfo> MemberStoreList { get; set; }
        public MemberFeeds()
        {
            Feeds = new List<FeedInfo>();          
            AvailableFeeds = new List<FeedInfo>();
            AssignedFeeds = new List<FeedInfo>();
            MemberStoreList = new List<StoreInfo>();

        }
    }
}
