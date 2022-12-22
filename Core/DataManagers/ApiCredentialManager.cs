
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public static class ApiCredentialManager
    {
        #region Store

        public static List<ApiCredentialInfo> GetSupplierUsers()
        {
            var db = new DB(App.SuppliersDBConn);
            var dt = db.QuerySP("CURATOR_GetUsers");

            if (db.Success == false)
            {
                Log.Error(db.DBException);
                return null;
            }

            return (from dr in dt.AsEnumerable() select new ApiCredentialInfo(dr)).ToList();
        }

        public static Exception Save(ApiCredentialInfo info)
        {
            var db = new DB(App.SuppliersDBConn);

            var parameters = new[]
            {
                new SqlParameter("@UserKey", Utils.ToDBValue(info.UserKey)),
                new SqlParameter("@UserID",Utils.ToDBValue(info.UserID)),
                new SqlParameter("@EntityID",Utils.ToDBValue(info.EntityID)),
                new SqlParameter("@DisplayName",Utils.ToDBValue(info.DisplayName))
            };

            DataTable dt = db.QuerySP("CURATOR_SaveUser", parameters);

            return db.DBException;
        }

        public static Exception Delete(int userKey)
        {
            DB db = new DB(App.SuppliersDBConn);

            var parameters = new[]
            {
                new SqlParameter("@UserKey", Utils.ToDBValue(userKey))
            };

            db.QuerySP("CURATOR_DeleteUser", parameters);

            return db.DBException;
        }

        #endregion
    }
}
