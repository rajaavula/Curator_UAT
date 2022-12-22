using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class ProductsListEdit : BaseModel
    {
        public int FeedKey { get; set; }
        public int? BrandKey { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public List<FeedInfo> Feeds { get; set; }
        public List<BrandBySourceInfo> BrandsBySource { get; set; }
        public List<BrandInfo> Brands { get; set; }
        public List<CategoryInfo> Categories { get; set; }

        public ComboBoxModel BrandModel { get; set; }
        public List<ProductInfo> Products { get; set; }

        public ProductsListEdit()
        {
            Feeds = new List<FeedInfo>();
            BrandsBySource = new List<BrandBySourceInfo>();
            Brands = new List<BrandInfo>();
            Categories = new List<CategoryInfo>();
            BrandModel = new ComboBoxModel();
            Products = new List<ProductInfo>();
        }
    }
}
