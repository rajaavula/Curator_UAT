using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace LeadingEdge.Curator.Core
{
	public static class EmailGenerator
	{
		public static string GetEmailCss()
		{
			StringBuilder html = new StringBuilder();
			html.Append("html, body { font-family: Arial, Verdana, Calibri; color: #545442; font-size: 14px; padding: 0; margin: 0; }");
			html.Append("a:link { color: #3366cc; }");
			html.Append(".main { border-collapse: collapse; border-spacing: 0; padding: 0; margin: 0; }");
			html.Append(".main .title, .main .value { vertical-align: top; text-align: left; }");
			html.Append(".main .title { font-weight: bold; padding: 8px 12px 8px 6px; background-color: #EEE; border-left: 1px solid #CCC; border-right: 1px solid #CCC; white-space: nowrap; }");
			html.Append(".main .value { padding: 8px 6px 8px 12px; }");
			html.Append("hr { border: none; border-top: 1px solid #CCC; height: 0; padding: 0; margin: 0; }");
			html.Append("h1 { margin: 0; }");
			html.Append("h1, h1 span { font-size: 18px; color:#fbb424; border-bottom:1px solid #fbb424; padding-bottom: 10px; }");
			html.Append("h2 { font-size: 18px; }");
			html.Append("h3 { font-size: 16px; }");
			html.Append(".error { color: #ff0000; } ");
			html.Append(".good { color: #24AB51; } ");
			html.Append(".items-table { border-collapse: collapse; border-spacing: 0; padding: 0; margin: 0; }");
			html.Append(".items-table th, .items-table td { padding: 10px 8px; vertical-align: top; text-align: left; }");
			html.Append(".items-table th { background-color: #EEE; border: 1px solid #CCC; }");
			html.Append(".items-table td { border: 1px solid #CCC; background-color: #FFF; }");
			html.Append(".items-table td .items-table { border: none; width: 100%; }");
			html.Append(".items-table td .items-table td { border: 1px solid #EEEEEE; padding: 2px 6px; }");
			html.Append(".signature { border-top: 1px solid #fbb424; padding-top: 15px;color: #fbb424; }");
			html.Append(".signature-sa { color: #fbb424; font-weight: bold; font-size:20px }");
			html.Append("table tr th { padding-bottom: 10px; padding-right: 15px; text-align:left }");
			html.Append("table tr td { padding-right: 15px; }");
			html.Append(".text-right { text-align: right; }");
			return html.ToString();
		}

		public static string SystemLoginEmail(UserInfo user)
		{
			CompanyInfo company = CompanyManager.GetCompany(user.CompanyID, false);
			if (company == null) return null;

			string link = App.ApplicationUrl;

			StringBuilder html = new StringBuilder();

			html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			html.Append("<head>");
			html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=us-ascii\">");
			html.AppendFormat("<title>{0} - Login Details</title>", App.PlatformName);
			html.AppendFormat("<style type=\"text/css\">{0}</style>", GetEmailCss());
			html.Append("</head>");
			html.Append("<body>");
			html.Append("<h1>Login Details</h1>");
			html.AppendFormat("<p>Once logged into {0} please change your password.</p>", App.PlatformName);
			html.Append("<p>Your login details are as follows:</p>");
			html.Append("<table class=\"main\">");
			html.Append("<tr>");
			html.Append("<td class=\"title\" style=\"border-top: 1px solid #CCC;\">Company:</td>");
			html.AppendFormat("<td class=\"value\">{0}</td>", company.Name);
			html.Append("</tr>");
			html.Append("<tr>");
			html.Append("<td class=\"title\">Username:</td>");
			html.AppendFormat("<td class=\"value\">{0}</td>", user.Login);
			html.Append("</tr>");
			html.Append("<tr>");
			html.Append("<td class=\"title\" style=\"border-bottom: 1px solid #CCC;\">Password:</td>");
			html.AppendFormat("<td class=\"value\">{0}</td>", user.Password);
			html.Append("</tr>");
			html.Append("</table>");
			html.AppendFormat("<p><a href=\"{0}\">Click here</a> to login using the username and password provided above.</p>", link);
			html.Append("</body>");
			html.Append("</html>");

			return html.ToString();
		}

		public static string FogottenPasswordEmail(List<UserInfo> users)
		{
			if (users == null || users.Count < 1) return null;

			StringBuilder html = new StringBuilder();

			html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			html.Append("<head>");
			html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=us-ascii\">");
			html.AppendFormat("<title>{0} - Login Details</title>", App.PlatformName);
			html.AppendFormat("<style type=\"text/css\">{0}</style>", GetEmailCss());
			html.Append("</head>");
			html.Append("<body>");
			html.Append("<h1>Login Details</h1>");
			html.AppendFormat("<p>Once logged into {0} please change your password.</p>", App.PlatformName);
			html.Append("<p>Your Platform login details are as follows:</p>");

			bool isFirst = true;
			foreach (UserInfo user in users)
			{
				if (user == null) continue;

				CompanyInfo company = CompanyManager.GetCompany(user.CompanyID, false);
				if (company == null) return null;

				string link = App.ApplicationUrl;

				if (!isFirst) html.Append("<br/>");

				html.Append("<table class=\"main\">");
				html.Append("<tr>");
				html.Append("<td class=\"title\" style=\"border-top: 1px solid #CCC;\">Company:</td>");
				html.AppendFormat("<td class=\"value\">{0}</td>", company.Name);
				html.Append("</tr>");
				html.Append("<tr>");
				html.Append("<td class=\"title\">Username:</td>");
				html.AppendFormat("<td class=\"value\">{0}</td>", user.Login);
				html.Append("</tr>");
				html.Append("<tr>");
				html.Append("<td class=\"title\">Password:</td>");
				html.AppendFormat("<td class=\"value\">{0}</td>", user.Password);
				html.Append("</tr>");
				html.Append("<tr>");
				html.Append("<td class=\"title\" style=\"border-bottom: 1px solid #CCC;\">Link:</td>");
				html.AppendFormat("<td class=\"value\"><a href=\"{0}\">Click here</a> to login using the username and password provided above.</td>", link);
				html.Append("</tr>");
				html.Append("</table>");

				if (isFirst) isFirst = false;
			}

			html.Append("</body>");
			html.Append("</html>");

			return html.ToString();
		}

		public static string SecurityAdminAccessLogEmail(CompanyInfo company, List<UserLoginHistoryInfo> userLoginHistory)
		{
			if (company == null || userLoginHistory == null) return null;

			StringBuilder html = new StringBuilder();

			html.Append("<!DOCTYPE) html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			html.Append("<head>");
			html.AppendFormat("<title>{0} User Activity Summary</title>", App.PlatformName);
			html.Append("<style type=\"text/css\">");
			html.Append("body { font-family: Arial, Verdana, Calibri; color: #545442; }");
			html.Append("a:link { color: #3366cc; }");
			html.Append(".header { font-size: 16pt; }");
			html.Append(".style1 { height: 25px; width: 150px; background-color: #ffcc99; }");
			html.Append(".style2 { height: 25px; width: 200px; }");
			html.Append(".RedStyle { color: red; }");
			html.Append(".GreenStyle { color: green; }");
			html.Append("</style>");
			html.Append("</head>");
			html.Append("<body>");
			html.AppendFormat("<p class=\"header\">User Activity Summary for {0}</p>", company.Name);

			html.AppendLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
			html.AppendFormat("<tr><td class=\"CellStyle\" colspan=\"3\">Please find below a list of all current active users on {0}.</td></tr>", App.PlatformName);
			html.AppendLine("<tr><td colspan=\"3\"><br/></td></tr>");
			html.AppendLine("<tr><td class=\"GreenStyle\" colspan=\"3\">Green = Logged in within last 7 days</td></tr>");
			html.AppendLine("<tr><td class=\"RedStyle\" colspan=\"3\">Red = Never logged in</td></tr>");
			html.AppendLine("<tr><td colspan=\"3\"><br/></td></tr>");
			html.AppendLine("<tr><td class=\"CellHeaderStyle\" width=\"30%\">Region</td>");
			html.AppendLine("<td class=\"CellHeaderStyle\"  width=\"50%\">User</td>");
			html.AppendLine("<td class=\"CellHeaderStyle\"  width=\"20%\">Last Login</td></tr>");

			int lastRegionID = -1;

			foreach (UserLoginHistoryInfo info in userLoginHistory)
			{
				if (info.RegionID != lastRegionID && lastRegionID != 1) html.AppendLine("<tr><td colspan=\"3\"><hr></td></tr>");

				html.AppendLine("<tr>");

				if (info.LastLogin == null)
				{
					html.AppendLine(String.Format("<td class=\"RedStyle\" >{0}</td>", info.RegionName));
					html.AppendLine(String.Format("<td class=\"RedStyle\" >{0}</td>", info.UserName));
					html.AppendLine("<td class=\"RedStyle\" ></td>");
				}
				else
				{
					if (info.LastLogin >= DateTime.Today.AddDays(-7))
					{
						html.AppendLine(String.Format("<td class=\"GreenStyle\" >{0}</td>", info.RegionName));
						html.AppendLine(String.Format("<td class=\"GreenStyle\" >{0}</td>", info.UserName));
						html.AppendLine(String.Format("<td class=\"GreenStyle\" >{0:dd/MM/yyyy HH:mm}</td>", info.LastLogin));
					}
					else
					{
						html.AppendLine(String.Format("<td class=\"CellStyle\" >{0}</td>", info.RegionName));
						html.AppendLine(String.Format("<td class=\"CellStyle\" >{0}</td>", info.UserName));
						html.AppendLine(String.Format("<td class=\"CellStyle\" >{0:dd/MM/yyyy HH:mm}</td>", info.LastLogin));
					}
				}

				html.AppendLine("</tr>");

				lastRegionID = info.RegionID;
			}

			html.AppendLine("</table>");
			html.Append("</body>");
			html.Append("</html>");

			return html.ToString();
		}

        public static string PushToSupplierEmail(SalesOrderInfo order, StoreInfo store)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            html.Append("<head>");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=us-ascii\">");
            html.AppendFormat("<title>{0} - Supplier Order</title>", App.PlatformName);
            html.AppendFormat("<style type=\"text/css\">{0}</style>", GetEmailCss());
            html.Append("</head>");
            html.Append("<body>");
            html.Append("<h1>Dear Supplier</h1>");
            html.AppendFormat("<p>Please find the attached purchase order from the {0} store.</p>", store.StoreName);
            html.Append("<p>For more information, please contact us on 0406920169 or raise a ticket to marketplace@leadingedgegroup.com.au</p>");
            html.Append("</body>");
            html.Append("</html>");

            return html.ToString();
        }
    }
}
