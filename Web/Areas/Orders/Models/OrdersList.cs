using System;
using Cortex.Utilities;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Orders.Models
{
    public class OrdersList : BaseModel
    {
        public bool CanEdit { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int SelectedDateRange { get; set; }
        public string SelectedStores { get; set; }
        public int OrderStatusID { get; set; }
        public bool FailedEDIDelivery { get; set;} 
        public bool IncompleteItemLines { get; set;} 
        public List<FromToDate> DateRanges { get; set; }
        public CheckComboModel StoresModel { get; set; }
        public List<StoreInfo> Stores { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public List<OrderInfo> Orders { get; set; }
        public List<OrderLineList> OrderDetails { get; set; }

        public OrdersList()
        {
            DateRanges = new List<FromToDate>();
            Stores = new List<StoreInfo>();
            Orders = new List<OrderInfo>();
            OrderStatuses = new List<OrderStatus>();
            OrderDetails = new List<OrderLineList>();
        }
    }
}
