using System;

namespace LeadingEdge.Curator.Web.Products.Models
{
    public class ERPIntegrationsListEdit : BaseModel
    {
        public Guid VendorID { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public ERPIntegrationsListEdit()
        {
        }
    }
}
