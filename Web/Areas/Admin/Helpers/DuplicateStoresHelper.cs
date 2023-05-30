using DevExpress.Web;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class DuplicateStoresHelper
	{
		public static DuplicateStoresList CreateModel()
		{
			DuplicateStoresList vm = new DuplicateStoresList();

			vm.Stores = GetStores(vm);
			vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

			return vm;
		}

		public static void UpdateModel(DuplicateStoresList vm, DuplicateStoresList cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			if (isExporting) return;
		}

		public static GridModel GetGridView(string name, bool exporting, DuplicateStoresList vm)
		{
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.01", vm.Name);
            grid.Settings.KeyFieldName = "StoreID";
            grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "DuplicateStores", Action = "GrdMainCallback" };
            grid.Settings.CommandColumn.ShowSelectCheckbox = true;
            grid.Settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreName";
                s.Caption = grid.Label(201025); // Name
                s.Width = 1500;
            });

            return grid;
        }

		public static List<StoreInfo> GetStores(DuplicateStoresList vm) 
		{
            return ProductManager.GetEcommerceMemberStoresList(vm.SI.User.UserID).OrderBy(x => x.StoreName).ToList();
        }

		public static List<StoreInfo> GetData(DuplicateStoresList vm)
		{
            List<StoreInfo> stores = GetStores(vm);
            StoreInfo sourceStore = stores.First(x => x.StoreID == vm.SourceStoreID);
            stores.Remove(sourceStore);
            return stores;
        }

        public static string DuplicateStores(DuplicateStoresList vm, string ids)
        {
            string error = string.Empty;

            int[] idArray = ids.Split(',').Select(int.Parse).ToArray();

            foreach (int id in idArray)
            {
                Exception ex = StoreManager.DuplicateStore(id, vm.SourceStoreID);
                if (ex != null)
                {
                    Log.Error(ex);
                    error = "Error duplicating one or more stores, please contact support";
                }
            }

            return error;
        }
	}
}