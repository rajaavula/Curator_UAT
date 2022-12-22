using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class UserGroupManager
	{
		public static List<UserGroupInfo> GetUserGroups(int companyID, int userUserGroupID)
		{
			DB db = new DB(App.CuratorDBConn);
			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserGroupID", DBNull.Value),
				new SqlParameter("@UserUserGroupID", Utils.ToDBValue(userUserGroupID))
			};

			DataTable dt = db.QuerySP("GetUserGroups", parameters);
			
			return (from dr in dt.AsEnumerable() select new UserGroupInfo(dr)).ToList();
		}

		public static UserGroupInfo GetUserGroup(int companyID, int userGroupID, int userUserGroupID)
		{
			DB db = new DB(App.CuratorDBConn);
			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID)),
				new SqlParameter("@UserUserGroupID", Utils.ToDBValue(userUserGroupID))
			};

			DataTable dt = db.QuerySP("GetUserGroups", parameters);
			if (!db.Success || db.RowCount == 0) return null;

			return new UserGroupInfo(dt.Rows[0]);
		}

		public static Exception Save(int? userGroupID, UserGroupInfo info)
		{
			DB db = new DB(App.CuratorDBConn);
			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID)),
				new SqlParameter("@Name", Utils.ToDBValue(info.Name)),
				new SqlParameter("@Description", Utils.ToDBValue(info.Description)),
				new SqlParameter("@IsOwner", Utils.ToDBValue(info.IsOwner)),
				new SqlParameter("@IsWorker", Utils.ToDBValue(info.IsWorker)), 
				new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID))
			};

			db.QuerySP("SaveUserGroup", parameters);

			return db.DBException;
		}

		public static Exception Delete(int companyID, int userGroupID )
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID))
			};

			db.QuerySP("DeleteUserGroup", parameters);

			return db.DBException;
		}
	}
}
