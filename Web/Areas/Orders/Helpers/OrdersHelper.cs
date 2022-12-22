using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Wizards;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Orders.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LeadingEdge.Curator.Web.Orders.Helpers
{
    public class OrdersHelper
    {
        public static OrdersList CreateModel()
        {
            var vm = new OrdersList();

            vm.CanEdit = vm.SI.UserGroupPermissions.Exists(x => x.Code == "EDITORDERS");
            vm.FromDate = DateTime.Today.AddDays(-1);
            vm.ToDate = DateTime.Today;
            vm.SelectedDateRange = 0;
            vm.FailedEDIDelivery = false;
            vm.IncompleteItemLines = false;

            vm.DateRanges = GetFromToDates();
            vm.OrderStatuses = OrderStatuses.Statuses;
            vm.Stores = GetMemberStores(vm);
            vm.StoresModel = GetStoresComboBoxSettings(vm);

            vm.Grids.Add("GrdOrders", GetGridOrders("GrdOrders", vm));
            vm.Grids.Add("GrdHistory", GetGridHistory("GrdHistory", vm));

            return vm;
        }

        public static void UpdateModel(OrdersList vm, OrdersList cached, bool isExporting)
        {
            vm.Orders = cached.Orders;
            vm.Grids = cached.Grids;
            if (isExporting) return;

            SetGrdOrders(vm);
        }

        public static GridModel GetGridOrders(string name, OrdersList vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.07", vm.Name, true);
            grid.Settings.KeyFieldName = "SalesOrderID";
            grid.Settings.CallbackRouteValues = new { Area = "Orders", Controller = "Orders", Action = "GrdOrdersCallback" };
            grid.Settings.Styles.Header.Font.Size = FontUnit.Parse("11px");
            grid.Settings.Styles.FocusedRow.Font.Size = FontUnit.Parse("12px");
            grid.Settings.CommandColumn.Width = 65;
            grid.Settings.ClientSideEvents.DetailRowExpanding = "function(s,e) { DetailRowExpanding(s, e); }";

            grid.Settings.DetailRowExpandedChanged = (sender, e) =>
            {
                MVCxGridView mvCxGridView = (sender as MVCxGridView);

                int salesOrderId = Convert.ToInt32(mvCxGridView.GetRowValues(e.VisibleIndex, "SalesOrderID"));

                if (e.Expanded) {

                    return;
                }

                if (!e.Expanded) {
                    // remove from vm list
                    OrderLineList orderDetail = vm.OrderDetails.Find(x => x.SalesOrderID == salesOrderId);
                    vm.OrderDetails.Remove(orderDetail);
                }
            };

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "CustomerName";
                s.Caption = grid.Label(200546);                 // Customer name
                s.Width = 145;
                s.SortIndex = 0;                                // Default vm.SortBy
                s.SortOrder = ColumnSortOrder.Ascending;        // Default vm.SortDirection
                s.FixedStyle = GridViewColumnFixedStyle.Left;
            }); 
            
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "CreatedDate";
                s.Caption = grid.Label(200651); // Created date
                s.Width = 130;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "PurchaseOrderNumber";
                s.Caption = grid.Label(300061); // 	Order no
                s.Width = 160;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SalesOrderPurchaseStatusID";
                s.Caption = grid.Label(200563); // Status
                s.Width = 160;
            });
 
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreName";
                s.Caption = grid.Label(300129); // Source Store
                s.Width = 440;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SalesOrderPaymentStatusID";
                s.Caption = grid.Label(300134); // Payment status
                s.Width = 140;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.SpinEdit;
                s.FieldName = "TotalAmount";
                s.Caption = grid.Label(300121); // Total
                s.Width = 100;
                s.PropertiesEdit.DisplayFormatString = "n2";
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "SalesOrderFulfillmentStatusID";
                s.Caption = grid.Label(300135); // Fulfillment status
                s.Width = 170;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "FulfillDate";
                s.Caption = grid.Label(300131); // Fulfilled date
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.Width = 130;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "ModifiedDate";
                s.Caption = grid.Label(200656); // Updated date
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                s.Width = 130;
            });

            //grid.Settings.Columns.Add(s =>
            //{
            //    s.ColumnType = MVCxGridViewColumnType.CheckBox;
            //    s.FieldName = "FailedEDIDelivery";
            //    s.Caption = grid.Label(300144); // Failed EDI delivery?
            //    s.Width = 145;
            //});

            //grid.Settings.Columns.Add(s =>
            //{
            //    s.ColumnType = MVCxGridViewColumnType.CheckBox;
            //    s.FieldName = "FailedNetSuiteSync";
            //    s.Caption = grid.Label(300145); // Failed NetSuite sync?
            //    s.Width = 160;
            //});

            grid.Settings.SettingsDetail.ShowDetailRow = true;
            grid.Settings.SetDetailRowTemplateContent(c =>
            {
                //Check if we have already loaded this before
                int salesOrderID = Convert.ToInt32(DataBinder.Eval(c.DataItem, "SalesOrderID"));
                OrderLineList oldDetail = vm.OrderDetails.Find(x => x.SalesOrderID == salesOrderID);

                if (oldDetail != null) {
                    string oldDetailHtml = U.RenderPartialToString("~/Areas/Orders/Views/Orders/ListOrderLines.ascx", oldDetail);

                    HttpContext.Current.Response.Write(oldDetailHtml);
                    return;
                }

                //Setup Model
                OrderLineList oll = new OrderLineList();
                oll.CanEdit = vm.CanEdit;
                oll.SalesOrderID = salesOrderID;

                //Get Order from grid
                var data = (List<SalesOrderInfo>)vm.Grids["GrdOrders"].Data;
                oll.Order = data.Find(x => x.SalesOrderID == oll.SalesOrderID);

                //Get Order Lines
                oll.OrderLines = GetSalesOrderLines(oll.SalesOrderID);

                // Using first address as edit is at order level for now
                if (oll.OrderLines.Count >= 1)
                {
                    oll.ShippingAddress = oll.OrderLines.First().ShippingAddressFormatted;
                }

                //Get Suppliers per item line
                foreach (SalesOrderLineInfo orderLine in oll.OrderLines) {
                    GetSelectableSuppliers(orderLine);
                }

                //Add to model
                vm.OrderDetails.Add(oll);

                string detailHtml = U.RenderPartialToString("~/Areas/Orders/Views/Orders/ListOrderLines.ascx", oll);

                HttpContext.Current.Response.Write(detailHtml);
            });

            return grid;
        }

        public static void SetGrdOrders(OrdersList vm)
        {
            List<SalesOrderInfo> orders = GetSalesOrders(vm);
            vm.Grids["GrdOrders"] = GetGridOrders("GrdOrders", vm);
            vm.Grids["GrdOrders"].Data = orders;
        }

        public static GridModel GetGridHistory(string name, OrdersList vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.07", vm.Name, true);
            grid.Settings.KeyFieldName = "ChangeID";
            grid.Settings.CallbackRouteValues = new { Area = "Orders", Controller = "Orders", Action = "GrdHistoryCallback" };
            grid.Settings.Styles.Header.Font.Size = FontUnit.Parse("11px");
            grid.Settings.Styles.FocusedRow.Font.Size = FontUnit.Parse("12px");
            grid.Settings.CommandColumn.Width = 65;

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.DateEdit;
                s.FieldName = "Time";
                s.Caption = grid.Label(200677); // Time
                s.Width = 200;
                s.PropertiesEdit.DisplayFormatString = "{0:dd/MM/yyyy HH:mm}";
                s.SortOrder = ColumnSortOrder.Descending;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "UserName";
                s.Caption = grid.Label(200108); // User
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "FormattedDetail";
                s.Caption = grid.Label(300163); // Change
                s.Width = 600;
            });

            return grid;
        }

        public static CheckComboModel GetStoresComboBoxSettings(OrdersList vm)
        {
            var model = new CheckComboModel();

            Setup.CheckBoxCombo(model.Settings, "StoreID");

            // UI
            var dropdown = model.Settings.DropDownSettings;
            dropdown.Width = Unit.Pixel(150);

            var listbox = model.Settings.ListBoxSettings;

            listbox.CallbackRouteValues = new { Area = "Orders", Controller = "Orders", Action = "StoreCallback" };
            listbox.Properties.ClientSideEvents.BeginCallback = "function(s,e) { Store_BeginCallback(s,e); }";
            listbox.Properties.ClientSideEvents.EndCallback = "function(s,e) { Store_EndCallback(s,e); }";
            listbox.Properties.ClientSideEvents.Init = "function(s,e) { Store_EndCallback(s,e); }";

            // Data
            var data = (from x in vm.Stores select new ListEditItem(x.StoreName, x.StoreID)).ToList();

            data.Insert(0, new ListEditItem("ALL", null));   // Add null option

            if (data.Count > 1)
            {
                vm.SelectedStores = Convert.ToString(data[1].Value);
                model.Settings.ListBoxSettings.SelectedIndex = 1;
            }
            else
            {
                model.Settings.ListBoxSettings.SelectedIndex = 0;
            }

            model.Data = data;

            return model;
        }

        public static List<SalesOrderInfo> GetSalesOrders(OrdersList vm)
        {
            string[] stores = vm.SelectedStores.Split(',');
            string[] storesCleaned = stores.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            vm.SelectedStores = string.Join(",", storesCleaned);

            return OrdersManager.GetSalesOrders(vm.FromDate, vm.ToDate, vm.SelectedStores, vm.OrderStatusID, vm.FailedEDIDelivery, vm.IncompleteItemLines);
        }

        public static List<SalesOrderInfo> GetSalesOrdersFromRange(OrdersList vm)
        {
            FromToDate chosenDate = vm.DateRanges.Find(x => x.FromToDateID == vm.SelectedDateRange);
            vm.FromDate = chosenDate.FromDate;
            vm.ToDate = chosenDate.ToDate;

            return GetSalesOrders(vm);
        }

        public static SalesOrderInfo GetSalesOrder(OrdersList vm, int salesOrderID)
        {
            var data = (List<SalesOrderInfo>)vm.Grids["GrdOrders"].Data;

            return data.Find(x => x.SalesOrderID == salesOrderID);
        }

        public static List<SalesOrderLineInfo> GetSalesOrderLines(int salesOrderID)
        {
            return OrdersManager.GetSalesOrderLines(salesOrderID).OrderBy(x => x.SalesOrderLineID).ToList();
        }

        public static SalesOrderLineInfo GetSalesOrderLine(int salesOrderID, int salesOrderLineID)
        {
            return OrdersManager.GetSalesOrderLines(salesOrderID).Find(x => x.SalesOrderLineID == salesOrderLineID);
        }

        public static List<ChangeHistoryInfo> GetChangeHistory(OrdersList vm, int salesOrderID)
        {
            return ChangeHistoryManager.GetChangeHistoryProductsDB(vm.SI.User.CompanyID, vm.SI.User.RegionID, "SALESORDER", salesOrderID, vm.SI.User.UserID);
        }

        public static List<FromToDate> GetFromToDates()
        {
            List<FromToDate> dates = new List<FromToDate>
            {
                new FromToDate(0, "Yesterday", DateTime.Today.AddDays(-1), DateTime.Today),
                new FromToDate(1, "Last 7 days", DateTime.Today.AddDays(-7), DateTime.Today),
                new FromToDate(2, "Last 30 days", DateTime.Today.AddDays(-30), DateTime.Today)
            };

            return dates;
        }

        public static FromToDate GetDateRange(OrdersList vm, int dateRangeID)
        {
            return vm.DateRanges.Find(x => x.FromToDateID == dateRangeID);
        }

        public static List<StoreInfo> GetMemberStores(OrdersList vm)
        {
            return ProductManager.GetMemberStoreList(vm.SI.User.UserID).OrderBy(x => x.StoreName).ToList();
        }

        public static void GetSelectableSuppliers(SalesOrderLineInfo orderLine)
        {
            orderLine.SelectableSuppliers = OrdersManager.GetSelectableSuppliers(orderLine.SalesOrderLineID);
        }

        public static SupplierLineInfo GetSelectedSupplierLine(OrdersList vm, int salesOrderID, int salesOrderLineID, int feedKey)
        {
            OrderLineList orderDetail = vm.OrderDetails.Find(x => x.SalesOrderID == salesOrderID);

            SalesOrderLineInfo orderLine = orderDetail.OrderLines.Find(x => x.SalesOrderLineID == salesOrderLineID);

            SupplierLineInfo supplierLine = orderLine.SelectableSuppliers.Find(x => x.FeedKey == feedKey);

            return supplierLine;
        }

        public static ShippingAddressInfo GetShippingAddress(int salesOrderID)
        {
            // Find order lines
            List<SalesOrderLineInfo> orderLines = GetSalesOrderLines(salesOrderID);

            // Return empty address if there is none
            if(orderLines.Count < 1) return new ShippingAddressInfo();

            // Otherwise use the first order line
            SalesOrderLineInfo firstOrderLine = orderLines[0];

            ShippingAddressInfo address = new ShippingAddressInfo(firstOrderLine);

            return address;
        }

        public static string GetBillingAddress(OrdersList vm, int salesOrderID) 
        {
            SalesOrderInfo order = GetSalesOrder(vm, salesOrderID);

            return order.BillingAddressFormatted;
        }

        public static FraudCheckInfo GetFraudCheck(OrdersList vm, int salesOrderID)
        {
            // Find order
            SalesOrderInfo orderInfo = GetSalesOrder(vm, salesOrderID);

            // Create fraud check info and return
            FraudCheckInfo fraudCheck = new FraudCheckInfo(orderInfo);

            return fraudCheck;
        }

        public static void SaveSalesOrderGridData(OrdersList vm, SalesOrderInfo orderInfo)
        {
            var data = (List<SalesOrderInfo>)vm.Grids["GrdOrders"].Data;

            SalesOrderInfo oldOrderInfo = data.Find(x => x.SalesOrderID == orderInfo.SalesOrderID);

            data.Add(orderInfo);
            data.Remove(oldOrderInfo);

            vm.Grids["GrdOrders"].Data = data;
        }

        public static bool SaveShippingAddress(OrdersList vm, int salesOrderID, ShippingAddressInfo shippingAddress)
        {
            // Update the db and save change history
            Exception ex = OrdersManager.SaveShippingAddress(salesOrderID, shippingAddress, vm.SI.User);
            if (ex != null) 
            { 
                Log.Error(ex);
                return false; // Display error message
            }

            // Reload memo and close popup
            return true;
        }

        public static bool SaveFraudCheck(OrdersList vm, int salesOrderID, FraudCheckInfo fraudCheck) 
        {
            //Get existing fraud check
            FraudCheckInfo oldFraudCheck = GetFraudCheck(vm, salesOrderID);

            // See if any changes have been made
            bool unchanged = Equals(oldFraudCheck, fraudCheck);

            // If there are no changes return false and popup will close without reloading the push to supplier buttons
            if (unchanged)
            {
                return false;
            }
            else
            {
                // Update db and save change history
                Exception ex = OrdersManager.SaveFraudCheck(salesOrderID, fraudCheck, vm.SI.User);
                if (ex != null) Log.Error(ex);

                // Update grid data
                SalesOrderInfo orderInfo = GetSalesOrder(vm, salesOrderID);
                SalesOrderInfo.UpdateFraudCheckFields(orderInfo, fraudCheck);
                SaveSalesOrderGridData(vm, orderInfo);

                // Return true to revalidate the push to supplier buttons
                return true;
            }
        }

        public static bool SendSupplierEmail(OrdersList vm, int salesOrderID, string salesOrderLineIDs)
        {
            // Convert ids in string to int array for processing later
            int[] salesOrderLineIntIDs = salesOrderLineIDs.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();

            // Find order and selected order lines
            SalesOrderInfo order = GetSalesOrder(vm, salesOrderID);

            // Find store info
            StoreInfo store = StoreManager.GetStore(order.StoreID);

            // Email Body
            string emailBody = EmailGenerator.PushToSupplierEmail(order, store);

            List<SalesOrderLineInfo> allSalesOrderLines = GetSalesOrderLines(salesOrderID);

            List<SalesOrderLineInfo> selectedSalesOrders = new List<SalesOrderLineInfo>();

            // Group the selected order lines based on chosen supplier, emails same supplier with all selected order lines in one pdf
            foreach (int salesOrderLineID in salesOrderLineIntIDs)
            {
                selectedSalesOrders.Add(allSalesOrderLines.Find(x => x.SalesOrderLineID == salesOrderLineID));
            }

            var groupedSelectedSalesOrders = selectedSalesOrders.GroupBy(x => x.SupplierID);

            foreach (var group in groupedSelectedSalesOrders)
            {
                try 
                {
                    // Since we have grouped them by supplier they will all have the same supplier
                    SalesOrderLineInfo firstLine = group.First();
                    GetSelectableSuppliers(firstLine);
                    SupplierLineInfo supplier = firstLine.SelectableSuppliers.Find(x => x.FeedKey == firstLine.SupplierID);

                    // Get report bytes
                    byte[] reportBytes = ReportGenerator.GetSendToSupplierBytes(order, group.ToList(), store);

                    // Send email out
                    EmailInfo emailInfo = new EmailInfo();
                    emailInfo.To = supplier.PushToSupplierEmail;
                    emailInfo.Subject = "Leading Edge Group Supplier Order";
                    emailInfo.Body = emailBody;
                    emailInfo.CompanyID = vm.SI.User.CompanyID;
                    emailInfo.RegionID = vm.SI.User.RegionID;
                    emailInfo.UserID = vm.SI.User.UserID;
                    emailInfo.ReferenceID = salesOrderID;
                    emailInfo.EmailType = "PUSH_TO_SUPPLIER";

                    int? emailID = EmailManager.InsertEmail(emailInfo);

                    if (!emailID.HasValue) throw new Exception("Supplier email failed to send");

                    // Send pdf
                    EmailObjectInfo emailObjectInfo = new EmailObjectInfo();
                    emailObjectInfo.EmailID = emailID.Value;
                    emailObjectInfo.Name = "LEG Sales Order.pdf";
                    emailObjectInfo.Bytes = reportBytes;
                    emailObjectInfo.ContentType = "SUPPLIER_ORDER";
                    emailObjectInfo.CompanyID = vm.SI.User.CompanyID;
                    emailObjectInfo.RegionID = vm.SI.User.RegionID;

                    int? emailObjectID = EmailManager.InsertEmailObject(emailObjectInfo);

                    if (!emailObjectID.HasValue) throw new Exception("Failed to attach pdf");
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return true;
        }

        public static bool SaveOrderLine(OrdersList vm, SalesOrderLineInfo orderLine)
        {
            // Update the db and save change history
            Exception ex = OrdersManager.SaveSalesOrderLine(orderLine, vm.SI.User);
            if (ex != null) Log.Error(ex);

            // Return true
            return true;
        }
    }
}