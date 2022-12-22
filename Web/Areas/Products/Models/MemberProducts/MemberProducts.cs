using System;
using System.Collections.Generic;
using System.Linq;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class MemberProducts : BaseModel
    {
        public int FeedKey { get; set; }
        public int ProductID { get; set; }
        public List<FeedInfo> Feeds { get; set; }
        public List<CategoryInfo> Categories { get; set; }
        public List<MemberCategoryByFeedInfo> MemberCategoriesByFeed { get; set; }
        public List<MemberFeedByStoreInfo> MemberFeedsByStore { get; set; }
        public List<BrandByFeedInfo> BrandByFeed { get; set; }
        public string SelectedCategories { get; set; }
        public string SelectedBrands { get; set; }
        public ComboBoxModel MemberFeedModel { get; set; }
        public CheckComboModel MemberCategoryModel { get; set; }
        public CheckComboModel BrandModel { get; set; }
        public UploadControlSettings BulkPRImportUploadControlSettings { get; set;}
        public PopupControlSettings BulkPRImportPopUploadSettings { get; set; }
        public List<TagInfo> MemberProductTags { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public List<ProductInfo> ProductTags { get; set; }
        public List<TagInfo> ProductTag { get; set; }
        public List<StoreInfo> MemberStoreList { get; set; }
        public int StoreID { get; set; }
        public MemberProducts()
        {
            Feeds = Feeds = new List<FeedInfo>();
            Categories = new List<CategoryInfo>();
            MemberCategoriesByFeed = new List<MemberCategoryByFeedInfo>();
            MemberFeedsByStore = new List<MemberFeedByStoreInfo>();
            BrandByFeed = new List<BrandByFeedInfo>();
            MemberFeedModel = new ComboBoxModel();
            MemberCategoryModel = new CheckComboModel();
            BrandModel = new CheckComboModel();
            BulkPRImportUploadControlSettings = new UploadControlSettings();
            MemberProductTags = new List<TagInfo>();
            ProductTags = new List<ProductInfo>();
            ProductTag = new List<TagInfo>();
            MemberStoreList = new List<StoreInfo>();
        }
    }
}
