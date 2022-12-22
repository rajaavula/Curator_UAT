using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class StoreInfo
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreUrl { get; set; }
        public string StoreApiKey { get; set; }
        public string StorePassword { get; set; }
        public string StoreSharedSecret { get; set; }
        public long ShopifyID { get; set; }
        public string MemberStores { get; set; }
        public byte[] Logo { get; set; }

        public StoreInfo() { }

        public StoreInfo(DataRow dr)
        {
            if (dr.Table.Columns.Contains("StoreID")) StoreID = Utils.FromDBValue<int>(dr["StoreID"]);
            if (dr.Table.Columns.Contains("StoreName")) StoreName = Utils.FromDBValue<string>(dr["StoreName"]);
            if (dr.Table.Columns.Contains("StoreUrl")) StoreUrl = Utils.FromDBValue<string>(dr["StoreUrl"]);
            if (dr.Table.Columns.Contains("StoreApiKey")) StoreApiKey = Utils.FromDBValue<string>(dr["StoreApiKey"]);
            if (dr.Table.Columns.Contains("StorePassword")) StorePassword = Utils.FromDBValue<string>(dr["StorePassword"]);
            if (dr.Table.Columns.Contains("StoreSharedSecret")) StoreSharedSecret = Utils.FromDBValue<string>(dr["StoreSharedSecret"]);
            if (dr.Table.Columns.Contains("ShopifyID")) ShopifyID = Utils.FromDBValue<long>(dr["ShopifyID"]);
            if (dr.Table.Columns.Contains("MemberStores")) MemberStores = Utils.FromDBValue<string>(dr["MemberStores"]);
            if (dr.Table.Columns.Contains("Logo")) Logo = Utils.FromDBValue<byte[]>(dr["Logo"]);
        }
    }
}