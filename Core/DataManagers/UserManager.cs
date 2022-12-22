using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class UserManager
	{
		public static UserInfo ValidateUser(string login, string password, out string error)
		{
			try
			{
				DB db = new DB(App.CuratorDBConn);

				var parameters = new[]
				{
					new SqlParameter("@Login", Utils.ToDBValue(login)),
					new SqlParameter("@Password", Utils.ToDBValue(U.Encrypt(password)))
				};

				DataTable dt = db.QuerySP("ValidateUser", parameters);

				if (!db.Success)
				{
					error = db.ErrorMessage;
					Log.Error(db.DBException);
					Log.Error(new Exception(db.ErrorMessage));
					return null;
				}

				error = null;

				return new UserInfo(dt.Rows[0]);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				error = "There was a problem loading your profile. Please contact support.";
				return null;
			}
		}

		public static UserInfo ValidateOidcUser(string emailAddress)
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@Email", Utils.ToDBValue(emailAddress))
			};

			var dt = db.QuerySP("ValidateOidcUser", parameters);
			if (!db.Success)
			{
				Log.Error(db.DBException);

				throw db.DBException;
			}

			return dt.Rows.Count > 0 ? new UserInfo(dt.Rows[0]) : null;
		}

		public static List<UserInfo> GetUsers(int companyID, int regionID)
		{
			return GetUsers(companyID, regionID, null, null, null, "General");
		} 

		public static List<UserInfo> GetUsers(int? companyID, int? regionID, string email)
		{
			return GetUsers(companyID, regionID, null, null, email, "EMAIL");
		}

		public static UserInfo GetLoggedInUser(int userID)
		{
			List<UserInfo> users = GetUsers(null, null, null, userID, null, "LOGGED-IN");

			if (users.Count != 1) return null;
			return users.First();
		}

		public static UserInfo GetUser(int? companyID, int userID)
		{
			List<UserInfo> users = GetUsers(companyID, null, null, userID, null, "General");

			if (users.Count != 1) return null;
			return users.First();
		}

		public static List<UserInfo> GetUsers(int? companyID, int? regionID, int? userGroupID, int? userID, string email, string type)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID)),
				new SqlParameter("@Email", Utils.ToDBValue(email)),
				new SqlParameter("@Type", Utils.ToDBValue(type))
			};

			DB db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetUsers", parameters);

			return new List<UserInfo>(from dr in dt.AsEnumerable() select new UserInfo(dr));
		}

		public static Exception Save(UserInfo info)
		{
			return Save(info, null);
		}

		public static Exception Save(UserInfo info, string regionIDs)
		{
			try
			{
				SqlParameter[] parameters =
				{
					new SqlParameter("@UserID", Utils.ToDBValue(info.UserID)),
					new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
					new SqlParameter("@Password", Utils.ToDBValue(U.Encrypt(info.Password))),
					new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
					new SqlParameter("@Position", Utils.ToDBValue(info.Position)),
					new SqlParameter("@Mobile", Utils.ToDBValue(info.Mobile)),
					new SqlParameter("@Telephone", Utils.ToDBValue(info.Telephone)),
					new SqlParameter("@Fax", Utils.ToDBValue(info.Fax)),
					new SqlParameter("@Email", Utils.ToDBValue(info.Email)),
					new SqlParameter("@UserGroupID", Utils.ToDBValue(info.UserGroupID)),					
					new SqlParameter("@Enabled", Utils.ToDBValue(info.Enabled)),
					new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
					new SqlParameter("@SendLoginDetailsEmail", Utils.ToDBValue(info.SendLoginDetailsEmail)),
					new SqlParameter("@LanguageID", Utils.ToDBValue(info.LanguageID)),
					new SqlParameter("@SalesRep", Utils.ToDBValue(info.SalesRep)),
					new SqlParameter("@OidcId", Utils.ToDBValue(info.OidcId)),
					new SqlParameter("@RegionIDs", Utils.ToDBValue(regionIDs)),
					new SqlParameter("@NewProductNotifications", Utils.ToDBValue(info.NewProductNotifications)),
					new SqlParameter("@ChangedProductNotifications", Utils.ToDBValue(info.ChangedProductNotifications)),
					new SqlParameter("@DeactivatedProductNotifications", Utils.ToDBValue(info.DeactivatedProductNotifications))
				};

				DB db = new DB(App.CuratorDBConn);
				DataTable dt = db.QuerySP("SaveUser", parameters);

				if (db.Success == false) return db.DBException;

				if (info.UserID < 0)
				{
					if (db.RowCount != 1) return new Exception("Unable to get new user ID");

					info.UserID = Utils.FromDBValue<int>(dt.Rows[0][0]);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);

				return ex;
			}

			return null;
		}

		public static Exception Delete(int companyID, int userID)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID))
			};

			DB db = new DB(App.CuratorDBConn);
			db.QuerySP("DeleteUser", parameters);
			return db.DBException;
		}

		public static List<UserInfo> GetJobStaffDetails(int companyID, int regionID, int jobScheduleID)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@JobScheduleID", Utils.ToDBValue(jobScheduleID))
			};

			DB db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetJobStaffDetails", parameters);

			return new List<UserInfo>(from dr in dt.AsEnumerable() select new UserInfo(dr));
		}

		#region Member Stores 
		public static Exception SaveMemberStores(int userID, string memberStoreIDList)
		{
			var db = new DB(App.ProductsDBConn);
			var parameters = new[]
			{
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@MemberStoreIDList", Utils.ToDBValue(memberStoreIDList))
				
			};

			db.QuerySP("CURATOR_SaveMemberStores", parameters);

			return db.DBException;
		}

		public static List<StoreInfo> GetMemberStores(int userID)
		{
			var db = new DB(App.ProductsDBConn);

			var parameters = new[]
			{
				new SqlParameter("@UserID",Utils.ToDBValue(userID))

			};
			var dt = db.QuerySP("CURATOR_GetMemberStores", parameters);

			return (from dr in dt.AsEnumerable() select new StoreInfo(dr)).OrderBy(x => x.StoreName).ToList();
		}

		public static List<StoreInfo> GetAvailableMemberStores()
		{
			var db = new DB(App.ProductsDBConn);
		
			var dt = db.QuerySP("CURATOR_GetAvailableMemberStores");

			return (from dr in dt.AsEnumerable() select new StoreInfo(dr)).ToList();
		}
        #endregion
	}
}
