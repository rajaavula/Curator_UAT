using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System.Collections.Generic;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class FeedsHelper
    {
        public static FeedsListEdit CreateModel()
        {
            var vm = new FeedsListEdit();

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            return vm;
        }

        public static void UpdateModel(FeedsListEdit vm, FeedsListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;

            if (isExporting) return;
        }

        public static GridModel GetGridView(string name, bool exporting, FeedsListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.02", vm.Name);
            grid.Settings.KeyFieldName = "FeedKey";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "Feeds", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "FeedName";
                s.Caption = grid.Label(200966); // Feed
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IncludeZeroStock";
                s.Caption = grid.Label(300110); // Include Zero Stock
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "PushToSupplierEmail";
                s.Caption = grid.Label(300176); // Push to supplier email
                s.Width = 250;
            });

            return grid;
        }

        public static List<FeedInfo> GetData()
        {
            return FeedManager.GetUsedFeeds();
        }

        public static FeedInfo GetDetail(FeedsListEdit vm, int id)
        {
            return ((List<FeedInfo>)vm.Grids["GrdMain"].Data).Find(x => x.FeedKey == id);
        }

        public static string Save(FeedInfo info)
        {
            string error = FeedManager.SaveFeed(info);
            return error;
        }
    }
}