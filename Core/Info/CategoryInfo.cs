using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class CategoryInfo
    {
        public int CategoryKey { get; set; }
        public Guid CategoryID { get; set; }
        public int? ParentKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public Guid? Level1CategoryID { get; set; }
        public string Level1CategoryName { get; set; }
        public string Level1CategoryDescription { get; set; }

        public Guid? Level2CategoryID { get; set; }
        public string Level2CategoryName { get; set; }
        public string Level2CategoryDescription { get; set; }

        public Guid? Level3CategoryID { get; set; }
        public string Level3CategoryName { get; set; }
        public string Level3CategoryDescription { get; set; }

        public Guid? Level4CategoryID { get; set; }
        public string Level4CategoryName { get; set; }
        public string Level4CategoryDescription { get; set; }

        public Guid? Level5CategoryID { get; set; }
        public string Level5CategoryName { get; set; }
        public string Level5CategoryDescription { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public bool MemberSelected { get; set; }

        public string CategoryPath
        {
            get
            {
                string path = Level1CategoryName;
                if (!String.IsNullOrEmpty(Level2CategoryName)) path += string.Concat(" / ", Level2CategoryName);
                if (!String.IsNullOrEmpty(Level3CategoryName)) path += string.Concat(" / ", Level3CategoryName);
                if (!String.IsNullOrEmpty(Level4CategoryName)) path += string.Concat(" / ", Level4CategoryName);
                if (!String.IsNullOrEmpty(Level5CategoryName)) path += string.Concat(" / ", Level5CategoryName);

                return path;
            }
        }

        public string ShortCategoryPath
        {
            get
            {
                if (String.IsNullOrEmpty(CategoryPath)) return string.Empty;
                int length = 70;

                return CategoryPath.Length <= length ? CategoryPath : string.Concat("...", CategoryPath.Substring(CategoryPath.Length - length));
            }
        }

        public CategoryInfo()
        {
        }

        public CategoryInfo(DataRow dr)
        {
            CategoryKey = Utils.FromDBValue<int>(dr["CategoryKey"]);
            if (dr.Table.Columns.Contains("CategoryID")) CategoryID = Utils.FromDBValue<Guid>(dr["CategoryID"]);
            if (dr.Table.Columns.Contains("ParentKey")) ParentKey = Utils.FromDBValue<int?>(dr["ParentKey"]);
            if (dr.Table.Columns.Contains("Name")) Name = Utils.FromDBValue<string>(dr["Name"]);
            if (dr.Table.Columns.Contains("Description")) Description = Utils.FromDBValue<string>(dr["Description"]);
            if (dr.Table.Columns.Contains("Level")) Level = Utils.FromDBValue<int>(dr["Level"]);

            if (dr.Table.Columns.Contains("Level1CategoryID")) Level1CategoryID = Utils.FromDBValue<Guid?>(dr["Level1CategoryID"]);
            if (dr.Table.Columns.Contains("Level1CategoryName")) Level1CategoryName = Utils.FromDBValue<string>(dr["Level1CategoryName"]);
            if (dr.Table.Columns.Contains("Level1CategoryDescription")) Level1CategoryDescription = Utils.FromDBValue<string>(dr["Level1CategoryDescription"]);

            if (dr.Table.Columns.Contains("Level2CategoryID")) Level2CategoryID = Utils.FromDBValue<Guid?>(dr["Level2CategoryID"]);
            if (dr.Table.Columns.Contains("Level2CategoryName")) Level2CategoryName = Utils.FromDBValue<string>(dr["Level2CategoryName"]);
            if (dr.Table.Columns.Contains("Level2CategoryDescription")) Level2CategoryDescription = Utils.FromDBValue<string>(dr["Level2CategoryDescription"]);

            if (dr.Table.Columns.Contains("Level3CategoryID")) Level3CategoryID = Utils.FromDBValue<Guid?>(dr["Level3CategoryID"]);
            if (dr.Table.Columns.Contains("Level3CategoryName")) Level3CategoryName = Utils.FromDBValue<string>(dr["Level3CategoryName"]);
            if (dr.Table.Columns.Contains("Level3CategoryDescription")) Level3CategoryDescription = Utils.FromDBValue<string>(dr["Level3CategoryDescription"]);

            if (dr.Table.Columns.Contains("Level4CategoryID")) Level4CategoryID = Utils.FromDBValue<Guid?>(dr["Level4CategoryID"]);
            if (dr.Table.Columns.Contains("Level4CategoryName")) Level4CategoryName = Utils.FromDBValue<string>(dr["Level4CategoryName"]);
            if (dr.Table.Columns.Contains("Level4CategoryDescription")) Level4CategoryDescription = Utils.FromDBValue<string>(dr["Level4CategoryDescription"]);

            if (dr.Table.Columns.Contains("Level5CategoryID")) Level5CategoryID = Utils.FromDBValue<Guid?>(dr["Level5CategoryID"]);
            if (dr.Table.Columns.Contains("Level5CategoryName")) Level5CategoryName = Utils.FromDBValue<string>(dr["Level5CategoryName"]);
            if (dr.Table.Columns.Contains("Level5CategoryDescription")) Level5CategoryDescription = Utils.FromDBValue<string>(dr["Level5CategoryDescription"]);

            if (dr.Table.Columns.Contains("CreatedDate")) CreatedDate = Utils.FromDBValue<DateTimeOffset>(dr["CreatedDate"]);
            if (dr.Table.Columns.Contains("ModifiedDate")) ModifiedDate = Utils.FromDBValue<DateTimeOffset?>(dr["ModifiedDate"]);

            if (dr.Table.Columns.Contains("MemberSelected")) MemberSelected = Utils.FromDBValue<bool>(dr["MemberSelected"]);
        }
    }
}