using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class ProductManager
    {
        #region Products

        public static List<ProductInfo> GetProducts(string feedName, string manufactureName, string sortBy, string sortDirection)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@MainSource", Utils.ToDBValue(feedName)),
                new SqlParameter("@ManufacturerName", Utils.ToDBValue(manufactureName)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection))
            };

            var dt = db.QuerySP("CURATOR_GetProducts", parameters);

            return (from dr in dt.AsEnumerable() select new ProductInfo(dr)).ToList();
        }

        public static Exception Save(ProductInfo info)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@ProductID", Utils.ToDBValue(info.ProductID)),
                new SqlParameter("@CategoryKey", Utils.ToDBValue(info.CategoryKey)),
                new SqlParameter("@BuyPrice", Utils.ToDBValue(info.BuyPrice)),
                new SqlParameter("@RecommendedRetailPrice", Utils.ToDBValue(info.RecommendedRetailPrice)),
                new SqlParameter("@MemberRecommendedRetailPrice", Utils.ToDBValue(info.MemberRecommendedRetailPrice)),
                new SqlParameter("@Markup", Utils.ToDBValue(info.Markup)),
                new SqlParameter("@StockOnHand", Utils.ToDBValue(info.StockOnHand)),
                new SqlParameter("@IncludeZeroStock", Utils.ToDBValue(info.IncludeZeroStock))
            };

            DataTable dt = db.QuerySP("CURATOR_SaveProduct", parameters);

            return db.DBException;
        }

        #endregion

        #region Products without categories

        public static List<ProductsWithoutCategoriesInfo> GetProductsWithoutCategories(string feedName, string sortBy, string sortDirection)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@MainSource", Utils.ToDBValue(feedName)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection))
            };

            var dt = db.QuerySP("CURATOR_GetProductsWithoutCategories", parameters);

            return (from dr in dt.AsEnumerable() select new ProductsWithoutCategoriesInfo(dr)).ToList();
        }

        public static Exception AssignCategoryToProducts(int categoryKey, string productIDList, int companyID, int regionID, int userID)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@CategoryKey", Utils.ToDBValue(categoryKey)),
                new SqlParameter("@ProductIDList", Utils.ToDBValue(productIDList))
            };

            db.QuerySP("CURATOR_AssignCategoryToProducts", parameters);

            if (db.DBException == null)     // Create a change history record for Undo to CURATOR DB
            {
                var info = new ChangeHistoryInfo();
                info.CompanyID = companyID;
                info.RegionID = regionID;
                info.ReferenceType = "PRODUCTS_WITHOUT_CATEGORIES";
                info.ReferenceID = categoryKey;
                info.NewValue = productIDList;
                info.UserID = userID;
                info.PlaceholderID = 200748; // Products without categories

                ChangeHistoryManager.SaveChangeHistory(info);
            }

            return db.DBException;
        }

        #endregion

        #region ProductBrandRestrictions

        public static List<ProductBrandRestrictionInfo> GetProductBrandRestrictions(string brandName, string sortBy, string sortDirection)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@BrandName", Utils.ToDBValue(brandName)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection))
            };

            var dt = db.QuerySP("CURATOR_GetProductBrandRestrictions", parameters);

            return (from dr in dt.AsEnumerable() select new ProductBrandRestrictionInfo(dr)).ToList();
        }

        public static Exception AssignCatalogToProducts(string catalogName, string productIDList, int companyID, int regionID, int userID)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@catalogName", Utils.ToDBValue(catalogName)),
                new SqlParameter("@ProductIDList", Utils.ToDBValue(productIDList))
            };

            db.QuerySP("CURATOR_AssignCatalogToProducts", parameters);

            if (db.DBException == null)     // Create a change history record for Undo to CURATOR DB
            {
                var info = new ChangeHistoryInfo();
                info.CompanyID = companyID;
                info.RegionID = regionID;
                info.ReferenceType = "PRODUCT_BRAND_RESTRICTIONS";
                info.OldValue = productIDList;
                info.NewValue = catalogName;
                info.UserID = userID;
                info.PlaceholderID = 300103;

                ChangeHistoryManager.SaveChangeHistory(info);
            }

            return db.DBException;
        }

        #endregion

        #region Integration Products

        public static List<ERPIntegrationProductInfo> GetERPIntegrationProducts(Guid vendorID, string sortBy, string sortDirection)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@VendorID", Utils.ToDBValue(vendorID)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection))
            };

            var dt = db.QuerySP("CURATOR_GetERPIntegrationProducts", parameters);

            return (from dr in dt.AsEnumerable() select new ERPIntegrationProductInfo(dr)).ToList();
        }

        public static Exception SendSamplesToERPIntegrations(string productIDs)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@ProductIDs", Utils.ToDBValue(productIDs))
            };

            db.QuerySP("CURATOR_SendSamplesToERPIntegrations", parameters);

            return db.DBException;
        }

        #endregion

        #region Member Products 

        public static List<ProductInfo> GetMemberProducts(int storeID, string selectedFeeds, string selectedCategories, string selectedBrands, string sortBy, string sortDirection)
        {
            return GetMemberProducts(storeID, selectedFeeds, selectedCategories, selectedBrands, sortBy, sortDirection, App.MaxProductsRows, false);
        }

        public static List<ProductInfo> GetMemberProducts(int storeID, string selectedFeeds, string selectedCategories, string selectedBrands, string sortBy, string sortDirection, int maxRows, bool allRows)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID",Utils.ToDBValue(storeID)),
                new SqlParameter("@selectedFeeds",Utils.ToDBValue(selectedFeeds)),
                new SqlParameter("@SelectedCategories",Utils.ToDBValue(selectedCategories)),
                new SqlParameter("@SelectedBrands",Utils.ToDBValue(selectedBrands)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection)),
                new SqlParameter("@MaxRows", Utils.ToDBValue(maxRows)),
                new SqlParameter("@AllRows", Utils.ToDBValue(allRows))
            };
            var dt = db.QuerySP("CURATOR_GetMemberProducts", parameters);

            return (from dr in dt.AsEnumerable() select new ProductInfo(dr)).OrderBy(x => x.ShortDescription).ToList();
        }

        public static List<ProductInfo> GetAvailableMemberProducts(int storeID, string selectedFeeds, string selectedCategories, string selectedBrands, string sortBy, string sortDirection)
        {
            return GetAvailableMemberProducts(storeID, selectedFeeds, selectedCategories, selectedBrands, sortBy, sortDirection, App.MaxProductsRows, false);
        }

        public static List<ProductInfo> GetAvailableMemberProducts(int storeID, string selectedFeeds, string selectedCategories, string selectedBrands, string sortBy, string sortDirection, int maxRows, bool allRows)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID",Utils.ToDBValue(storeID)),
                new SqlParameter("@SelectedFeeds",Utils.ToDBValue(selectedFeeds)),
                new SqlParameter("@SelectedCategories",Utils.ToDBValue(selectedCategories)),
                new SqlParameter("@SelectedBrands",Utils.ToDBValue(selectedBrands)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection)),
                new SqlParameter("@MaxRows", Utils.ToDBValue(maxRows)),
                new SqlParameter("@AllRows", Utils.ToDBValue(allRows))
            };
            var dt = db.QuerySP("CURATOR_GetAvailableMemberProducts", parameters);

            return (from dr in dt.AsEnumerable() select new ProductInfo(dr)).OrderBy(x => x.ShortDescription).ToList();
        }

        public static Exception SaveMemberPricing(int storeID, string productIDList, int pricingRule, decimal priceValue, bool retailRounding, int modifiedBy, bool includeShipping, decimal shippingValue)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@ProductIDList", Utils.ToDBValue(productIDList)),
                new SqlParameter("@PricingRule", Utils.ToDBValue(pricingRule)),
                new SqlParameter("@PriceValue", Utils.ToDBValue(priceValue)),
                new SqlParameter("@RetailRounding", Utils.ToDBValue(retailRounding)),
                new SqlParameter("@ModifiedBy", Utils.ToDBValue(modifiedBy)),
                new SqlParameter("@IncludeShipping", Utils.ToDBValue(includeShipping)),
                new SqlParameter("@ShippingValue", Utils.ToDBValue(shippingValue))
            };

            db.QuerySP("CURATOR_SaveMemberPricing", parameters);

            return db.DBException;
        }

        public static Exception SaveProductTags(int storeID, string productIDList, string productTags, bool defaultProductTags, int modifiedBy)
        {
            string tags = string.Join(",", productTags.Split(',').ToList().FindAll(x => !string.IsNullOrEmpty(x)));

            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@ProductIDList", Utils.ToDBValue(productIDList)),
                new SqlParameter("@ProductTags", Utils.ToDBValue(tags)),
                new SqlParameter("@DefaultProductTags", Utils.ToDBValue(defaultProductTags)),
                new SqlParameter("@ModifiedBy", Utils.ToDBValue(modifiedBy))
            };

            db.QuerySP("CURATOR_SaveProductTags", parameters);

            return db.DBException;
        }

        public static List<TagInfo> GetProductTags(int storeID)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID",Utils.ToDBValue(storeID))

            };
            var dt = db.QuerySP("CURATOR_GetProductTags", parameters);

            return (from dr in dt.AsEnumerable() select new TagInfo(dr)).OrderBy(x => x.ProductTags).ToList();
        }

        public static List<TagInfo> GetTagsforProduct(int storeID, int productID)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
          {
                new SqlParameter("@StoreID",Utils.ToDBValue(storeID)),
                 new SqlParameter("@ProductID", Utils.ToDBValue(productID)),
            };
            var dt = db.QuerySP("CURATOR_GetTagsforProduct", parameters);

            return (from dr in dt.AsEnumerable() select new TagInfo(dr)).ToList();
        }

        public static Exception UpdateMemberProducts(int storeID, string allKeys, string productIDs)
        {
            string all = Regex.Replace(allKeys, "[^0-9,]", string.Empty);

            var sqlParameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@AllKeys", Utils.ToDBValue(all)),
                new SqlParameter("@ProductIDs", Utils.ToDBValue(productIDs))
            };

            var db = new DB(App.ProductsDBConn);

            db.QuerySP("CURATOR_UpdateMemberProducts", sqlParameters);

            return db.DBException;
        }

        #endregion

        #region Trade services
        public static List<TradeServiceInfo> GetTradeServicesProducts(int storeID, int? categoryKey, string sortBy, string sortDirection)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID",Utils.ToDBValue(storeID)),
                new SqlParameter("@CategoryKey",Utils.ToDBValue(categoryKey.HasValue?  Utils.ToDBValue(categoryKey.Value) : DBNull.Value)),
                new SqlParameter("@SortBy", Utils.ToDBValue(sortBy)),
                new SqlParameter("@SortDirection", Utils.ToDBValue(sortDirection))
            };
            var dt = db.QuerySP("CURATOR_GetTradeServicesProducts", parameters);

            return (from dr in dt.AsEnumerable() select new TradeServiceInfo(dr)).OrderBy(x => x.ShortDescription).ToList();
        }
        #endregion

        #region Member Stores

        public static List<StoreInfo> GetAllMemberStoresList(int userID)
        {
            return GetMemberStoreList(userID, "ALL");
        }

        public static List<StoreInfo> GetEcommerceMemberStoresList(int userID)
        {
            return GetMemberStoreList(userID, "ECOMMERCE");
        }

        private static List<StoreInfo> GetMemberStoreList(int userID, string type)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@UserID",Utils.ToDBValue(userID)),
                new SqlParameter("@Type",Utils.ToDBValue(type))
            };

            var dt = db.QuerySP("CURATOR_GetMemberStoreList", parameters);

            return (from dr in dt.AsEnumerable() select new StoreInfo(dr)).ToList();
        }

        #endregion
    }
}
