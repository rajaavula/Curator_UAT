using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	class LayoutManager
	{

		public static Dictionary<string, string> GetGridLayouts(int companyID, int regionID, int userID)
		{
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
			};

			DB db = new DB(App.CuratorDBConn);
			DataTable dt = db.QuerySP("GetGridLayouts", parameters);

			if (!db.Success || db.RowCount == 0) return new Dictionary<string, string>();
			string gridLayoutString = "";
			foreach (DataRow dr in dt.AsEnumerable())
			{
				gridLayoutString += dr.ItemArray[0].ToString();
			}

			Dictionary<string, string> layout = DecodeGridLayouts(gridLayoutString);

			if (layout == null)
			{
				layout = new Dictionary<string, string>();
			}
			return layout;


		}
		public static Exception SaveGridLayout(int companyID, int regionID, int userID, Dictionary<string, string> gridLayout)
		{
			string encodedGridlayout = EncodeGridLayouts(gridLayout);
			SqlParameter[] parameters =
			{
				new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
				new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
				new SqlParameter("@UserID", Utils.ToDBValue(userID)),
				new SqlParameter("@GridLayout", Utils.ToDBValue(encodedGridlayout))
			};
			DB db = new DB(App.CuratorDBConn);
			db.QuerySP("SaveGridLayouts", parameters);
			return db.DBException;

		}

		//converts the layout dictionary into a base64 string to save to the database 
		public static string EncodeGridLayouts(Dictionary<string, string> gridLayouts)
		{
			string result = null;

			if (gridLayouts != null && gridLayouts.Count > 0)
			{
				List<string> values = new List<string>();

				foreach (var kv in gridLayouts)
				{
					values.Add(String.Format("{0}={1}", kv.Key, kv.Value));
				}

				if (values.Count > 0)
				{
					string encodedValue = String.Join("~", values);

					result = Utils.Base64Encode(encodedValue);
				}
			}

			return result;
		}

		// converts a base64 string into a dictionary of grid layouts 
		public static Dictionary<string, string> DecodeGridLayouts(string value)
		{
			Dictionary<string, string> gridLayouts = null;

			if (String.IsNullOrEmpty(value) == false)
			{
				gridLayouts = new Dictionary<string, string>();

				string result = Utils.Base64Decode(value);

				while (String.IsNullOrEmpty(result) == false)
				{
					int nextIndex = result.IndexOf('~');

					if (nextIndex == -1) nextIndex = result.Length;

					string current = result.Substring(0, nextIndex);

					int equalIndex = current.IndexOf('=');

					string modelName = current.Substring(0, equalIndex);

					int layoutStartIndex = (equalIndex + 1);

					string gridLayout = current.Substring(layoutStartIndex, current.Length - layoutStartIndex);

					if (String.IsNullOrEmpty(modelName) == false && String.IsNullOrEmpty(gridLayout) == false)
					{
						gridLayouts.Add(modelName, gridLayout);
					}

					int resultStartIndex = (nextIndex + 1);

					if (resultStartIndex > result.Length)
					{
						result = null;
					}

					else
					{
						result = result.Substring(resultStartIndex, result.Length - resultStartIndex);
					}
				}
			}

			return gridLayouts;
		}
	}
}
