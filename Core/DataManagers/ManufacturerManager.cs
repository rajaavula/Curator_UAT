using Cortex.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
    public static class ManufacturerManager
    {
		public static List<ManufacturerInfo> GetManufacturers()
		{
			var db = new DB(App.ProductsDBConn);

			var dt = db.QuerySP("CURATOR_GetManufacturers");

			return (from dr in dt.AsEnumerable() select new ManufacturerInfo(dr)).ToList();
        }

        public static List<ValueDescription> GetManufacturersList(bool nullOption)
        {
            var list = new List<ValueDescription>();

            if (nullOption) list.Add(new ValueDescription(null, null));

            var manufacturers = GetManufacturers().OrderBy(x => x.ManufacturerName).ToList();

            list.AddRange(from m in manufacturers select new ValueDescription(m.ManufacturerKey, m.ManufacturerName));

            return list;
        }
    }
}
