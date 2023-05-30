using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class CategoryManager
    {
        #region Category

        public static List<CategoryInfo> GetCategories()
        {
            var db = new DB(App.ProductsDBConn);
            var dt = db.QuerySP("CURATOR_GetCategories");

            return (from dr in dt.AsEnumerable() select new CategoryInfo(dr)).OrderBy(x => x.Name).ToList();
        }

        public static List<ValueDescription> GetCategoriesList(bool nullOption)
        {
            var list = new List<ValueDescription>();

            if (nullOption) list.Add(new ValueDescription(null, null));

            var categories = GetCategories().OrderBy(x => x.CategoryPath).ToList();

            list.AddRange(from c in categories select new ValueDescription(c.CategoryKey, c.CategoryPath));

            return list;
        }

        public static Exception Save(int? categoryKey, CategoryInfo info)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CategoryKey", Utils.ToDBValue(categoryKey)),
                new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
                new SqlParameter("@Description", Utils.ToDBValue(info.Description)),
                new SqlParameter("@ParentKey", Utils.ToDBValue(info.ParentKey))
            };

            DataTable dt = db.QuerySP("CURATOR_SaveCategory", parameters);

            if (db.Success == false) return db.DBException;

            if (categoryKey == null)
            {
                if (db.RowCount != 1) return new Exception("Unable to get new category key");
                info.CategoryKey = Utils.FromDBValue<int>(dt.Rows[0][0]);
            }

            return db.DBException;
        }

        public static Exception Delete(int categoryKey)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CategoryKey", Utils.ToDBValue(categoryKey))
            };

            db.QuerySP("CURATOR_DeleteCategory", parameters);

            return db.DBException;
        }

        #endregion

        #region Member Categories

        public static List<CategoryInfo> GetMemberCategories(int storeID)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID))
            };
            var dt = db.QuerySP("CURATOR_GetMemberCategories", parameters);

            return (from dr in dt.AsEnumerable() select new CategoryInfo(dr)).OrderBy(x => x.Name).ToList();
        }

        public static List<MemberCategoryByFeedInfo> GetMemberCategoriesByFeed(int storeID, string selectedFeeds)

        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@SelectedFeeds", Utils.ToDBValue(selectedFeeds))
            };
            var dt = db.QuerySP("CURATOR_GetMemberCategoriesByFeed", parameters);

            return (from dr in dt.AsEnumerable() select new MemberCategoryByFeedInfo(dr)).ToList();
        }

        public static Exception UpdateMemberCategories(int storeID, string allKeys, string categoryKeys)
        {
            string all = Regex.Replace(allKeys, "[^0-9,]", string.Empty);

            var sqlParameters = new[]
            {
                new SqlParameter("@StoreID", Utils.ToDBValue(storeID)),
                new SqlParameter("@AllKeys", Utils.ToDBValue(all)),
                new SqlParameter("@CategoryKeys", Utils.ToDBValue(categoryKeys))
            };

            var db = new DB(App.ProductsDBConn);

            db.QuerySP("CURATOR_UpdateMemberCategories", sqlParameters);

            return db.DBException;
        }

        #endregion

        #region Category Mappings

        public static List<CategoryMappingInfo> GetCategoryMappings(int? feedID)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@FeedID", Utils.ToDBValue(feedID))
            };

            var dt = db.QuerySP("CURATOR_GetCategoryMappings", parameters);

            if (db.Success == false)
            {
                Log.Error(db.DBException);
                return null;
            }

            return new List<CategoryMappingInfo>(from DataRow dr in dt.Rows select new CategoryMappingInfo(dr));
        }

        public static CategoryMappingSummaryInfo GetCategoryMappingSummary(int categoryMappingID)
        {
            var summaries = GetCategoryMappingSummaries(categoryMappingID, "MAPPING");

            return summaries?.FirstOrDefault();
        }

        public static List<CategoryMappingSummaryInfo> GetCategoryMappingSummaries(int? categoryMappingID, string type)
        {
            var db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CategoryMappingID", Utils.ToDBValue(categoryMappingID)),
                new SqlParameter("@Type", Utils.ToDBValue(type))
            };

            var dt = db.QuerySP("CURATOR_GetCategoryMappingSummaries", parameters);

            if (db.Success == false)
            {
                Log.Error(db.DBException);
                return null;
            }

            return new List<CategoryMappingSummaryInfo>(from DataRow dr in dt.Rows select new CategoryMappingSummaryInfo(dr));
        }

        public static Exception UpdateCategoryMapping(CategoryMappingInfo info)
        {
            DB db = new DB(App.ProductsDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CategoryMappingID", Utils.ToDBValue(info.CategoryMappingID)),
                new SqlParameter("@CategoryID", Utils.ToDBValue(info.CategoryID))
            };

            db.QuerySP("CURATOR_SaveCategoryMapping", parameters);

            return db.DBException;
        }

        #endregion
    }
}