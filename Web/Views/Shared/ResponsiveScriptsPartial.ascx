<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript" src="/Assets/Lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="/Assets/Lib/select2/dist/js/select2.min.js"></script>
<%
if (App.IsLive)
{
%>
    <script type="text/javascript" src="/Assets/Js/responsive.min.js?<%= App.Version %>"></script>
<%
}
else
{
%>
    <script type="text/javascript" src="/Assets/Js/responsive.js?<%= App.Version %>"></script>
<%
}
%>