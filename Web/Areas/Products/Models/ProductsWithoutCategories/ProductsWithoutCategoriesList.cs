using LeadingEdge.Curator.Core;
using System;
using System.Collections.Generic;


namespace LeadingEdge.Curator.Web.Products.Models
{
    public class ProductsWithoutCategoriesList : BaseModel
    {
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public List<CategoryInfo> Categories { get; set; }
        public int FeedKey { get; set; }
        public List<FeedInfo> Feeds { get; set; }

        public ProductsWithoutCategoriesList()
        {
            Categories = new List<CategoryInfo>();
            Feeds = new List<FeedInfo>();
        }
    }
}
