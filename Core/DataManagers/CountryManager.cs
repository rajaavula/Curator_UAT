using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class CountryManager
	{
		public static List<CountryInfo> GetCountries()
		{
			try
			{
				DB db = new DB(App.CuratorDBConn);

				DataTable dt = db.QuerySP("GetCountries");

				return new List<CountryInfo>(from dr in dt.AsEnumerable() select new CountryInfo(dr));
			}
			catch(Exception ex)
			{
				Log.Error(ex);
			}

			return new List<CountryInfo>();
		}
	}
}
