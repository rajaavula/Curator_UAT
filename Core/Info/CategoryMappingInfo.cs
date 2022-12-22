using Cortex.Utilities;
using System;
using System.Data;

namespace LeadingEdge.Curator.Core
{
    public sealed class CategoryMappingInfo
    {
        public int? CategoryMappingID { get; set; }
        public int FeedID { get; set; }
        public string FeedName { get; set; }
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public int? CategoryID { get; set; }
        public string Level1CategoryName { get; set; }
        public string Level2CategoryName { get; set; }
        public string Level3CategoryName { get; set; }
        public string Level4CategoryName { get; set; }
        public string Level5CategoryName { get; set; }

        public string CategoryPath
        {
            get
            {
                string path = Level1CategoryName;
                if (!string.IsNullOrEmpty(Level2CategoryName)) path += string.Concat(" / ", Level2CategoryName);
                if (!string.IsNullOrEmpty(Level3CategoryName)) path += string.Concat(" / ", Level3CategoryName);
                if (!string.IsNullOrEmpty(Level4CategoryName)) path += string.Concat(" / ", Level4CategoryName);
                if (!string.IsNullOrEmpty(Level5CategoryName)) path += string.Concat(" / ", Level5CategoryName);

                return path;
            }
        }

        public CategoryMappingInfo() { }

        public CategoryMappingInfo(DataRow dr)
        {
            CategoryMappingID = Utils.FromDBValue<int?>(dr["CategoryMappingID"]);
            FeedID = Utils.FromDBValue<int>(dr["FeedID"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            ManufacturerID = Utils.FromDBValue<int>(dr["ManufacturerID"]);
            ManufacturerName = Utils.FromDBValue<string>(dr["ManufacturerName"]);
            Category1 = Utils.FromDBValue<string>(dr["Category1"]);
            Category2 = Utils.FromDBValue<string>(dr["Category2"]);
            Category3 = Utils.FromDBValue<string>(dr["Category3"]);
            CategoryID = Utils.FromDBValue<int?>(dr["CategoryID"]);
            Level1CategoryName = Utils.FromDBValue<string>(dr["Level1CategoryName"]);
            Level2CategoryName = Utils.FromDBValue<string>(dr["Level2CategoryName"]);
            Level3CategoryName = Utils.FromDBValue<string>(dr["Level3CategoryName"]);
            Level4CategoryName = Utils.FromDBValue<string>(dr["Level4CategoryName"]);
            Level5CategoryName = Utils.FromDBValue<string>(dr["Level5CategoryName"]);
        }
    }
}