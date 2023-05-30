using Cortex.Utilities;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public  class SupplierLineInfo
    {
        public int FeedKey { get; set; }
        public string FeedName { get; set; }
        public bool IsEDISupplier { get; set; }
        public int Stock { get; set; }
        public decimal ResellerBuyEx { get; set; }
        public decimal WeightGrams { get; set; }

        public SupplierLineInfo() { }

        public SupplierLineInfo(DataRow dr)
        {
            FeedKey = Utils.FromDBValue<int>(dr["FeedKey"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            IsEDISupplier = Utils.FromDBValue<bool>(dr["IsEDISupplier"]);
            Stock = Utils.FromDBValue<int>(dr["Stock"]);
            ResellerBuyEx = Utils.FromDBValue<decimal>(dr["ResellerBuyEx"]);
            WeightGrams = Utils.FromDBValue<decimal>(dr["WeightGrams"]);
        }
    }
}