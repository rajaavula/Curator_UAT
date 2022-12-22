using System.Collections.Generic;
using DevExpress.Web.Mvc;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class CategoryMappingsListEdit : BaseModel
    {
        public int? FeedID { get; set; }

        public int? Products { get; set; }

        public List<ValueDescription> Feeds { get; set; }
        public List<ValueDescription> Categories { get; set; }

        public PopupControlSettings ConfirmPopupSettings { get; set; }

        public CategoryMappingsListEdit()
        {
            Feeds = new List<ValueDescription>();
            Categories = new List<ValueDescription>();

            ConfirmPopupSettings = new PopupControlSettings();
        }
    }
}