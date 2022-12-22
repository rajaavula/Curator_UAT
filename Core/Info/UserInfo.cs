using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int UserGroupID { get; set; }
        public string UserGroupName { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public bool SendLoginDetailsEmail { get; set; }
        public DateTime? SentLoginDetailsEmail { get; set; }
        public string LanguageID { get; set; }
        public bool SalesRep { get; set; }
        public Guid? OidcId { get; set; }
        public int? StoreID { get; set; }
        public string MemberStoreIDList { get; set; }
        public bool NewProductNotifications { get; set; }
		public bool ChangedProductNotifications { get; set; }
		public bool DeactivatedProductNotifications { get; set; }

        public UserInfo() { }

        public UserInfo(DataRow dr)
        {
            UserID = Utils.FromDBValue<int>(dr["UserID"]);
            CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
            RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
            Name = Utils.FromDBValue<string>(dr["Name"]);
            Login = Utils.FromDBValue<string>(dr["Login"]);
            Password = Utils.FromDBValue<string>(dr["Password"]);
            Password = U.Decrypt(Utils.FromDBValue<string>(dr["Password"]));
            Position = Utils.FromDBValue<string>(dr["Position"]);
            Mobile = Utils.FromDBValue<string>(dr["Mobile"]);
            Telephone = Utils.FromDBValue<string>(dr["Telephone"]);
            Fax = Utils.FromDBValue<string>(dr["Fax"]);
            Email = Utils.FromDBValue<string>(dr["Email"]);
            UserGroupID = Utils.FromDBValue<int>(dr["UserGroupID"]);
            UserGroupName = Utils.FromDBValue<string>(dr["UserGroupName"]);
            Enabled = Utils.FromDBValue<bool>(dr["Enabled"]);
            Created = Utils.FromDBValue<DateTime>(dr["Created"]);
            SendLoginDetailsEmail = Utils.FromDBValue<bool>(dr["SendLoginDetailsEmail"]);
            SentLoginDetailsEmail = Utils.FromDBValue<DateTime?>(dr["SentLoginDetailsEmail"]);
            SalesRep = Utils.FromDBValue<bool>(dr["SalesRep"]);
            LanguageID = Utils.FromDBValue<string>(dr["LanguageID"]);
            OidcId = Utils.FromDBValue<Guid?>(dr["OidcId"]);
            StoreID = Utils.FromDBValue<int?>(dr["StoreID"]);  
            NewProductNotifications = Utils.FromDBValue<bool>(dr["NewProductNotifications"]);
            ChangedProductNotifications = Utils.FromDBValue<bool>(dr["ChangedProductNotifications"]);
            DeactivatedProductNotifications = Utils.FromDBValue<bool>(dr["DeactivatedProductNotifications"]);
        }
    }
}
