using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class SalesOrderInfo
    {
        public int SalesOrderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SourceOrderID { get; set; }
        public int StoreID { get; set; }
        public int CurrencyID { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime FulfillDate { get; set; }
        public DateTime CancelDate { get; set; }
        public int SalesOrderPaymentStatusID { get; set; }
        public int SalesOrderFulfillmentStatusID { get; set; }
        public int SalesOrderPurchaseStatusID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCompany { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerNote { get; set; }
        public int BillingAddressID { get; set; }
        public string BillingAddressCompany { get; set; }
        public string BillingAddressFirstName { get; set; }
        public string BillingAddressLastName { get; set; }
        public string BillingAddressStreet1 { get; set; }
        public string BillingAddressStreet2 { get; set; }
        public string BillingAddressCity { get; set; }
        public string BillingAddressRegion { get; set; }
        public string BillingAddressZip { get; set; }
        public string BillingAddressCountry { get; set; }
        public string BillingAddressEmail { get; set; }
        public string BillingAddressPhone { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Tags { get; set; }
        public int TotalItems { get; set; }
        public long TotalWeightGrams { get; set; }
        public string PaymentMethod { get; set; }
        public string CustomerIPAddress { get; set; }
        public bool CustomerIsNew { get; set; }
        public int FraudScore { get; set; }
        public bool ShippingAddressChecked { get; set; }
        public bool CustomerIPAddressChecked { get; set; }
        public bool FraudChecked { get; set; }
        public DateTime PurchaseErrorDate { get; set; }
        public DateTime NetSuiteUpdateDate { get; set; }
        public string NetSuiteUpdateResult { get; set; }
        public string StoreName { get; set; }

        public string CustomerName
        {
            get
            {
                return string.Concat(CustomerFirstName, " ", CustomerLastName);
            }
        }

        public string BillingAddressFormatted
        {
            get
            {
                var list = new List<string> {   BillingAddressFirstName + " " +  BillingAddressLastName, BillingAddressPhone, BillingAddressEmail,
                                                BillingAddressStreet1, BillingAddressStreet2, BillingAddressCity, BillingAddressRegion,
                                                BillingAddressZip, BillingAddressCountry, PaymentMethod };
                list.RemoveAll(x => x == null);

                if (list.Count == 0) return string.Empty;

                return string.Join(Environment.NewLine, list);
            }
        }

        public SalesOrderInfo() { }

        public SalesOrderInfo(DataRow dr)
        {
            SalesOrderID = Utils.FromDBValue<int>(dr["SalesOrderID"]);
            CreatedDate = Utils.FromDBValue<DateTime>(dr["CreatedDate"]);
            ModifiedDate = Utils.FromDBValue<DateTime>(dr["ModifiedDate"]);
            SourceOrderID = Utils.FromDBValue<string>(dr["SourceOrderID"]);
            StoreID = Utils.FromDBValue<int>(dr["StoreID"]);
            CurrencyID = Utils.FromDBValue<int>(dr["CurrencyID"]);
            SalesOrderNumber = Utils.FromDBValue<string>(dr["SalesOrderNumber"]);
            PurchaseOrderNumber = Utils.FromDBValue<string>(dr["PurchaseOrderNumber"]);
            OrderDate = Utils.FromDBValue<DateTime>(dr["OrderDate"]);
            FulfillDate = Utils.FromDBValue<DateTime>(dr["FulfillDate"]);
            CancelDate = Utils.FromDBValue<DateTime>(dr["CancelDate"]);
            SalesOrderPaymentStatusID = Utils.FromDBValue<int>(dr["SalesOrderPaymentStatusID"]);
            SalesOrderFulfillmentStatusID = Utils.FromDBValue<int>(dr["SalesOrderFulfillmentStatusID"]);
            SalesOrderPurchaseStatusID = Utils.FromDBValue<int>(dr["SalesOrderPurchaseStatusID"]);
            CustomerID = Utils.FromDBValue<int>(dr["CustomerID"]);
            CustomerCompany = Utils.FromDBValue<string>(dr["CustomerCompany"]);
            CustomerFirstName = Utils.FromDBValue<string>(dr["CustomerFirstName"]);
            CustomerLastName = Utils.FromDBValue<string>(dr["CustomerLastName"]);
            CustomerEmail = Utils.FromDBValue<string>(dr["CustomerEmail"]);
            CustomerPhone = Utils.FromDBValue<string>(dr["CustomerPhone"]);
            CustomerNote = Utils.FromDBValue<string>(dr["CustomerNote"]);
            BillingAddressID = Utils.FromDBValue<int>(dr["BillingAddressID"]);
            BillingAddressCompany = Utils.FromDBValue<string>(dr["BillingAddressCompany"]);
            BillingAddressFirstName = Utils.FromDBValue<string>(dr["BillingAddressFirstName"]);
            BillingAddressLastName = Utils.FromDBValue<string>(dr["BillingAddressLastName"]);
            BillingAddressStreet1 = Utils.FromDBValue<string>(dr["BillingAddressStreet1"]);
            BillingAddressStreet2 = Utils.FromDBValue<string>(dr["BillingAddressStreet2"]);
            BillingAddressCity = Utils.FromDBValue<string>(dr["BillingAddressCity"]);
            BillingAddressRegion = Utils.FromDBValue<string>(dr["BillingAddressRegion"]);
            BillingAddressZip = Utils.FromDBValue<string>(dr["BillingAddressZip"]);
            BillingAddressCountry = Utils.FromDBValue<string>(dr["BillingAddressCountry"]);
            BillingAddressEmail = Utils.FromDBValue<string>(dr["BillingAddressEmail"]);
            BillingAddressPhone = Utils.FromDBValue<string>(dr["BillingAddressPhone"]);
            SubtotalAmount = Utils.FromDBValue<decimal>(dr["SubtotalAmount"]);
            ShippingAmount = Utils.FromDBValue<decimal>(dr["ShippingAmount"]);
            DiscountAmount = Utils.FromDBValue<decimal>(dr["DiscountAmount"]);
            TaxAmount = Utils.FromDBValue<decimal>(dr["TaxAmount"]);
            TotalAmount = Utils.FromDBValue<decimal>(dr["TotalAmount"]);
            Tags = Utils.FromDBValue<string>(dr["Tags"]);
            TotalItems = Utils.FromDBValue<int>(dr["TotalItems"]);
            TotalWeightGrams = Utils.FromDBValue<long>(dr["TotalWeightGrams"]);
            PaymentMethod = Utils.FromDBValue<string>(dr["PaymentMethod"]);
            CustomerIPAddress = Utils.FromDBValue<string>(dr["CustomerIPAddress"]);
            CustomerIsNew = Utils.FromDBValue<bool>(dr["CustomerIsNew"]);
            FraudScore = Utils.FromDBValue<int>(dr["FraudScore"]);
            ShippingAddressChecked = Utils.FromDBValue<bool>(dr["ShippingAddressChecked"]);
            CustomerIPAddressChecked = Utils.FromDBValue<bool>(dr["CustomerIPAddressChecked"]);
            FraudChecked = Utils.FromDBValue<bool>(dr["FraudChecked"]);
            PurchaseErrorDate = Utils.FromDBValue<DateTime>(dr["PurchaseErrorDate"]);
            NetSuiteUpdateDate = Utils.FromDBValue<DateTime>(dr["NetSuiteUpdateDate"]);
            NetSuiteUpdateResult = Utils.FromDBValue<string>(dr["NetSuiteUpdateResult"]);
            StoreName = Utils.FromDBValue<string>(dr["StoreName"]);
        }

        public static void UpdateFraudCheckFields(SalesOrderInfo order, FraudCheckInfo fraudCheck)
        {
            order.PaymentMethod = fraudCheck.PaymentMethod;
            order.FraudScore = fraudCheck.FraudScore;
            order.CustomerIsNew = fraudCheck.CustomerIsNew;
            order.ShippingAddressChecked = fraudCheck.ShippingAddressChecked;
            order.CustomerIPAddressChecked = fraudCheck.CustomerIPAddressChecked;
            order.FraudChecked = fraudCheck.FraudChecked;
        }
    }
}