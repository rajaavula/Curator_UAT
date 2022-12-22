using System;
using System.Security.Principal;

namespace LeadingEdge.Curator.Core
{
	public class Identity : IIdentity
	{
		public string Name { get; private set; }

		public string AuthenticationType
		{
			get { return "User"; }
		}

		public bool IsAuthenticated
		{
			get { return (String.IsNullOrEmpty(Name) == false); }
		}

		public Identity(string userID)
		{
			if (String.IsNullOrEmpty(userID))
			{
				throw new ArgumentNullException("userID");
			}

			Name = userID;
		}
	}
}
