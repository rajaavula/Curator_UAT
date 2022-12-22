using Cortex.Utilities;
using System;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public sealed class BrandBySourceInfo
    {
        public int? BrandKey { get; set; }
        public string BrandName { get; set; }
        public string MainSource { get; set; }

        public BrandBySourceInfo() { }

        public BrandBySourceInfo(DataRow dr)
        {
            BrandKey = Utils.FromDBValue<int>(dr["BrandKey"]);
            BrandName = Utils.FromDBValue<string>(dr["BrandName"]);
            MainSource = Utils.FromDBValue<string>(dr["MainSource"]);
        }
    }
}
