using Cortex.Utilities;
using System;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public sealed class CatalogInfo
    {
        public int CatalogID { get; set; }
        public string Catalog { get; set; }

        public CatalogInfo() { }

        public CatalogInfo(DataRow dr)
        {
            CatalogID = Utils.FromDBValue<int>(dr["CatalogID"]);
            Catalog = Utils.FromDBValue<string>(dr["Catalog"]);
        }
    }
}