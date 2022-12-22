using System;
using System.Text.RegularExpressions;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Home.Models;

namespace LeadingEdge.Curator.Web.Home.Helpers
{
    public class HomeHelper
    {
        public static HomeIndex CreateModel()
        {
            var vm = new HomeIndex();
            vm.RegionID = vm.SI.User.RegionID;
            vm.LanguageID = vm.SI.Language.LanguageID;
            return vm;
        }

        public static HomeDetails CreateDetailModel()
        {
            HomeDetails vm = new HomeDetails();
            vm.NewProductNotifications = vm.SI.User.NewProductNotifications;
            vm.ChangedProductNotifications = vm.SI.User.ChangedProductNotifications;
            vm.DeactivatedProductNotifications = vm.SI.User.DeactivatedProductNotifications;

            return vm;
        }

        public static HomeContact CreateContactModel()
        {
            HomeContact vm = new HomeContact();
            return vm;
        }

        public static Exception UpdateDetails(HomeDetails vm)
        {
            try
            {
                vm.SI.User.NewProductNotifications = vm.NewProductNotifications;
                vm.SI.User.ChangedProductNotifications = vm.ChangedProductNotifications;
                vm.SI.User.DeactivatedProductNotifications = vm.DeactivatedProductNotifications;

                return UserManager.Save(vm.SI.User);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static HomeRegions CreateRegionsModel()
        {
            HomeRegions vm = new HomeRegions();
            vm.Regions = CompanyRegionManager.GetCompanyRegionsByUser(vm.SI.User.CompanyID, vm.SI.User.UserID);
            return vm;
        }

        public static Exception UpdateRegion(HomeRegions vm)
        {
            try
            {
                vm.Regions = CompanyRegionManager.GetCompanyRegionsByUser(vm.SI.User.CompanyID, vm.SI.User.UserID);

                if (vm.Regions == null || vm.Regions.Count < 1) throw new Exception("No regions found.");

                CompanyRegionInfo region = vm.Regions.Find(x => x.RegionID == vm.RegionID);

                if (region == null) throw new Exception("Region not found.");

                UserInfo user = UserManager.GetUser(null, vm.SI.User.UserID);

                if (user == null) throw new Exception("User not found.");

                user.RegionID = region.RegionID;

                Exception ex = UserManager.Save(user);

                if (ex != null) throw ex;

                vm.SI.User = user;

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static HomeBookmarks CreateBookmarksModel()
        {
            HomeBookmarks vm = new HomeBookmarks();
            return vm;
        }
    }
}