using System.Security.Principal;
using System.Web.Security;

namespace LeadingEdge.Curator.Core
{
	internal class NotAuthenticatedIdentity : IIdentity
	{
		public string Name
		{
			get { return FormsAuthentication.FormsCookieName; }
		}

		public string AuthenticationType
		{
			get { return FormsAuthentication.FormsCookieName; }
		}

		public bool IsAuthenticated
		{
			get { return false; }
		}
	}
}