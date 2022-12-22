using System;
using System.Collections.Generic;
using System.Linq;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class ProductDashboard : BaseModel
    {
        public string SupplierName { get; set; }
        public int UpdatedRecordCount { get; set; }
        public int CreatedRecordCount { get; set; }
        public int DeactivatedRecordCount { get; set; }
        public DateTime LastUpdatedDateOrTime { get; set; }
        public int ItemcountUpdatetoNetSuite { get; set; }
        public List<ProductDashboardInfo> SupplierUpdates { get;  set; }
    }
}