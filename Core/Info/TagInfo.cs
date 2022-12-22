using Cortex.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public  class TagInfo
    {
        
        public string ProductTags { get; set; }
       
        public TagInfo() { }

        public TagInfo(DataRow dr)
        {
            ProductTags = Utils.FromDBValue<string>(dr["ProductTag"]);

        }
    }
}
