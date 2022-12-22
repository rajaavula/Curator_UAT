<%@ Page Language="C#" Inherits="BasePage<HomeContact>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
	<link rel="icon" href="/favicon.ico" type="image/x-icon" />
	<title>Contact | Leading Edge Curator</title>
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">

	<link href="/Assets/Css/login.css?<%= App.Version %>" rel="stylesheet" type="text/css"/>
	<script src="/Assets/Lib/jquery/dist/jquery.min.js" type="text/javascript"></script>
	<script type="text/javascript" language="javascript" src="/Assets/Js/Home/contact.js?<%= App.Version %>"></script><%
	Html.DevExpress().RenderStyleSheets(Page,
		new StyleSheet { ExtensionSuite = ExtensionSuite.NavigationAndLayout, Theme = App.Theme },
		new StyleSheet { ExtensionSuite = ExtensionSuite.Editors, Theme = App.Theme });
	Html.DevExpress().RenderScripts(Page,
		new Script { ExtensionSuite = ExtensionSuite.NavigationAndLayout },
		new Script { ExtensionSuite = ExtensionSuite.Editors });
%></head>
<body>
	<div class="row">
		<div class="col-6 login-bk">
					</div>
		<div class="col-6 d-flex justify-content-center align-items-center">	
			<p>Contact us page</p>
		</div>
	</div>
</body>
</html>
