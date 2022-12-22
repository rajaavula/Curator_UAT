using System;

namespace LeadingEdge.Curator.Web
{
	public class OidcException : Exception
	{
		public OidcException(string code) : base("There was an OIDC error.")
		{
			Code = code;
        }

		public string Code { get; }
	}
}
