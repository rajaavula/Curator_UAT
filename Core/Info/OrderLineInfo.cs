using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class OrderLineInfo
    {
        public int? OrderLineKey { get; set; }
        public Guid OrderLineID { get; set; }
        public long? ShopifyLineID { get; set; }
        public int? OrderKey { get; set; }
        public long? ShopifyOrderID { get; set; }
        public long? VariantID { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public int? Quantity { get; set; }
        public long? ProductID { get; set; }
        public bool RequiresShipping { get; set; }
        public bool Taxable { get; set; }
        public decimal Grams { get; set; }
        public decimal Price { get; set; }
        public bool Refunded { get; set; }
        public string Barcode { get; set; }
        public string OriginalSupplier { get; set; }
        public string Currency { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCarrier { get; set; }
        public string TrackingNumber { get; set; }
        public string SupplierMessage { get; set; }
        public string MemberMessage { get; set; }
        public decimal CostPerSupplier { get; set; }
        public string Status { get; set; } // Raja will provide a list of this

        public OrderLineInfo() { }

        public OrderLineInfo(DataRow dr)
        {
            OrderLineKey = Utils.FromDBValue<int?>(dr["OrderLineKey"]);
            OrderKey = Utils.FromDBValue<int?>(dr["OrderKey"]);
            Title = Utils.FromDBValue<string>(dr["Title"]);
            SKU = Utils.FromDBValue<string>(dr["SKU"]);
            Quantity = Utils.FromDBValue<int>(dr["Quantity"]);
            RequiresShipping = Utils.FromDBValue<bool>(dr["RequiresShipping"]);
            Taxable = Utils.FromDBValue<bool>(dr["Taxable"]);
            Grams = Utils.FromDBValue<decimal>(dr["Grams"]);
            Price = Utils.FromDBValue<decimal>(dr["Price"]);
            Refunded = Utils.FromDBValue<bool>(dr["Refunded"]);
        }
    }
}