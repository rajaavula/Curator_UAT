using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ProductBrandRestrictionInfo
    {
        public int ProductID { get; set; }
        public string SupplierPartNumber { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCategory { get; set; }
        public string SupplierSubCategory1 { get; set; }
        public string SupplierSubCategory2 { get; set; }

        public ProductBrandRestrictionInfo() { }
        
        public ProductBrandRestrictionInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("ProductID")) ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            if (dr.Table.Columns.Contains("SupplierPartNumber")) SupplierPartNumber = Utils.FromDBValue<string>(dr["SupplierPartNumber"]);
            if (dr.Table.Columns.Contains("ShortDescription")) ShortDescription = Utils.FromDBValue<string>(dr["ShortDescription"]);
            if (dr.Table.Columns.Contains("LongDescription")) LongDescription = Utils.FromDBValue<string>(dr["LongDescription"]);
            if (dr.Table.Columns.Contains("SupplierName")) SupplierName = Utils.FromDBValue<string>(dr["SupplierName"]);
            if (dr.Table.Columns.Contains("SupplierCategory")) SupplierCategory = Utils.FromDBValue<string>(dr["SupplierCategory"]);
            if (dr.Table.Columns.Contains("SupplierSubCategory1")) SupplierSubCategory1 = Utils.FromDBValue<string>(dr["SupplierSubCategory1"]);
            if (dr.Table.Columns.Contains("SupplierSubCategory2")) SupplierSubCategory2 = Utils.FromDBValue<string>(dr["SupplierSubCategory2"]);
        }
    }
}