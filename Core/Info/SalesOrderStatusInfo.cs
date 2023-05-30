using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class SalesOrderStatusInfo
    {
        public int SalesOrderLineStatusID { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }

        public SalesOrderStatusInfo() { }

        public SalesOrderStatusInfo(DataRow dr)
        {
            SalesOrderLineStatusID = Utils.FromDBValue<int>(dr["SalesOrderLineStatusID"]);
            StatusName = Utils.FromDBValue<string>(dr["StatusName"]);
            StatusDescription = Utils.FromDBValue<string>(dr["StatusDescription"]);
        }
    }
}