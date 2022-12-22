using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class LabelManager
	{
		public static List<LabelInfo> GetLabels(int companyID, string type)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@Type", Utils.ToDBValue(type))
			};

			DataTable dt = db.QuerySP("GetLabels", parameters);

			return (from dr in dt.AsEnumerable() select new LabelInfo(dr)).ToList();
		}

		public static LabelInfo GetLabel(int companyID, int placeholderID, string languageID)
		{
			var parameters = new[]
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)), 
				new SqlParameter("@PlaceholderID", Utils.ToDBValue(placeholderID)),
				new SqlParameter("@LanguageID", Utils.ToDBValue(languageID))
			};

			DB db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetLabel", parameters);

			if (db.Success && db.RowCount == 1) return new LabelInfo(dt.Rows[0]);

			return null;
		}

		public static string Save(int? labelID, LabelInfo info)
		{
			DB db = new DB(App.CuratorDBConn);

			var parameters = new[]
			{
				new SqlParameter("@LabelID", Utils.ToDBValue(labelID)),
				new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
				new SqlParameter("@PlaceholderID", Utils.ToDBValue(info.PlaceholderID)),
				new SqlParameter("@LanguageID", Utils.ToDBValue(info.LanguageID)),
				new SqlParameter("@LabelText", Utils.ToDBValue(info.LabelText)),
				new SqlParameter("@ToolTipText", Utils.ToDBValue(info.ToolTipText))
			};

			db.QuerySP("SaveLabel", parameters);

			return db.Success == false ? db.ErrorMessage : String.Empty;
		}

		public static string Delete(int labelID, int companyID)
		{
			DB db = new DB(App.CuratorDBConn);

			SqlParameter[] parameters = new[]
			{
				new SqlParameter("@LabelID", Utils.ToDBValue(labelID)),
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
			};

			db.QuerySP("DeleteLabel", parameters);

			return db.Success == false ? db.ErrorMessage : String.Empty;
		}

		public static void ReloadClientLabels(int companyID)
		{
			List<LabelInfo> labels = GetLabels(companyID, "DEFAULT");

			App.Labels.RemoveAll(x => x.CompanyID == companyID);
			App.Labels.AddRange(labels);
		}
	}
}
