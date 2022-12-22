using LeadingEdge.Curator.Core;
using System;
using System.Collections.Generic;


namespace LeadingEdge.Curator.Web.Products.Models
{
    public class ProductBrandRestrictionsList : BaseModel
    {
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public List<BrandInfo> Brands { get; set; }
        public int BrandKey { get; set; }
        public List<CatalogInfo> Catalogs { get; set; }

        public ProductBrandRestrictionsList()
        {
            Brands = new List<BrandInfo>();
            Catalogs = new List<CatalogInfo>();
        }
    }
}