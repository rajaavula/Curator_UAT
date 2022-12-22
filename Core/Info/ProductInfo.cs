using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ProductInfo
    {
        public int ProductID { get; set; }
        public Guid ProductGuid { get; set; }
        public int? CategoryKey { get; set; }
        public string SupplierPartNumber { get; set; }      // SKU
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Feed { get; set; }
        public string Brand { get; set; }
        public string Barcode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCategory { get; set; }
        public string SupplierSubcategory1 { get; set; }
        public string SupplierSubcategory2 { get; set; }
        public string CategoryName { get; set; }
        public string Category1Name { get; set; }
        public string Category2Name { get; set; }
        public string Category3Name { get; set; }
        public string Category4Name { get; set; }
        public string Category5Name { get; set; }
        public string Tags { get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? RecommendedRetailPrice { get; set; }
        public decimal? MemberRecommendedRetailPrice { get; set; }
        public decimal? Markup { get; set; }
        public int StockOnHand { get; set; }
        public bool IncludeZeroStock { get; set; }
        public int PricingRule { get; set; }
        public string PricingRuleText
        {
            get
            {
                switch (this.PricingRule)
                {
                    case 0:
                        return " No pricing";
                    case 1:
                        return "New RRP";
                    case 2:
                        return "RRP adjustment $";
                    case 3:
                        return "RRP adjustment %";
                    case 4:
                        return "Buy price adjustment $";
                    case 5:
                        return "Buy price adjustment %";
                }
                return "";
            }
        }
        public decimal PriceValue { get; set; }
        public bool RetailRounding { get; set; }
        public decimal NewRRP { get; set; }
        public decimal? MarkUpPercentage
        {
            get
            {
                if (BuyPrice == 0) return null;
                return ((this.NewRRP / BuyPrice) - 1) * 100;
            }
        }
        public DateTime MemberPriceModifiedDate { get; set; }
        public bool NegativeProfit
        {
            get
            {
                if (BuyPrice.HasValue == false) return false;

                if (RecommendedRetailPrice.HasValue == false) return false;

                if (PricingRule == 0) return (RecommendedRetailPrice - BuyPrice < 0);

                return (NewRRP - BuyPrice < 0);
            }
        }
        public string CategoryPath
        {
            get
            {
                string path = Category1Name;
                if (!String.IsNullOrEmpty(Category2Name)) path += string.Concat(" / ", Category2Name);
                if (!String.IsNullOrEmpty(Category3Name)) path += string.Concat(" / ", Category3Name);
                if (!String.IsNullOrEmpty(Category4Name)) path += string.Concat(" / ", Category4Name);
                if (!String.IsNullOrEmpty(Category5Name)) path += string.Concat(" / ", Category5Name);
                return path;
            }
        }
        public string ProductTags { get; set; }
        public bool DefaultProductTags {  get; set; }
        public bool Selected { get; set;}

        public ProductInfo() { }
        
        public ProductInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("ProductID")) ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            if (dr.Table.Columns.Contains("ProductGuid")) ProductGuid = Utils.FromDBValue<Guid>(dr["ProductGuid"]);
            if (dr.Table.Columns.Contains("CategoryKey")) CategoryKey = Utils.FromDBValue<int?>(dr["CategoryKey"]);
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

            if (dr.Table.Columns.Contains("CategoryName")) CategoryName = Utils.FromDBValue<string>(dr["CategoryName"]);
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
            if (dr.Table.Columns.Contains("IncludeZeroStock")) IncludeZeroStock = Utils.FromDBValue<bool>(dr["IncludeZeroStock"]);

            if (dr.Table.Columns.Contains("PricingRule")) PricingRule = Utils.FromDBValue<int>(dr["PricingRule"]);
            if (dr.Table.Columns.Contains("PriceValue")) PriceValue = Utils.FromDBValue<decimal>(dr["PriceValue"]);
            if (dr.Table.Columns.Contains("RetailRounding")) RetailRounding = Utils.FromDBValue<bool>(dr["RetailRounding"]);
            if (dr.Table.Columns.Contains("NewRRP")) NewRRP = Utils.FromDBValue<decimal>(dr["NewRRP"]);
            if (dr.Table.Columns.Contains("MemberPriceModifiedDate")) MemberPriceModifiedDate = Utils.FromDBValue<DateTime>(dr["MemberPriceModifiedDate"]);
            if (dr.Table.Columns.Contains("ProductTags")) ProductTags = Utils.FromDBValue<string>(dr["ProductTags"]);
            if (dr.Table.Columns.Contains("DefaultProductTags")) DefaultProductTags = Utils.FromDBValue<bool>(dr["DefaultProductTags"]);

            if (dr.Table.Columns.Contains("Selected")) Selected = Utils.FromDBValue<bool>(dr["Selected"]);

        }
    }
}