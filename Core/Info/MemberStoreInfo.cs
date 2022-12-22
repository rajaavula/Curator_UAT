using Cortex.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public  class MemberStoreInfo
    {
        public string MemberStores { get; set; }
       
        public MemberStoreInfo() { }

        public MemberStoreInfo(DataRow dr)
        {
            MemberStores = Utils.FromDBValue<string>(dr["MemberStores"]);
        }
    }
}
