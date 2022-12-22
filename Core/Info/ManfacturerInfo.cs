using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class ManufacturerInfo
    {
        public int ManufacturerKey { get; set; }
        public string ManufacturerName { get; set; }

        public ManufacturerInfo()
        {
        }

        public ManufacturerInfo(DataRow dr)
        {
            ManufacturerKey = Utils.FromDBValue<int>(dr["ManufacturerKey"]);
            ManufacturerName = Utils.FromDBValue<string>(dr["ManufacturerName"]);
        }
    }
}
