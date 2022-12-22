using System.Security.Principal;

namespace LeadingEdge.Curator.Core
{
	public class NotAuthenticatedPrincipal : IPrincipal
	{
		public bool IsInRole(string role)
		{
			return false;
		}

		public IIdentity Identity
		{
			get { return new NotAuthenticatedIdentity(); }
		}
	}
}