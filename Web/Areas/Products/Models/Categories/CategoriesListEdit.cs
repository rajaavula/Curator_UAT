using LeadingEdge.Curator.Core;
using System.Collections.Generic;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class CategoriesListEdit : BaseModel
    {
        public List<CategoryInfo> Categories { get; set; }
        public ComboBoxModel ParentCategoryModel { get; set; }

        public CategoriesListEdit()
        {
            Categories = new List<CategoryInfo>();
            ParentCategoryModel = new ComboBoxModel();
        }
    }
}