using System.Data.SqlClient;

namespace LeadingEdge.Curator.Core
{
	public class DatabaseCsvParameters
	{
		public string Filename { get; set; }
		public string ConnectionString { get; set; }
		public string StoredProcedureName { get; set; }
		public SqlParameter[] SqlParameters { get; set; }
	}
}
