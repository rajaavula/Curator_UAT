using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class SalesOrderLineInfo
    {
        public int SalesOrderLineID { get; set; }
        public int SalesOrderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SourceLineID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int ProductID { get; set; }
        public string SupplierPartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public string ShortDescription { get; set; }
        public int SalesOrderLineStatusID { get; set; }
        public int Quantity { get; set; }
        public long WeightGrams { get; set; }
        public bool RequiresShipping { get; set; }
        public int ShippingAddressID { get; set; }
        public string ShippingAddressCompany { get; set; }
        public string ShippingAddressFirstName { get; set; }
        public string ShippingAddressLastName { get; set; }
        public string ShippingAddressStreet1 { get; set; }
        public string ShippingAddressStreet2 { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressRegion { get; set; }
        public string ShippingAddressZip { get; set; }
        public string ShippingAddressCountry { get; set; }
        public string ShippingAddressEmail { get; set; }
        public string ShippingAddressPhone { get; set; }
        public string DeliveryInstructions { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string InternalNotes { get; set; }
        public int? PurchaseOrderID { get; set; }
        public string PurchaseResult { get; set; }
        public List<SupplierLineInfo> SelectableSuppliers { get; set; }
        public int? SupplierID {get; set;}
        public string OriginalSupplier { get; set; }
        public decimal RRPEx { get; set; }
        public decimal ResellerBuyEx { get; set; }
        public decimal ResellerBuyInc { get; set; }

        public string ShippingAddressFormatted
        {
            get
            {
                var list = new List<string> {   ShippingAddressFirstName + " " + ShippingAddressLastName, ShippingAddressEmail, ShippingAddressPhone,
                                                ShippingAddressStreet1, ShippingAddressStreet2, ShippingAddressCity, ShippingAddressRegion, 
                                                ShippingAddressZip, ShippingAddressCountry  };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }

        public string ShippingAddress
        {
            get
            {
                var list = new List<string> {  ShippingAddressStreet1, ShippingAddressStreet2, ShippingAddressCity, ShippingAddressRegion,
                                                ShippingAddressZip, ShippingAddressCountry  };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }

        public SalesOrderLineInfo() { }

        public SalesOrderLineInfo(DataRow dr)
        {
            SalesOrderLineID = Utils.FromDBValue<int>(dr["SalesOrderLineID"]);
            SalesOrderID = Utils.FromDBValue<int>(dr["SalesOrderID"]);
            CreatedDate = Utils.FromDBValue<DateTime>(dr["CreatedDate"]);
            ModifiedDate = Utils.FromDBValue<DateTime>(dr["ModifiedDate"]);
            SourceLineID = Utils.FromDBValue<string>(dr["SourceLineID"]);
            CurrencyID = Utils.FromDBValue<int>(dr["CurrencyID"]);
            CurrencyCode = Utils.FromDBValue<string>(dr["CurrencyCode"]);
            CurrencyName = Utils.FromDBValue<string>(dr["CurrencyName"]);
            ProductID = Utils.FromDBValue<int>(dr["ProductID"]);
            SupplierPartNumber = Utils.FromDBValue<string>(dr["SupplierPartNumber"]);
            ManufacturerPartNumber = Utils.FromDBValue<string>(dr["ManufacturerPartNumber"]);
            ManufacturerID = Utils.FromDBValue<int>(dr["ManufacturerID"]);
            ManufacturerName = Utils.FromDBValue<string>(dr["ManufacturerName"]);
            ShortDescription = Utils.FromDBValue<string>(dr["ShortDescription"]);
            SalesOrderLineStatusID = Utils.FromDBValue<int>(dr["SalesOrderLineStatusID"]);
            Quantity = Utils.FromDBValue<int>(dr["Quantity"]);
            WeightGrams = Utils.FromDBValue<long>(dr["WeightGrams"]);
            RequiresShipping = Utils.FromDBValue<bool>(dr["RequiresShipping"]);
            ShippingAddressID = Utils.FromDBValue<int>(dr["ShippingAddressID"]);
            ShippingAddressCompany = Utils.FromDBValue<string>(dr["ShippingAddressCompany"]);
            ShippingAddressFirstName = Utils.FromDBValue<string>(dr["ShippingAddressFirstName"]);
            ShippingAddressLastName = Utils.FromDBValue<string>(dr["ShippingAddressLastName"]);
            ShippingAddressStreet1 = Utils.FromDBValue<string>(dr["ShippingAddressStreet1"]);
            ShippingAddressStreet2 = Utils.FromDBValue<string>(dr["ShippingAddressStreet2"]);
            ShippingAddressCity = Utils.FromDBValue<string>(dr["ShippingAddressCity"]);
            ShippingAddressRegion = Utils.FromDBValue<string>(dr["ShippingAddressRegion"]);
            ShippingAddressZip = Utils.FromDBValue<string>(dr["ShippingAddressZip"]);
            ShippingAddressCountry = Utils.FromDBValue<string>(dr["ShippingAddressCountry"]);
            ShippingAddressEmail = Utils.FromDBValue<string>(dr["ShippingAddressEmail"]);
            ShippingAddressPhone = Utils.FromDBValue<string>(dr["ShippingAddressPhone"]);
            DeliveryInstructions = Utils.FromDBValue<string>(dr["DeliveryInstructions"]);
            SubtotalAmount = Utils.FromDBValue<decimal>(dr["SubtotalAmount"]);
            ShippingAmount = Utils.FromDBValue<decimal>(dr["ShippingAmount"]);
            DiscountAmount = Utils.FromDBValue<decimal>(dr["DiscountAmount"]);
            TaxAmount = Utils.FromDBValue<decimal>(dr["TaxAmount"]);
            TotalAmount = Utils.FromDBValue<decimal>(dr["TotalAmount"]);
            InternalNotes = Utils.FromDBValue<string>(dr["InternalNotes"]);
            PurchaseOrderID = Utils.FromDBValue<int?>(dr["PurchaseOrderID"]);
            PurchaseResult = Utils.FromDBValue<string>(dr["PurchaseResult"]);
            SupplierID = Utils.FromDBValue<int?>(dr["SupplierID"]);
            OriginalSupplier = Utils.FromDBValue<string>(dr["OriginalSupplier"]);
            RRPEx = Utils.FromDBValue<decimal>(dr["RRPEx"]);
            ResellerBuyEx = Utils.FromDBValue<decimal>(dr["ResellerBuyEx"]);
            ResellerBuyInc = Utils.FromDBValue<decimal>(dr["ResellerBuyInc"]);
        }
    }
}