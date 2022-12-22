<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<MemberFeeds>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link type="text/css" rel="stylesheet" href="/Assets/Css/assign-list.css?<%= App.Version %>"/>
	<script type="text/javascript" src="/Assets/Js/Admin/assign-list.js?<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/MemberFeeds/memberfeeds.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "MemberFeeds", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(300071)%>:</label></td><!-- Store -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "StoreID";
						s.Properties.ValueField = "StoreID";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "StoreName";
						s.Width = Unit.Pixel(150);						
                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeMemberStore(); }";
					}).BindList(Model.MemberStoreList).Bind(Model.StoreID).Render();
				%>
				</td>
							
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);						
					}).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "btnSave";
						settings.Text = Model.LabelText(200247);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { Save(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <input id="assign-userobjectids" name="FeedKeys" type="hidden" value="<%: Model.FeedKeys %>" />
	<input id="assign-issave" name="IsSave" type="hidden" value="<%: Model.IsSave %>" />
	<table id="assign-access">
		<tr>
			<td class="assign-list">
				<h3><%= Model.Label(300011)%></h3> <!-- Available feeds -->
				<div class="table-container">
					<table id="available">
						<% foreach (FeedInfo p in Model.AvailableFeeds) { %>
						<tr id="<%: p.FeedKey %>" class="assign">
							<td class="name"><%: p.FeedName %></td>
							<td class="link"><a href="#"><%= Model.Label(200374)%></a></td>
						</tr>
						<% } %>
					</table>
				</div>
			</td>
			<td class="assign-list">
				<h3><%= Model.Label(300012)%></h3> <!-- Assigned feeds -->
				<div class="table-container">
					<table id="assigned">
						<% foreach (FeedInfo p in Model.AssignedFeeds) { %>
						<tr id="<%: p.FeedKey %>" class="assign">
							<td class="name"><%: p.FeedName %></td>
							<td class="link"><a href="#"><%= Model.Label(200375)%></a></td>
						</tr>
						<% } %>
					</table>
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>
