using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class OrderInfo
    {
        public int? OrderKey { get; set; }
        public Guid? OrderID { get; set; }
        public long? ShopifyID { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Number { get; set; }
        public string Note { get; set; }
        public string Token { get; set; }
        public string Gateway { get; set; }
        public bool Test { get; set; } // Probably needs to be removed from the Orders table?
        public decimal? TotalPrice { get; set; }
        public decimal? SubtotalPrice { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalTax { get; set; }
        public bool TaxesIncluded { get; set; }
        public string Currency { get; set; }
        public string FinancialStatus { get; set; }
        public bool Confirmed { get; set; }
        public decimal? TotalDiscounts { get; set; }
        public decimal? TotalLineItemsPrice { get; set; }
        public string CartToken { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public long CustomerID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingAddressZip { get; set; }
        public string ShippingAddressPhone { get; set; }
        public string ShippingAddressFirstName { get; set; }
        public string ShippingAddressLastName { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressProvince { get; set; }
        public string ShippingAddressCountry { get; set; }
        public DateTime? LastImport { get; set; }
        public DateTime? LastExport { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string CustomerName
        {
            get
            {
                return string.Concat(CustomerFirstName, " ", CustomerLastName);
            }
        }
        public string ShippingAddressFormatted
        {
            get
            {
                var list = new List<string> { ShippingAddress, ShippingAddressCity, ShippingAddressZip, ShippingAddressProvince, ShippingAddressCountry };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }
        public string FullShippingAddressFormatted
        {
            get
            {
                var list = new List<string> { CustomerName, Phone, Email, ShippingAddress, ShippingAddressCity, ShippingAddressZip, ShippingAddressProvince, ShippingAddressCountry };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }
        public string BillingAddressFormatted
        {
            get
            {
                var list = new List<string> { CustomerName, Phone, Email, ShippingAddress, ShippingAddressCity, ShippingAddressZip, ShippingAddressProvince, ShippingAddressCountry, PaymentMethod };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }
        
        // Not in order table, might be an existing field so need to cross check when sp is done
        public string Member { get; set; } // StoreID maybe?
        public string Status { get; set; } // Raja will provide a list of this
        public string SourceStore { get; set; } // StoreName maybe?
        public decimal Shipping { get; set; }
        public int Quantity { get; set; }
        public string OrderNumber { get; set; } // Number? OrderKey? or a different field possibly?
        public bool IsCancelled { get; set; }
        public string PaymentStatus { get; set; } // FinancialStatus maybe?
        public string FufillmentStatus { get; set; }
        public DateTime? FulfilledDate { get; set; } // This might be the same as ClosedAt, need to cross check
        public string Tags { get; set; }
        public string DeliveryInstructions { get; set; } // Might need to add at item level, see spreadsheet for notes
        public string PaymentMethod { get; set; }
        public string IPAddress { get; set; }
        public string FraudScore { get; set; }
        public bool FraudCheckCompleted { get; set; }
        public bool NewCustomer { get; set; }
        public bool IPAddressChecked { get; set; }
        public bool ShippingAddressChecked { get; set; }
        public int InternalID { get; set; } // By InternalID could they just mean what we already use for the table? e.g. OrderID, OrderKey?
        public bool FailedEDIDelivery { get; set; }
        public bool FailedNetSuiteSync { get; set; }
        
        public OrderInfo() { }

        public OrderInfo(DataRow dr)
        {
            OrderKey = Utils.FromDBValue<int?>(dr["OrderKey"]);
            ShopifyID = Utils.FromDBValue<long>(dr["ShopifyID"]);
            ClosedAt = Utils.FromDBValue<DateTime>(dr["ClosedAt"]);
            CreatedAt = Utils.FromDBValue<DateTime>(dr["CreatedAt"]);
            UpdatedAt = Utils.FromDBValue<DateTime>(dr["UpdatedAt"]);
            Number = Utils.FromDBValue<int>(dr["Number"]);
            Note = Utils.FromDBValue<string>(dr["Note"]);
            TotalPrice = Utils.FromDBValue<decimal>(dr["TotalPrice"]);
            SubtotalPrice = Utils.FromDBValue<decimal>(dr["SubtotalPrice"]);
            TotalWeight = Utils.FromDBValue<decimal>(dr["TotalWeight"]);
            TotalTax = Utils.FromDBValue<decimal>(dr["TotalTax"]);
            TaxesIncluded = Utils.FromDBValue<bool>(dr["TaxesIncluded"]);
            Currency = Utils.FromDBValue<string>(dr["Currency"]);
            FinancialStatus = Utils.FromDBValue<string>(dr["FinancialStatus"]);
            Confirmed = Utils.FromDBValue<bool>(dr["Confirmed"]);
            TotalDiscounts = Utils.FromDBValue<decimal>(dr["TotalDiscounts"]);
            TotalLineItemsPrice = Utils.FromDBValue<decimal>(dr["TotalLineItemsPrice"]);
            CustomerFirstName = Utils.FromDBValue<string>(dr["CustomerFirstName"]);
            CustomerLastName = Utils.FromDBValue<string>(dr["CustomerLastName"]);
            CustomerID = Utils.FromDBValue<long>(dr["CustomerId"]);
            Email = Utils.FromDBValue<string>(dr["Email"]);
            Phone = Utils.FromDBValue<string>(dr["Phone"]);
            ShippingAddress = Utils.FromDBValue<string>(dr["ShippingAddress"]);
            ShippingAddressZip = Utils.FromDBValue<string>(dr["ShippingAddressZip"]);
            ShippingAddressPhone = Utils.FromDBValue<string>(dr["ShippingAddressPhone"]);
            ShippingAddressFirstName = Utils.FromDBValue<string>(dr["ShippingAddressFirstName"]);
            ShippingAddressLastName = Utils.FromDBValue<string>(dr["ShippingAddressLastName"]);
            ShippingAddressCity = Utils.FromDBValue<string>(dr["ShippingAddressCity"]);
            ShippingAddressProvince = Utils.FromDBValue<string>(dr["ShippingAddressProvince"]);
            ShippingAddressCountry = Utils.FromDBValue<string>(dr["ShippingAddressCountry"]);
            StoreID = Utils.FromDBValue<int>(dr["StoreID"]);
            StoreName = Utils.FromDBValue<string>(dr["StoreName"]);
        }
    }
}