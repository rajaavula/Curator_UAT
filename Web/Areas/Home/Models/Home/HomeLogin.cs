namespace LeadingEdge.Curator.Web.Home.Models
{
	public class HomeLogin : BaseModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string CaptchaText { get; set; }
		public string Message { get; set; }
		public string ReturnUrl { get; set; }
	}
}