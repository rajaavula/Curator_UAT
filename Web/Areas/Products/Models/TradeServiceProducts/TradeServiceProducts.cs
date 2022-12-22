using System;
using System.Collections.Generic;
using System.Linq;
using Cortex.Utilities;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class TradeServiceProducts : BaseModel
    {

        public int? CategoryKey { get; set; }
        
        public List<ValueDescription> Categories { get; set; }

        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        
        public List<TradeServiceInfo> TradeServiceProductsList { get; set; }

        public TradeServiceProducts()
        {


           

            Categories = new List<ValueDescription>();
            TradeServiceProductsList = new List<TradeServiceInfo>();
        }
    }
}
