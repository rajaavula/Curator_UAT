using System.Data.SqlClient;

namespace Platform.Core
{
	public class DatabaseCsvArgs
	{
		public string Filename { get; set; }
		public string ConnectionString { get; set; }
		public string StoredProcedureName { get; set; }
		public SqlParameter[] SqlParameters { get; set; }
	}
}
