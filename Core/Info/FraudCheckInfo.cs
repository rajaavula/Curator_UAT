using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class FraudCheckInfo
    {
        public string PaymentMethod { get; set; }
        public int FraudScore { get; set; }
        public bool CustomerIsNew { get; set; }
        public bool ShippingAddressChecked { get; set; }
        public bool CustomerIPAddressChecked { get; set; }
        public string IPAddress { get; set; }
        public bool FraudChecked { get; set; }

        public FraudCheckInfo() { }

        public FraudCheckInfo(SalesOrderInfo orderInfo)
        {
            PaymentMethod = orderInfo.PaymentMethod;
            FraudScore = orderInfo.FraudScore;
            CustomerIsNew = orderInfo.CustomerIsNew;
            ShippingAddressChecked = orderInfo.ShippingAddressChecked;
            CustomerIPAddressChecked = orderInfo.CustomerIPAddressChecked;
            IPAddress = orderInfo.CustomerIPAddress;
            FraudChecked = orderInfo.FraudChecked;
        }
    }
}
