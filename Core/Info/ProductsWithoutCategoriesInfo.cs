using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ProductsWithoutCategoriesInfo
    {
        public int ProductID { get; set; }
        public string SupplierPartNumber { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Brand { get; set; }
        public string SupplierCategory { get; set; }
        public string SupplierSubcategory1 { get; set; }
        public string SupplierSubcategory2 { get; set; }
        public bool? IsTVSeries { get; set; }

        public ProductsWithoutCategoriesInfo() { }
        
        public ProductsWithoutCategoriesInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("ProductID")) ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            if (dr.Table.Columns.Contains("SupplierPartNumber")) SupplierPartNumber = Utils.FromDBValue<string>(dr["SupplierPartNumber"]);
            if (dr.Table.Columns.Contains("ShortDescription")) ShortDescription = Utils.FromDBValue<string>(dr["ShortDescription"]);
            if (dr.Table.Columns.Contains("LongDescription")) LongDescription = Utils.FromDBValue<string>(dr["LongDescription"]);
            if (dr.Table.Columns.Contains("SupplierCategory")) SupplierCategory = Utils.FromDBValue<string>(dr["SupplierCategory"]);
            if (dr.Table.Columns.Contains("SupplierSubcategory1")) SupplierSubcategory1 = Utils.FromDBValue<string>(dr["SupplierSubCategory1"]);
            if (dr.Table.Columns.Contains("SupplierSubcategory2")) SupplierSubcategory2 = Utils.FromDBValue<string>(dr["SupplierSubCategory2"]);
            if (dr.Table.Columns.Contains("IsTVSeries")) IsTVSeries = Utils.FromDBValue<bool?>(dr["IsTVSeries"]);
        }
    }
}