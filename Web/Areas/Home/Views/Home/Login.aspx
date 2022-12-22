<%@ Page Language="C#" Inherits="BasePage<HomeLogin>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
	<link rel="icon" href="/favicon.ico" type="image/x-icon" />
	<title>Login | Leading Edge Curator</title>
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">

	<link href="/Assets/Css/login.css?<%= App.Version %>" rel="stylesheet" type="text/css" />
	<script src="/Assets/Lib/jquery/dist/jquery.min.js" type="text/javascript"></script>
	<script type="text/javascript" language="javascript" src="/Assets/Js/Home/login.js?<%= App.Version %>"></script><%
	Html.DevExpress().RenderStyleSheets(Page,
		new StyleSheet { ExtensionSuite = ExtensionSuite.NavigationAndLayout, Theme = App.Theme },
		new StyleSheet { ExtensionSuite = ExtensionSuite.Editors, Theme = App.Theme });
	Html.DevExpress().RenderScripts(Page,
		new Script { ExtensionSuite = ExtensionSuite.NavigationAndLayout },
		new Script { ExtensionSuite = ExtensionSuite.Editors });
%></head>
<body>
	 <div class="corner-ribbon top-right sticky blue shadow"><%= App.IsLive ? "" : "UAT"%></div>
	<div class="row">
		<div class="col-6 login-bk">
					</div>
		<div class="col-6 d-flex justify-content-center align-items-center">
			<div id="login-container">
				<form id="frmLogin" action="<%= Url.Action("Login", "Home") %>" method=	"post">
					<input type="hidden" name="ReturnUrl" value="<%: Model.ReturnUrl %>" />
					<div class="company-name logo">leading edge&nbsp;<span class="company-name logo" style="color:#fbb424">curator</span></div>
					<%
					if (App.BypassOpenIdConnect)
					{
					%>
					<table class="fields">
						<tr>
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "UserName";
									s.Width = 250;
									s.Attributes.Add("placeholder", "Username");
									s.Style.Add("padding", "5px");
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ValidationGroup = "MAIN";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
								}).Bind(Model.Username).Render();
							%></td>
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "Password";
									s.Width = 250;
									s.Attributes.Add("placeholder", "Password");
									s.Style.Add("padding", "5px");
									s.Properties.Password = true;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ValidationGroup = "MAIN";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
								}).Bind(Model.Password).Render();
							%></td>
						</tr>
						<tr>
							<td class="field" style="padding-left: 2px;"><%
								Html.Cortex().PositiveButton(s => {
									s.Name = "btnLogin";
									s.Text = "Login";
									s.Width = Unit.Pixel(250);
									s.Height = Unit.Pixel(40);
									s.ClientSideEvents.Click = "function (s, e) { Login(s, e); } ";
								}).Render();
							%></td>
						</tr>
				
					</table>
					<%
					}
					%>
					<p style="color: #ff0000; text-align: center;"><%= String.IsNullOrEmpty(Model.Message) ? "&nbsp;" : Model.Message %></p>
					<%
					if (!App.BypassOpenIdConnect)
					{
					%>
						<p style="color: #ff0000; text-align: center;"><a id="retry" href="/Login">Try&nbsp;Again</a></p>
					<%
                    }
					%>
					<p style="color: #ff0000; text-align: center;"><a id="contact" href="/Contact">Contact&nbsp;Support</a></p>
				</form>
			</div>			
		</div>
	</div>
	
</body>
</html>