using System;
using System.Collections.Generic;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Models
{
	public class UsersEdit : BaseModel
	{
		public int? UserID { get; set; }
		public int CompanyID { get; set; }
		public int RegionID { get; set; }
		public string FullName { get; set; }
		public string Login { get; set; } 
		public string Password { get; set; }
		public string Position { get; set; }
		public string Mobile { get; set; }
		public string Telephone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public int UserGroupID { get; set; }		
		public bool Enabled { get; set; }
		public bool SendLoginDetailsEmail { get; set; }
		public string LanguageID { get; set; }
		public int[] Regions { get; set; }
		public bool SalesRep { get; set; }		
        public bool NewProductNotifications { get; set; }
		public bool ChangedProductNotifications { get; set; }
		public bool DeactivatedProductNotifications { get; set; }

		public List<CompanyRegionInfo> CompanyRegions { get; set; }
		public List<CompanyRegionInfo> AssignedCompanyRegions { get; set; }
		public List<UserGroupInfo> UserGroups { get; set; }
		public List<LanguageInfo> Languages { get; set; }

		public CheckBoxListSettings RegionCheckBoxListSettings { get; set; }
		public List<StoreInfo> MemberStores { get; set; }
		public string MemberStoreIDList { get; set; }
		public int MemberStoreID { get; set; }
		public string StoreName { get; set; }
		public int StoreID { get; set; }
		public List<StoreInfo> AvailableMemberStores { get; set; }

		public string ReloadUrl
		{
			get { return (UserID > 0) ? String.Format("/Admin/Users/Edit/{0}", UserID) : "/Admin/Users/New"; }
		}

		public UsersEdit()
		{
			CompanyRegions = new List<CompanyRegionInfo>();
			AssignedCompanyRegions = new List<CompanyRegionInfo>();
			UserGroups = new List<UserGroupInfo>();
			RegionCheckBoxListSettings = new CheckBoxListSettings();
			Languages = new List<LanguageInfo>();
			MemberStores = new List<StoreInfo>();
		}
	}
}