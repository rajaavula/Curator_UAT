using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public static class LanguageManager
	{
		public static List<LanguageInfo> GetLanguages()
		{
			DB db = new DB(App.CuratorDBConn);

			DataTable dt = db.QuerySP("GetLanguages");

			if (!db.Success || db.RowCount == 0) return new List<LanguageInfo>();

			return (from dr in dt.AsEnumerable() select new LanguageInfo(dr)).ToList();
		}
	}
}
