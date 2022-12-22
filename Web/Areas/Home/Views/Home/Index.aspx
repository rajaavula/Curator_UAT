<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<HomeIndex>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link href="/Assets/Css/home.css?<%= App.Version %>" rel="stylesheet" type="text/css" />
	<script src="/Assets/Js/Home/home.js?<%= App.Version %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<!-- <h2 class="page-title"><%= Model.Label(200000) %></h2> -->
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<div class="home-background">  
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
</asp:Content>
