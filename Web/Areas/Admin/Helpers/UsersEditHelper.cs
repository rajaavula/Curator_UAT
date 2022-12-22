using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Web.Mvc;
using System.Collections.Generic;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
    public class UsersEditHelper
    {
        public static UsersEdit CreateModel(int? id)
        {
            UsersEdit vm = new UsersEdit();
            vm.UserGroups = UserGroupManager.GetUserGroups(vm.SI.User.CompanyID, vm.SI.UserGroup.UserGroupID);
            vm.Languages = LanguageManager.GetLanguages();
            vm.CompanyRegions = CompanyRegionManager.GetCompanyRegions(vm.SI.User.CompanyID);
            vm.AvailableMemberStores = GetAvailableMemberStores();
            
            if (id.HasValue && id.Value > 0)
            {
                UserInfo user = UserManager.GetUser(vm.SI.User.CompanyID, id.Value);
                vm.UserID = id;
                vm.MemberStores = GetMemberStores(vm);

                SetModelFromInfo(user, vm);   
            }
            else
            {
                SetModelForNewUser(vm);
            }

            vm.RegionCheckBoxListSettings = GetRegionCheckBoxListSettings(vm, id);

            return vm;
        }

        public static void UpdateModel(UsersEdit vm, UsersEdit cached)
        {
            vm.UserID = cached.UserID;
            vm.UserGroups = cached.UserGroups;
            vm.Languages = cached.Languages;
            vm.RegionCheckBoxListSettings = cached.RegionCheckBoxListSettings;
            vm.MemberStores = cached.MemberStores;                      
            vm.AvailableMemberStores = GetAvailableMemberStores();
            
            // Things we aren't changing...
            if (String.IsNullOrEmpty(vm.Password)) vm.Password = cached.Password;
            vm.CompanyID = cached.CompanyID;
            vm.Login = cached.Login;

            // Disabled for SSO as per [#CX050526] Curator Shopify Changes 10.2 - Allow members to opt in/out of notifications
            vm.Password = cached.Password;

            if (vm.UserID < 0)
            {
                vm.Password = "D8Me3xZ#jd!id03";
            }

            vm.SendLoginDetailsEmail = false;   // as above

            if (vm.UserID == vm.SI.User.UserID) // Account for when the user edits himself and removes the region he is logged in as
            {
                if (!vm.Regions.ToList().Exists(x => x == vm.SI.User.RegionID)) vm.Regions[vm.Regions.Length] = vm.SI.User.RegionID;
                vm.RegionID = vm.SI.User.RegionID;
            }
            else
            {
                vm.RegionID = vm.Regions.Min(); // get first created region for the company
            }

            string regionIDs = String.Join(",", from regionID in vm.Regions select regionID);

            if (!Regex.IsMatch(vm.Password, App.PasswordRegex))
            {
                vm.Exception = new Exception("The new password must be a minimum of 8 characters and must contain a number.");
                return;
            }

            UserInfo info = CreateInfoFromModel(vm);
           
            vm.Exception = UserManager.Save(info, regionIDs);
            vm.UserID = info.UserID;
            if (vm.Exception != null) return;

            vm.Exception = SaveMemberStores(vm, vm.MemberStoreIDList); // for saving member stores against user 
            if (vm.Exception != null) return;

            if (vm.UserID == vm.SI.User.UserID)     // If the user is editing himself then refresh the user info
            {
                vm.SI.User = UserManager.GetUser(vm.SI.User.CompanyID, vm.SI.User.UserID);
                vm.SI.UserRegions = CompanyRegionManager.GetCompanyRegionsByUser(vm.SI.User.CompanyID, vm.SI.User.UserID);
                vm.MemberStores = GetMemberStores(vm);
            }

            if (vm.UserID < 0) vm.UserID = info.UserID;
            vm.Success = (vm.Exception == null);
        }

        public static void SetModelForNewUser(UsersEdit vm)
        {
            vm.CompanyID = vm.SI.User.CompanyID;
            vm.UserID = -1;
            vm.Enabled = true;
            vm.LanguageID = "ENGLISH";
            vm.UserGroupID = vm.UserGroups[0].UserGroupID;
            vm.Regions = new[] { vm.SI.User.RegionID };
            vm.AssignedCompanyRegions.Add(CompanyRegionManager.GetCompanyRegion(vm.CompanyID, vm.SI.User.RegionID));           
        }

        public static void SetModelFromInfo(UserInfo info, UsersEdit vm)
        {
            vm.UserID = info.UserID;
            vm.CompanyID = info.CompanyID;
            vm.RegionID = info.RegionID;
            vm.FullName = info.Name;
            vm.Login = info.Login;
            vm.Password = info.Password;
            vm.Position = info.Position;
            vm.Mobile = info.Mobile;
            vm.Telephone = info.Telephone;
            vm.Fax = info.Fax;
            vm.Email = info.Email;
            vm.UserGroupID = info.UserGroupID;
            vm.SalesRep = info.SalesRep;
            vm.Enabled = info.Enabled;
            vm.SendLoginDetailsEmail = info.SendLoginDetailsEmail;
            vm.LanguageID = info.LanguageID;
            vm.MemberStoreIDList = string.Join(",", vm.MemberStores.Select(x => x.StoreID)); // initializing shopify store selectize list
            vm.AssignedCompanyRegions = CompanyRegionManager.GetCompanyRegionsByUser(info.CompanyID, info.UserID);
            vm.NewProductNotifications = info.NewProductNotifications;
            vm.ChangedProductNotifications = info.ChangedProductNotifications;
            vm.DeactivatedProductNotifications = info.DeactivatedProductNotifications;
        }

        public static UserInfo CreateInfoFromModel(UsersEdit vm)
        {
            UserInfo info = new UserInfo();

            info.UserID = vm.UserID ?? -1;
            info.CompanyID = vm.CompanyID;
            info.RegionID = vm.RegionID;
            info.Name = vm.FullName;
            info.Login = vm.Login;
            info.Password = vm.Password;
            info.Position = vm.Position;
            info.Mobile = vm.Mobile;
            info.Telephone = vm.Telephone;
            info.Fax = vm.Fax;
            info.Email = vm.Email;
            info.UserGroupID = vm.UserGroupID;
            info.SalesRep = vm.SalesRep;
            info.Enabled = vm.Enabled;
            info.SendLoginDetailsEmail = vm.SendLoginDetailsEmail;
            info.LanguageID = vm.LanguageID;
            info.MemberStoreIDList = vm.MemberStoreIDList;
            info.NewProductNotifications = vm.NewProductNotifications;
            info.ChangedProductNotifications = vm.ChangedProductNotifications;
            info.DeactivatedProductNotifications = vm.DeactivatedProductNotifications;

            return info;
        }

        public static CheckBoxListSettings GetRegionCheckBoxListSettings(UsersEdit vm, int? userID)
        {
            var s = new CheckBoxListSettings();
            s.Name = "Regions_CheckBoxList";
            s.Properties.ValueType = typeof(int);
            s.Properties.ValueField = "RegionID";
            s.Properties.TextField = "Name";
            s.Properties.RepeatDirection = RepeatDirection.Horizontal;
            s.Properties.RepeatColumns = 4;

            s.Properties.ValidationSettings.Display = Display.Static;
            s.Properties.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Top;
            s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.Text;
            s.Properties.ValidationSettings.RequiredField.IsRequired = true;
            s.Properties.ValidationSettings.RequiredField.ErrorText = "Please select at least one region";

            s.PreRender = (sender, e) =>
            {
                var c = (ASPxCheckBoxList)sender;
                foreach (ListEditItem item in c.Items)
                {
                    item.Selected = vm.AssignedCompanyRegions.Exists(x => x.RegionID == (int)item.Value);
                }
            };

            foreach (CompanyRegionInfo region in vm.CompanyRegions)
            {
                s.Properties.Items.Add(new ListEditItem(region.Name, region.RegionID));
            }

            return s;
        }

        public static Exception SaveMemberStores(UsersEdit vm, string memberStoreIDList)
        {
            Exception ex = UserManager.SaveMemberStores(vm.UserID.Value, memberStoreIDList);     

            return ex;
        }

        public static List<StoreInfo> GetMemberStores(UsersEdit vm)
        {
            return UserManager.GetMemberStores(vm.UserID.Value);
        }

        private static List<StoreInfo> GetAvailableMemberStores()
        {
            return UserManager.GetAvailableMemberStores().ToList();
        }
    }
}