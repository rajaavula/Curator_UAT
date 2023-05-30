using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class MemberFeedsHelper
    {
        public static MemberFeeds CreateModel()
        {
            MemberFeeds vm = new MemberFeeds();
            vm.MemberStoreList = GetMemberStores(vm);
            if (vm.MemberStoreList != null && vm.MemberStoreList.Count > 0) vm.StoreID = vm.MemberStoreList.First().StoreID;
            else
            {
                StoreInfo storeInfo = new StoreInfo();
                storeInfo.StoreID = 0;
                storeInfo.StoreName = "No stores assigned";
                vm.MemberStoreList.Add(storeInfo);
                vm.StoreID = vm.MemberStoreList.First().StoreID;

            }
            vm.Feeds = GetAllFeeds();

            if (vm.Feeds != null && vm.Feeds.Count > 0)
            {
                vm.FeedKey = vm.Feeds.First().FeedKey;
            }
            vm.AssignedFeeds = FeedManager.GetFeedsByMember(vm.StoreID);
            vm.AvailableFeeds = vm.Feeds.FindAll(x => vm.AssignedFeeds.Exists(y => y.FeedKey == x.FeedKey) == false).OrderBy(x => x.FeedName).ToList();
            vm.FeedKeys = string.Join(",", from memberObject in vm.AssignedFeeds select memberObject.FeedKey);

            return vm;
        }

        public static void UpdateModel(MemberFeeds vm, MemberFeeds cached)
        {
            vm.Feeds = cached.Feeds;
            vm.MemberStoreList = cached.MemberStoreList;
            if (vm.IsSave && vm.StoreID > 0)
            {
                FeedManager.UpdateMemberFeeds(vm.StoreID, vm.FeedKeys);
            }

            vm.AssignedFeeds = FeedManager.GetFeedsByMember(vm.StoreID);
            vm.AvailableFeeds = FeedManager.GetAllFeeds();
            vm.AvailableFeeds = vm.AvailableFeeds.FindAll(x => vm.AssignedFeeds.Exists(y => y.FeedKey == x.FeedKey) == false).OrderBy(x => x.FeedName).ToList();
            vm.FeedKeys = string.Join(",", from memberObject in vm.AssignedFeeds select memberObject.FeedKey);

            vm.IsSave = false;        
        }

        public static List<FeedInfo> GetAllFeeds()
        {
            return FeedManager.GetAllFeeds();
        }

        public static List<StoreInfo> GetMemberStores(MemberFeeds vm)
        {
            return ProductManager.GetEcommerceMemberStoresList(vm.SI.User.UserID).OrderBy(x => x.StoreName).ToList();
        }
    }
}

