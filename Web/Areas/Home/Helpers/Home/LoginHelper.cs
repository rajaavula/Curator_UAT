using System;
using System.Collections.Generic;
using System.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Home.Models;
using Microsoft.Owin.Security;

namespace LeadingEdge.Curator.Web.Home.Helpers
{
	public class LoginHelper
	{
		public static string Login_CaptchaText_Key = "Login_CaptchaText";

		public static HomeLogin CreateModel(string code)
		{
			var vm = new HomeLogin();
			vm.ReturnUrl = HttpContext.Current.Request.QueryString["ReturnUrl"];

			code = code?.ToUpper();

			switch (code)
			{
				case "010":
					vm.Message = "There was an unexpected error finding your SSO details. Please contact support.";
					break;

				case "020":
					vm.Message = "Your SSO user is not authorized to access the Leading Edge Curator. Please contact support.";
					break;

				case "030":
					vm.Message = "There was an unexpected error during the SSO login process. Please try again or contact support.";
					break;

				case "040":
					vm.Message = "Your SSO user session could not be created. Please contact support.";
					break;

				case "000":
					vm.Message = "There was an unexpected error while logging in. Please try again or contact support.";
					break;
			}

			return vm;
		}

		public static bool FormsLogin(HomeLogin vm)
		{
			string error;

			// Try and get user details using credentials
			var info = UserManager.ValidateUser(vm.Username, vm.Password, out error);

			if (info == null)
			{
				vm.Message = error;
				Log.Write(error);
				SecurityHelper.FormsLogout();
				return false;
			}

			// Set ASP .NET security principal stuff
			if (!SecurityHelper.FormsLogin(info.UserID, true))
			{
				vm.Message = "There was a security related error. Please contact support.";
				return false;
			}

			try
			{
				// If OK then try and create a session
				if (!App.CreateSession(info, out error))
				{
					Log.Error(new Exception(error));
					vm.Message = error;
					return false;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				vm.Message = ex.Message;
				return false;
			}


			return true;
		}

		public static bool CookieLogin(HomeLogin vm, IAuthenticationManager authentication)
		{
			string error;

			// Try and get user details using credentials
			var info = UserManager.ValidateUser(vm.Username, vm.Password, out error);

			if (info == null)
			{
				vm.Message = error;
				Log.Write(error);
				SecurityHelper.CookieLogout(authentication);
				return false;
			}

			// Set ASP .NET security principal stuff
			if (!SecurityHelper.CookieLogin(authentication, info.UserID, true))
			{
				vm.Message = "There was a security related error. Please contact support.";
				return false;
			}

			try
			{
				// If OK then try and create a session
				if (!App.CreateSession(info, out error))
				{
					Log.Error(new Exception(error));
					vm.Message = error;
					return false;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				vm.Message = ex.Message;
				return false;
			}


			return true;
		}

		public static Exception ForgottenPassword(string email)
		{
			try
			{
				if (string.IsNullOrEmpty(email)) return null;

				List<UserInfo> users = UserManager.GetUsers(null, null, email);
				if (users == null || users.Count < 1) throw new Exception("User not found");

				UserInfo user = users[0];
				if (user == null) throw new Exception("User not found");

				var body = EmailGenerator.FogottenPasswordEmail(users);
				if (string.IsNullOrEmpty(body)) throw new Exception("Could not generate login details email");

				// EDIT FOR CLIENT
				int? emailID = EmailManager.InsertEmail(user.Email, "Platform - Login Details", body, user.CompanyID, user.RegionID, "PASSWORD", user.UserID);
				if (!emailID.HasValue) throw new Exception("Could not send login details email");
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				return ex;
			}

			return null;
		}
	}
}