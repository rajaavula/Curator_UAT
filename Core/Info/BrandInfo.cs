using Cortex.Utilities;
using System;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public sealed class BrandInfo
    {
        public int? BrandKey { get; set; }      // Nullable for use in dropdown lists only. BrandID in DB will never be null
        public string BrandName { get; set; }

        public BrandInfo() { }

        public BrandInfo(DataRow dr)
        {
            BrandKey = Utils.FromDBValue<int?>(dr["BrandKey"]);
            BrandName = Utils.FromDBValue<string>(dr["BrandName"]);
        }
    }
}
