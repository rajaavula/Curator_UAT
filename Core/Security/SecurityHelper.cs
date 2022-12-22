using LeadingEdge.Core.Claims;
using LeadingEdge.Core.Claims.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace LeadingEdge.Curator.Core
{
	public static class SecurityHelper
	{
		private static IPrincipal Principal
		{
			set
			{
				HttpContext.Current.User = value;
				Thread.CurrentPrincipal = value;
			}
		}

		public static bool AuthenticateFormsRequest()
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (cookie == null)
			{
				Principal = new NotAuthenticatedPrincipal();
				return false;
			}

			FormsAuthenticationTicket ticket;
			try
			{
				ticket = FormsAuthentication.Decrypt(cookie.Value);
			}
			catch
			{
				ticket = null;
			}

			if (ticket == null || ticket.Expired)
			{
				Principal = new NotAuthenticatedPrincipal();
				return false;
			}

			string userID = ticket.Name;
			bool isValidUser = IsValidUser(userID);
			if (isValidUser)
			{
				Principal = new Principal(new Identity(userID));
			}
			else
			{
				Principal = new NotAuthenticatedPrincipal();
			}

			return isValidUser;
		}

		public static bool FormsLogin(int userID, bool rememberMe)
		{
			try
			{
				string sUserID = Convert.ToString(userID);
				DateTime expiryTime = (rememberMe ? DateTime.Now.AddYears(1) : DateTime.Now.AddMinutes(App.SessionTimeoutMinutes));

				FormsAuthentication.Initialize();
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, sUserID, DateTime.Now, expiryTime, rememberMe, string.Empty, FormsAuthentication.FormsCookiePath);
				string encryptedTicket = FormsAuthentication.Encrypt(ticket);
				HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
				cookie.HttpOnly = true; // prevent any attacker from accessing the cookie with client side scripts

				if (HttpContext.Current.Request.IsSecureConnection)
				{
					cookie.Secure = true; // prevent surf jacking
				}
				if (rememberMe) cookie.Expires = expiryTime;
				HttpContext.Current.Response.Cookies.Add(cookie);
				Principal = new Principal(new Identity(sUserID));
			}
			catch
			{
				return false;
			}

			return true;
		}

		public static void FormsLogout()
		{
			var SI = (SessionInfo)HttpContext.Current.Session["sSessionInfo"];
			App.RemoveSession(SI);

			Principal = new NotAuthenticatedPrincipal();
			FormsAuthentication.SignOut();
		}

		public static bool AuthenticateCookieRequest(IAuthenticationManager authentication)
		{
			return authentication.User.Identity.IsAuthenticated;
		}

		public static bool CookieLogin(IAuthenticationManager authentication, int userID, bool rememberMe)
		{
			try
			{
				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType);

				identity.AddClaim(new Claim(JwtClaimNames.CortexId, userID.ToString()));

				authentication.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public static void CookieLogout(IAuthenticationManager authentication)
		{
			var SI = (SessionInfo)HttpContext.Current.Session["sSessionInfo"];

			App.RemoveSession(SI);

			authentication.SignOut(authentication.GetAuthenticationTypes().Select(o => o.AuthenticationType).ToArray());

			HttpContext.Current.Session.Abandon();
		}

		public static int? GetUserId(IAuthenticationManager authentication)
		{
			return authentication.User.GetCortexId();
		}

		private static bool IsValidUser(string userID)
		{
			return (string.IsNullOrEmpty(userID) == false);
		}
	}
}
