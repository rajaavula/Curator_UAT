using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class FeedInfo
    {
        public int FeedKey { get; set; }
        public string FeedName { get; set; }
        public bool IncludeZeroStock { get; set; }
        public string PushToSupplierEmail { get; set; }

        public FeedInfo() { }

        public FeedInfo(DataRow dr)
        {
            FeedKey = Utils.FromDBValue<int>(dr["FeedKey"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            IncludeZeroStock = Utils.FromDBValue<bool>(dr["IncludeZeroStock"]);
            PushToSupplierEmail = Utils.FromDBValue<string>(dr["PushToSupplierEmail"]);
        }
    }
}