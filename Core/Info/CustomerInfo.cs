using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class CustomerInfo
    {
        public int CustomerID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string Region { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal EstimatedMonthlySpend { get; set; }
        public int? AssignedToUserID { get; set; }
        public string AssignedTo { get; set; }
        public int? AssignedToRepUserID { get; set; }
        public string AssignedToRep { get; set; }
        public bool Enabled { get; set; }
        public string Notes { get; set; }

        public CustomerInfo() { }

        public CustomerInfo(DataRow dr)
        {
            CustomerID = Utils.FromDBValue<int>(dr["CustomerID"]);
            CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
            RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
            CreatedDate = U.GetLocalTime(Utils.FromDBValue<DateTime>(dr["CreatedDate"])); 
            UpdatedDate = U.GetLocalTime(Utils.FromDBValue<DateTime>(dr["UpdatedDate"]));
            CustomerName = Utils.FromDBValue<string>(dr["CustomerName"]);
            CustomerPhone = Utils.FromDBValue<string>(dr["CustomerPhone"]);
            CustomerEmail = Utils.FromDBValue<string>(dr["CustomerEmail"]);
            CustomerEmail = Utils.FromDBValue<string>(dr["CustomerEmail"]);
            ContactName = Utils.FromDBValue<string>(dr["ContactName"]);
            ContactNumber = Utils.FromDBValue<string>(dr["ContactNumber"]);
            Region = Utils.FromDBValue<string>(dr["Region"]);
            CurrencySymbol = Utils.FromDBValue<string>(dr["CurrencySymbol"]);
            EstimatedMonthlySpend = Utils.FromDBValue<decimal>(dr["EstimatedMonthlySpend"]);
            AssignedToUserID = Utils.FromDBValue<int?>(dr["AssignedToUserID"]);
            AssignedTo = Utils.FromDBValue<string>(dr["AssignedTo"]);
            AssignedToRepUserID = Utils.FromDBValue<int?>(dr["AssignedToRepUserID"]);
            AssignedToRep = Utils.FromDBValue<string>(dr["AssignedToRep"]);
            Enabled = Utils.FromDBValue<bool>(dr["Enabled"]);
            Notes = Utils.FromDBValue<string>(dr["Notes"]);
        }
    }
}
