using System.Data;

namespace Utility.Portal.Web.Admin.Models
{
	public class UserRecentBookings : BaseModel
	{
		public int UserID { get; set; }
		public DataTable Bookings { get; set; }

		public UserRecentBookings()
		{
			Bookings = new DataTable();
		}
	}
}