using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ERPIntegrationProductInfo
    {
        public int ProductID { get; set; }
        public string SupplierPartNumber { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Feed { get; set; }
        public string Brand { get; set; }
        public string Barcode { get; set; }

        public string SupplierName { get; set; }
        public string SupplierCategory { get; set; }
        public string SupplierSubcategory1 { get; set; }
        public string SupplierSubcategory2 { get; set; }

        public string Category1Name { get; set; }
        public string Category2Name { get; set; }
        public string Category3Name { get; set; }
        public string Category4Name { get; set; }
        public string Category5Name { get; set; }

        public decimal? BuyPrice { get; set; }
        public decimal? RecommendedRetailPrice { get; set; }
        public decimal? MemberRecommendedRetailPrice { get; set; }
        public decimal? Markup { get; set; }
        public int StockOnHand { get; set; }

        public bool QueuedToNetSuite { get; set; }
        public bool SentToNetSuite { get; set; }
        public bool QueuedToMagento { get; set; }
        public bool SentToMagento { get; set; }

        public ERPIntegrationProductInfo() { }

        public ERPIntegrationProductInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("ProductID")) ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            if (dr.Table.Columns.Contains("SupplierPartNumber")) SupplierPartNumber = Utils.FromDBValue<string>(dr["SupplierPartNumber"]);
            if (dr.Table.Columns.Contains("ShortDescription")) ShortDescription = Utils.FromDBValue<string>(dr["ShortDescription"]);
            if (dr.Table.Columns.Contains("LongDescription")) LongDescription = Utils.FromDBValue<string>(dr["LongDescription"]);
            if (dr.Table.Columns.Contains("Feed")) Feed = Utils.FromDBValue<string>(dr["Feed"]);
            if (dr.Table.Columns.Contains("Brand")) Brand = Utils.FromDBValue<string>(dr["Brand"]);
            if (dr.Table.Columns.Contains("Barcode")) Barcode = Utils.FromDBValue<string>(dr["Barcode"]);

            if (dr.Table.Columns.Contains("SupplierName")) SupplierName = Utils.FromDBValue<string>(dr["SupplierName"]);
            if (dr.Table.Columns.Contains("SupplierCategory")) SupplierCategory = Utils.FromDBValue<string>(dr["SupplierCategory"]);
            if (dr.Table.Columns.Contains("SupplierSubcategory1")) SupplierSubcategory1 = Utils.FromDBValue<string>(dr["SupplierSubCategory1"]);
            if (dr.Table.Columns.Contains("SupplierSubcategory2")) SupplierSubcategory2 = Utils.FromDBValue<string>(dr["SupplierSubCategory2"]);

            if (dr.Table.Columns.Contains("Category1Name")) Category1Name = Utils.FromDBValue<string>(dr["Category1Name"]);
            if (dr.Table.Columns.Contains("Category2Name")) Category2Name = Utils.FromDBValue<string>(dr["Category2Name"]);
            if (dr.Table.Columns.Contains("Category3Name")) Category3Name = Utils.FromDBValue<string>(dr["Category3Name"]);
            if (dr.Table.Columns.Contains("Category4Name")) Category4Name = Utils.FromDBValue<string>(dr["Category4Name"]);
            if (dr.Table.Columns.Contains("Category5Name")) Category5Name = Utils.FromDBValue<string>(dr["Category5Name"]);

            if (dr.Table.Columns.Contains("RecommendedRetailPrice")) RecommendedRetailPrice = Utils.FromDBValue<decimal?>(dr["RecommendedRetailPrice"]);
            if (dr.Table.Columns.Contains("MemberRecommendedRetailPrice")) MemberRecommendedRetailPrice = Utils.FromDBValue<decimal?>(dr["MemberRecommendedRetailPrice"]);
            if (dr.Table.Columns.Contains("BuyPrice")) BuyPrice = Utils.FromDBValue<decimal?>(dr["BuyPrice"]);
            if (dr.Table.Columns.Contains("Markup")) Markup = Utils.FromDBValue<decimal?>(dr["Markup"]);
            if (dr.Table.Columns.Contains("StockOnHand")) StockOnHand = Utils.FromDBValue<int>(dr["StockOnHand"]);

            if (dr.Table.Columns.Contains("QueuedToNetSuite")) QueuedToNetSuite = Utils.FromDBValue<bool>(dr["QueuedToNetSuite"]);
            if (dr.Table.Columns.Contains("SentToNetSuite")) SentToNetSuite = Utils.FromDBValue<bool>(dr["SentToNetSuite"]);
            if (dr.Table.Columns.Contains("QueuedToMagento")) QueuedToMagento = Utils.FromDBValue<bool>(dr["QueuedToMagento"]);
            if (dr.Table.Columns.Contains("SentToMagento")) SentToMagento = Utils.FromDBValue<bool>(dr["SentToMagento"]);
        }
    }
}