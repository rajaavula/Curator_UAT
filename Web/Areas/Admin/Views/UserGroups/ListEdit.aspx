<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<UserGroupsListEdit>" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/user-groups.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content  ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "UserGroups", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGrid(GrdMain); }";
					}).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "btnNew";
						settings.Text = Model.LabelText(200036);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { New(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NegativeButton(settings =>
					{
						settings.Name = "btnDelete";
						settings.Text = Model.LabelText(200324);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { Delete(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<table id="grid-container">
		<tr>
			<td class="cell">
				<div id="grid-pane">
					<%Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);%>	
				</div>
			</td>
			<td class="cell">
				<div id="edit-pane">
					<label class="heading"><%= Model.Label(200325)%></label>       <!-- Edit Details -->
					<input id="UserGroupID" type="hidden" />
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200338)%>:</td>       <!-- Group Name: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "UserGroupName";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200196)%>:</td>       <!-- Description: -->
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "Description";
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().CheckBox(s => {
									s.Name = "IsOwner";
									s.Text = Model.Label(200339);		// Is owner
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().CheckBox(s => {
									s.Name = "IsWorker";
									s.Text = Model.Label(200807);		// Is Worker
								}).Render();
							%></td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right"><%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);      // Cancel
								s.CausesValidation = false;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Get(); }";
							}).Render();
						%></div>
						<div class="button-right"><%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);      // Save;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Save(); }";
							}).Render();
						%></div>
						<div class="clear"></div>
					</div>
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>