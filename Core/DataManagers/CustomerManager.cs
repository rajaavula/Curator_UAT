using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class CustomerManager
    {
        public static List<CustomerInfo> GetCustomers(int companyID, int regionID, int? customerID, string type)
        {
            DB db = new DB(App.CuratorDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
                new SqlParameter("@CustomerID", Utils.ToDBValue(customerID)),
                new SqlParameter("@Type", Utils.ToDBValue(type))
            };

            DataTable dt = db.QuerySP("GetCustomers", parameters);
            return (from dr in dt.AsEnumerable() select new CustomerInfo(dr)).ToList();
        }

        public static List<CustomerInfo> GetCustomers(int companyID, int regionID)
        {
            return GetCustomers(companyID, regionID, null, null);
        }

        public static CustomerInfo GetCustomer(int companyID, int regionID, int customerID)
        {
            var customers = GetCustomers(companyID, regionID, customerID, null);

            if (customers.Count() == 1) return customers.First();

            return null;
        }

        public static Exception SaveCustomer(CustomerInfo info, int changeUserID)
        {
            DB db = new DB(App.CuratorDBConn);

            SqlParameter[] parameters =
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
                new SqlParameter("@CustomerID", Utils.ToDBValue(info.CustomerID)),
                new SqlParameter("@CustomerName", Utils.ToDBValue(info.CustomerName)),
                new SqlParameter("@CustomerPhone", Utils.ToDBValue(info.CustomerPhone)),
                new SqlParameter("@CustomerEmail", Utils.ToDBValue(info.CustomerEmail)),
                new SqlParameter("@ContactName", Utils.ToDBValue(info.ContactName)),
                new SqlParameter("@ContactNumber", Utils.ToDBValue(info.ContactNumber)),
                new SqlParameter("@Region", Utils.ToDBValue(info.Region)),
                new SqlParameter("@CurrencySymbol", Utils.ToDBValue(info.CurrencySymbol)),
                new SqlParameter("@EstimatedMonthlySpend", Utils.ToDBValue(info.EstimatedMonthlySpend)),
                new SqlParameter("@AssignedToUserID", Utils.ToDBValue(info.AssignedToUserID)),
                new SqlParameter("@AssignedToRepUserID", Utils.ToDBValue(info.AssignedToRepUserID)),
                new SqlParameter("@Enabled", Utils.ToDBValue(info.Enabled)),
                new SqlParameter("@Notes", Utils.ToDBValue(info.Notes)),
                new SqlParameter("@ChangeUserID", Utils.ToDBValue(changeUserID))
            };

            DataTable dt = db.QuerySP("SaveCustomer", parameters);

            if (db.Success == false) return db.DBException;

            if (db.RowCount != 1) return new Exception("Unable to get customer ID");

            info.CustomerID = Utils.FromDBValue<int>(dt.Rows[0][0]);
            
            return db.DBException;
        }

        public static Exception Delete(int companyID, int regionID, int customerID)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@CustomerID", Utils.ToDBValue(customerID))
			};

			DB db = new DB(App.CuratorDBConn);
			db.QuerySP("DeleteCustomer", parameters);
            
			return db.DBException;
		}
    }
}
