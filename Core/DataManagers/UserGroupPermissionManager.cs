using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class UserGroupPermissionManager
	{
		public static List<UserGroupPermissionInfo> GetUserGroupPermissions(int companyID)
		{
			try
			{
				DB db = new DB(App.CuratorDBConn);

				SqlParameter[] parameters = new[]
				{
					new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
					new SqlParameter("@UserGroupID", DBNull.Value)
				};

				DataTable dt = db.QuerySP("GetUserGroupPermissions", parameters);

				if (!db.Success || db.RowCount == 0) return new List<UserGroupPermissionInfo>();

				return new List<UserGroupPermissionInfo>(from dr in dt.AsEnumerable() select new UserGroupPermissionInfo(dr));
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return new List<UserGroupPermissionInfo>();
		}

		public static List<UserGroupPermissionInfo> GetUserGroupPermissions(int companyID, int userGroupID)
		{
			try
			{
				DB db = new DB(App.CuratorDBConn);

				SqlParameter[] parameters = new[]
				{
					new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
					new SqlParameter("@UserGroupID", userGroupID)
				};

				DataTable dt = db.QuerySP("GetUserGroupPermissions", parameters);

				if (!db.Success || db.RowCount == 0) return new List<UserGroupPermissionInfo>();

				return new List<UserGroupPermissionInfo>(from dr in dt.AsEnumerable() select new UserGroupPermissionInfo(dr));
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return new List<UserGroupPermissionInfo>();
		}

		public static void UpdateUserGroupPermissions(int companyID, int userGroupID, string userObjectIDs)
		{
			try
			{
				var sqlParameters = new[]
				{
					new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID)),
					new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
					new SqlParameter("@UserObjectIDs", Utils.ToDBValue(userObjectIDs)),
				};

				var db = new DB(App.CuratorDBConn);

				db.QuerySP("UpdateUserGroupPermissions", sqlParameters);

				if (db.Success == false) throw new Exception(db.ErrorMessage);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
		}
	}
}
