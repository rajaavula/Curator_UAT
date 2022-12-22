<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<UserGroupPermissionsList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link type="text/css" rel="stylesheet" href="/Assets/Css/assign-list.css?<%= App.Version %>"/>
	<script type="text/javascript" src="/Assets/Js/Admin/assign-list.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content  ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "UserGroupPermissions", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200202)%>:</label></td><!-- User Group -->
				<td class="option"><%
					Html.Cortex().ComboBox(s => {
						s.Name = "UserGroupID";
						s.Properties.TextField = "Name";
						s.Properties.ValueField = "UserGroupID";
						s.Properties.ValueType = typeof (int);
                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeGroup(); }";
					}).BindList(Model.UserGroups).Bind(Model.UserGroupID).Render();
				%></td>
				<td class="option"><%
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
	<input id="assign-userobjectids" name="UserObjectIDs" type="hidden" value="<%: Model.UserObjectIDs %>" />
	<input id="assign-issave" name="IsSave" type="hidden" value="<%: Model.IsSave %>" />
	<table id="assign-access">
		<tr>
			<td class="assign-list">
				<h3><%= Model.Label(200433)%></h3> <!-- Available Permissions -->
				<div class="table-container">
					<table id="available">
						<% foreach (UserGroupPermissionInfo p in Model.AvailableUserGroupPermissions) { %>
						<tr id="<%: p.UserObjectID %>" class="assign">
							<td class="area"><%: p.Type %></td>
							<td class="group"><%: p.Group %></td>
							<td class="name"><%: p.Name %></td>
							<td class="link"><a href="#"><%= Model.Label(200374)%></a></td>
						</tr>
						<% } %>
					</table>
				</div>
			</td>
			<td class="assign-list">
				<h3><%= Model.Label(200435)%></h3> <!-- Assigned Permissions -->
				<div class="table-container">
					<table id="assigned">
						<% foreach (UserGroupPermissionInfo p in Model.AssignedUserGroupPermissions) { %>
						<tr id="<%: p.UserObjectID %>" class="assign">
							<td class="area"><%: p.Type %></td>
							<td class="group"><%: p.Group %></td>
							<td class="name"><%: p.Name %></td>
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
