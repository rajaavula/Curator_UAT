using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Orders.Models
{
    public class OrderLineList
    {
        public bool CanEdit { get; set; }
        public int SalesOrderID { get; set; }
        public string ShippingAddress { get; set; }
        public SalesOrderInfo Order { get; set; }
        public List<SalesOrderLineInfo> OrderLines { get; set; }

        public OrderLineList()
        {
            Order = new SalesOrderInfo();
            OrderLines = new List<SalesOrderLineInfo>();
        }
    }
}