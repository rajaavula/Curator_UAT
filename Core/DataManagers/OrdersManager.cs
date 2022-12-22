using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class OrdersManager
    {
        public static List<SalesOrderInfo> GetSalesOrders(DateTime fromDate, DateTime toDate, string selectedStores, int orderStatusID, bool failedEDIDelivery, bool incompleteItemLines)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@FromDate", Utils.ToDBValue(fromDate)),
                new SqlParameter("@ToDate", Utils.ToDBValue(toDate)),
                new SqlParameter("@SelectedStores", Utils.ToDBValue(selectedStores)),
                new SqlParameter("@OrderStatusID", Utils.ToDBValue(orderStatusID)),
                new SqlParameter("@FailedEDIDelivery", Utils.ToDBValue(failedEDIDelivery)),
                new SqlParameter("@IncompleteItemLines", Utils.ToDBValue(incompleteItemLines)),
            };

            var dt = db.QuerySP("CURATOR_GetSalesOrders", parameters);

            return (from dr in dt.AsEnumerable() select new SalesOrderInfo(dr)).ToList();
        }

        public static List<SalesOrderLineInfo> GetSalesOrderLines(int salesOrderID)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@SalesOrderID", Utils.ToDBValue(salesOrderID))
            };

            var dt = db.QuerySP("CURATOR_GetSalesOrderLines", parameters);

            return (from dr in dt.AsEnumerable() select new SalesOrderLineInfo(dr)).ToList();
        }

        public static List<SupplierLineInfo> GetSelectableSuppliers(int salesOrderLineID)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@SalesOrderLineID", Utils.ToDBValue(salesOrderLineID))
            };

            var dt = db.QuerySP("CURATOR_GetSelectableSuppliers", parameters);

            return (from dr in dt.AsEnumerable() select new SupplierLineInfo(dr)).ToList();
        }

        public static Exception SaveShippingAddress(int salesOrderID, ShippingAddressInfo shippingAddress, UserInfo user)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@SalesOrderID", Utils.ToDBValue(salesOrderID)),
                new SqlParameter("@ShippingAddressID", Utils.ToDBValue(shippingAddress.ShippingAddressID)),
                new SqlParameter("@ShippingAddressCompany", Utils.ToDBValue(shippingAddress.ShippingAddressCompany)),
                new SqlParameter("@ShippingAddressFirstName", Utils.ToDBValue(shippingAddress.ShippingAddressFirstName)),
                new SqlParameter("@ShippingAddressLastName", Utils.ToDBValue(shippingAddress.ShippingAddressLastName)),
                new SqlParameter("@ShippingAddressStreet1", Utils.ToDBValue(shippingAddress.ShippingAddressStreet1)),
                new SqlParameter("@ShippingAddressStreet2", Utils.ToDBValue(shippingAddress.ShippingAddressStreet2)),
                new SqlParameter("@ShippingAddressCity", Utils.ToDBValue(shippingAddress.ShippingAddressCity)),
                new SqlParameter("@ShippingAddressRegion", Utils.ToDBValue(shippingAddress.ShippingAddressRegion)),
                new SqlParameter("@ShippingAddressZip", Utils.ToDBValue(shippingAddress.ShippingAddressZip)),
                new SqlParameter("@ShippingAddressCountry", Utils.ToDBValue(shippingAddress.ShippingAddressCountry)),
                new SqlParameter("@ShippingAddressEmail", Utils.ToDBValue(shippingAddress.ShippingAddressEmail)),
                new SqlParameter("@ShippingAddressPhone", Utils.ToDBValue(shippingAddress.ShippingAddressPhone)),
                new SqlParameter("@CompanyID", Utils.ToDBValue(user.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(user.RegionID)),
                new SqlParameter("@UserID", Utils.ToDBValue(user.UserID))
            };

            db.QuerySP("CURATOR_SaveShippingAddress", parameters);

            if (!db.Success)
            {
                return new Exception(db.ErrorMessage);
            }
            else
            {
                return null;
            }
        }

        public static Exception SaveFraudCheck(int salesOrderID, FraudCheckInfo fraudCheck, UserInfo user)
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@SalesOrderID", Utils.ToDBValue(salesOrderID)),
                new SqlParameter("@PaymentMethod", Utils.ToDBValue(fraudCheck.PaymentMethod)),
                new SqlParameter("@FraudScore", Utils.ToDBValue(fraudCheck.FraudScore)),
                new SqlParameter("@CustomerIsNew", Utils.ToDBValue(fraudCheck.CustomerIsNew)),
                new SqlParameter("@ShippingAddressChecked", Utils.ToDBValue(fraudCheck.ShippingAddressChecked)),
                new SqlParameter("@CustomerIPAddressChecked", Utils.ToDBValue(fraudCheck.CustomerIPAddressChecked)),
                new SqlParameter("@FraudChecked", Utils.ToDBValue(fraudCheck.FraudChecked)),
                new SqlParameter("@CompanyID", Utils.ToDBValue(user.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(user.RegionID)),
                new SqlParameter("@UserID", Utils.ToDBValue(user.UserID))
            };

            db.QuerySP("CURATOR_SaveFraudCheck", parameters);

            if (!db.Success)
            {
                return new Exception(db.ErrorMessage);
            }
            else
            {
                return null;
            }
        }

        public static Exception SaveSalesOrderLine(SalesOrderLineInfo salesOrderLine, UserInfo user) 
        {
            var db = new DB(App.ProductsDBConn);
            var parameters = new[]
            {
                new SqlParameter("@SalesOrderID", Utils.ToDBValue(salesOrderLine.SalesOrderID)),
                new SqlParameter("@SalesOrderLineID", Utils.ToDBValue(salesOrderLine.SalesOrderLineID)),
                new SqlParameter("@WeightGrams", Utils.ToDBValue(salesOrderLine.WeightGrams)),
                new SqlParameter("@SubtotalAmount", Utils.ToDBValue(salesOrderLine.SubtotalAmount)),
                new SqlParameter("@DiscountAmount", Utils.ToDBValue(salesOrderLine.DiscountAmount)),
                new SqlParameter("@ShippingAmount", Utils.ToDBValue(salesOrderLine.ShippingAmount)),
                new SqlParameter("@TotalAmount", Utils.ToDBValue(salesOrderLine.TotalAmount)),
                new SqlParameter("@SupplierID", Utils.ToDBValue(salesOrderLine.SupplierID)),
                new SqlParameter("@CompanyID", Utils.ToDBValue(user.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(user.RegionID)),
                new SqlParameter("@UserID", Utils.ToDBValue(user.UserID))
            };

            db.QuerySP("CURATOR_SaveSalesOrderLine", parameters);

            if (!db.Success)
            {
                return new Exception(db.ErrorMessage);
            }
            else
            {
                return null;
            }
        }
    }
}