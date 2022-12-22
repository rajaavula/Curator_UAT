using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class ProductDashboardInfo
    {
        
        public string SupplierName { get; set; }
        public int UpdatedRecordCount { get; set; }
        public int CreatedRecordCount { get; set; }
        public int DeactivatedRecordCount { get; set; }
        public DateTime PMLastUpdated { get; set; }
        public int NetSuiteUpdates { get; set; }
        public DateTime NSLastUpdated { get; set; }

        public ProductDashboardInfo() { }

        public ProductDashboardInfo(DataRow dr)
        {
            SupplierName = Utils.FromDBValue<string>(dr["SupplierName"]);
            UpdatedRecordCount = Utils.FromDBValue<int>(dr["UpdatedRecordCount"]);
            CreatedRecordCount = Utils.FromDBValue<int>(dr["CreatedRecordCount"]);
            DeactivatedRecordCount = Utils.FromDBValue<int>(dr["DeactivatedRecordCount"]);
            PMLastUpdated = Utils.FromDBValue<DateTime>(dr["PMLastUpdated"]);
            NetSuiteUpdates = Utils.FromDBValue<int>(dr["NetSuiteUpdates"]);
            NSLastUpdated = Utils.FromDBValue<DateTime>(dr["NSLastUpdated"]);
        }

    }
}
