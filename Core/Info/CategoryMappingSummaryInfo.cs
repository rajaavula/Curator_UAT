using Cortex.Utilities;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public sealed class CategoryMappingSummaryInfo
    {
        public int CategoryMappingID { get; set; }
        public int Products { get; set; }

        public CategoryMappingSummaryInfo() { }

        public CategoryMappingSummaryInfo(DataRow dr)
        {
            CategoryMappingID = Utils.FromDBValue<int>(dr["CategoryMappingID"]);
            Products = Utils.FromDBValue<int>(dr["Products"]);
        }
    }
}