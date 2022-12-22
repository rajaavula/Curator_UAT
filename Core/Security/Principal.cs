using System.Security.Principal;

namespace LeadingEdge.Curator.Core
{
	public class Principal : IPrincipal
	{
		private readonly Identity identity;

		public Principal(Identity identity)
		{
			this.identity = identity;
		}

		public bool IsInRole(string role)
		{
			return true;
		}

		public IIdentity Identity
		{
			get { return identity; }
		}
	}
}
