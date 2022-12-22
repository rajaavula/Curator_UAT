using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class FormManager
	{
		public static List<FormInfo> GetPermittedForms(int companyID, int userGroupID)
		{
			var db = new DB(App.CuratorDBConn);

			var paramsArray = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID))
			};

			var dt = db.QuerySP("GetPermittedForms", paramsArray);

			return new List<FormInfo>(from DataRow dr in dt.Rows select new FormInfo(dr));
		}

		public static List<FormInfo> GetForms()
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@Area", DBNull.Value),
				new SqlParameter("@Controller", DBNull.Value),
				new SqlParameter("@Action", DBNull.Value),
				new SqlParameter("@OwnersOnly", DBNull.Value),
				new SqlParameter("@IsDefault", DBNull.Value)
			};
			var dt = db.QuerySP("GetForms", parameters);

			return new List<FormInfo>(from DataRow dr in dt.Rows select new FormInfo(dr));
		}

		public static List<FormInfo> GetForms(bool ownersOnly, bool isDefault)
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@Area", DBNull.Value),
				new SqlParameter("@Controller", DBNull.Value),
				new SqlParameter("@Action", DBNull.Value),
				new SqlParameter("@OwnersOnly", Utils.ToDBValue(ownersOnly)),
				new SqlParameter("@IsDefault", Utils.ToDBValue(isDefault))
			};
			var dt = db.QuerySP("GetForms", parameters);

			return new List<FormInfo>(from DataRow dr in dt.Rows select new FormInfo(dr));
		}

		public static FormInfo GetForm(string area, string controller, string action)
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@Area", Utils.ToDBValue(area)),
				new SqlParameter("@Controller", Utils.ToDBValue(controller)),
				new SqlParameter("@Action", Utils.ToDBValue(action)),
				new SqlParameter("@OwnersOnly", DBNull.Value),
				new SqlParameter("@IsDefault", DBNull.Value)
			};

			var dt = db.QuerySP("GetForms", parameters);

			if (dt == null || dt.Rows.Count < 1) return null;

			return new FormInfo(dt.Rows[0]);
		}

		public static FormInfo GetForm(string area, string controller, string action, bool ownersOnly, bool isDefault)
		{
			var db = new DB(App.CuratorDBConn);

			var parameters = new []
			{
				new SqlParameter("@Area", Utils.ToDBValue(area)),
				new SqlParameter("@Controller", Utils.ToDBValue(controller)),
				new SqlParameter("@Action", Utils.ToDBValue(action)),
				new SqlParameter("@OwnersOnly", Utils.ToDBValue(ownersOnly)),
				new SqlParameter("@IsDefault", Utils.ToDBValue(isDefault))
			};

			var dt = db.QuerySP("GetForms", parameters);

			if (dt == null || dt.Rows.Count < 1) return null;

			return new FormInfo(dt.Rows[0]);
		}

		public static void UpdatePermittedForms(int userGroupID, int companyID, string formIDs)
		{
			var sqlParameters = new[]
			{
				new SqlParameter("@UserGroupID", Utils.ToDBValue(userGroupID)),
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@FormIDs", Utils.ToDBValue(formIDs)),
			};

			var db = new DB(App.CuratorDBConn);

			db.QuerySP("UpdateFormPermissions", sqlParameters);

			if (db.Success == false) Log.Error(new Exception(db.ErrorMessage));
		}
	}
}
