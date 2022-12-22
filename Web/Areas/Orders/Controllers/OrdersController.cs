using System.Web.Mvc;
using LeadingEdge.Curator.Web.Orders.Models;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Orders.Helpers;
using System;

namespace LeadingEdge.Curator.Web.Areas.Orders.Controllers
{
    public class OrdersController : AuthorizeController
    {
        [HttpGet]
        public ActionResult List()
        {
            OrdersList vm = OrdersHelper.CreateModel();

            SaveModelToCache(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult List(OrdersList vm)
        {
            OrdersList cached = GetModelFromCache<OrdersList>(vm.PageID);
            OrdersHelper.UpdateModel(vm, cached, IsExporting);

            if (IsExporting) return ExportGridView(ExportType.Xlsx, vm.Grids[ExportedGridName]);
            SaveModelToCache(vm);

            return View(vm);
        }

        [AjaxOnly]
        public ActionResult LoadDateRange(string pageID, int dateRangeID)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            var dateRange = OrdersHelper.GetDateRange(vm, dateRangeID);

            return SendAsJson(dateRange);
        }

        [AjaxOnly]
        public ActionResult StoreCallback(string pageID)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            vm.StoresModel = OrdersHelper.GetStoresComboBoxSettings(vm);

            SaveModelToCache(vm);

            return PartialView("CheckComboListBoxPartial", vm.StoresModel);
        }

        [AjaxOnly]
        public ActionResult UpdateTabs(string pageID, int salesOrderID)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            SalesOrderInfo orderInfo = OrdersHelper.GetSalesOrder(vm, salesOrderID);

            return SendAsJson(orderInfo);
        }

        public ActionResult GrdOrdersCallback(string pageID)
        {
            string name = CallbackID;
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            var args = GetCustomCallbackArgs("~");

            if (args.Count == 5)
            {
                vm.SelectedDateRange = Convert.ToInt32(args[0]);
                vm.SelectedStores = args[1];
                vm.OrderStatusID = Convert.ToInt32(args[2]);
                vm.FailedEDIDelivery = Convert.ToBoolean(args[3]);
                vm.IncompleteItemLines = Convert.ToBoolean(args[4]);

                SaveModelToCache(vm);

                vm.Grids[name].Data = OrdersHelper.GetSalesOrdersFromRange(vm);
            }

            if (args.Count == 6)
            {
                vm.FromDate = DateTime.Parse(args[0]);
                vm.ToDate = DateTime.Parse(args[1]);
                vm.SelectedStores = args[2];
                vm.OrderStatusID = Convert.ToInt32(args[3]);
                vm.FailedEDIDelivery = Convert.ToBoolean(args[4]);
                vm.IncompleteItemLines = Convert.ToBoolean(args[5]);

                SaveModelToCache(vm);

                vm.Grids[name].Data = OrdersHelper.GetSalesOrders(vm);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        public ActionResult GrdHistoryCallback(string pageID)
        {
            string name = CallbackID;
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            var args = GetCustomCallbackArgs("~");

            if (args.Count == 1)
            {
                int salesOrderID = Convert.ToInt32(args[0]);
                vm.Grids[name].Data = OrdersHelper.GetChangeHistory(vm, salesOrderID);
            }

            return PartialView("GridViewPartial", vm.Grids[name]);
        }

        [AjaxOnly]
        public ActionResult GetShippingAddress(int salesOrderID)
        {
            ShippingAddressInfo address = OrdersHelper.GetShippingAddress(salesOrderID);

            return SendAsJson(address);
        }

        [AjaxOnly]
        public ActionResult GetBillingAddress(string pageID, int salesOrderID)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            string billingAddress = OrdersHelper.GetBillingAddress(vm, salesOrderID);

            return SendAsJson(billingAddress);
        }

        [AjaxOnly]
        public ActionResult GetSelectedSupplierLine(string pageID, int salesOrderID, int salesOrderLineID, int feedKey)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            SupplierLineInfo supplerLine = OrdersHelper.GetSelectedSupplierLine(vm, salesOrderID, salesOrderLineID, feedKey);

            return SendAsJson(supplerLine);
        }

        [AjaxOnly]
        public ActionResult SaveShippingAddress(string pageID, int salesOrderID, ShippingAddressInfo shippingAddress)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            bool saved = OrdersHelper.SaveShippingAddress(vm, salesOrderID, shippingAddress);

            return SendAsJson(saved);
        }

        [AjaxOnly]
        public ActionResult GetFraudCheck(string pageID, int salesOrderID)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            FraudCheckInfo fraudCheck = OrdersHelper.GetFraudCheck(vm, salesOrderID);

            return SendAsJson(fraudCheck);
        }

        [AjaxOnly]
        public ActionResult SaveFraudCheck(string pageID, int salesOrderID, FraudCheckInfo fraudCheck)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            bool saved = OrdersHelper.SaveFraudCheck(vm, salesOrderID, fraudCheck);

            return SendAsJson(saved);
        }

        [AjaxOnly]
        public ActionResult PushToSupplier(string pageID, int salesOrderID, string salesOrderLineIDs)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            bool sent = OrdersHelper.SendSupplierEmail(vm, salesOrderID, salesOrderLineIDs);

            return SendAsJson(sent);
        }

        [AjaxOnly]
        public ActionResult SaveOrderLine(string pageID, SalesOrderLineInfo orderLine)
        {
            OrdersList vm = GetModelFromCache<OrdersList>(pageID);

            bool sent = OrdersHelper.SaveOrderLine(vm, orderLine);

            return SendAsJson(sent);
        }
    }
}