using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class VendorInfo
    {
        public Guid VendorID { get; set; }

        public string VendorName { get; set; }

        public int? MagentoID { get; set; }

        public bool MagentoEnabled { get; set; }

        public string NetSuiteInternalID { get; set; }

        public string NetSuiteEntityID { get; set; }

        public string NetSuiteCode { get; set; }

        public string NetSuiteName { get; set; }

        public bool NetSuiteEnabled { get; set; }

        public VendorInfo()
        {
        }

        public VendorInfo(DataRow dr)
        {
            VendorID = Utils.FromDBValue<Guid>(dr["VendorID"]);
            VendorName = Utils.FromDBValue<string>(dr["VendorName"]);
            MagentoID = Utils.FromDBValue<int?>(dr["MagentoID"]);
            MagentoEnabled = Utils.FromDBValue<bool>(dr["MagentoEnabled"]);
            NetSuiteInternalID = Utils.FromDBValue<string>(dr["NetSuiteInternalID"]);
            NetSuiteEntityID = Utils.FromDBValue<string>(dr["NetSuiteEntityID"]);
            NetSuiteCode = Utils.FromDBValue<string>(dr["NetSuiteCode"]);
            NetSuiteName = Utils.FromDBValue<string>(dr["NetSuiteName"]);
            NetSuiteEnabled = Utils.FromDBValue<bool>(dr["NetSuiteEnabled"]);
        }
    }
}
