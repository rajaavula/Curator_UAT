<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ApiCredentialsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/api-credentials.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "ApiCredentials", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option">
				<%
                    Html.Cortex().NeutralButton(true,s =>
                    {
						s.Name = "Refresh";
						s.Text = Model.LabelText(200034);
						s.UseSubmitBehavior = false;
						s.CausesValidation = false;
						s.ClientSideEvents.Click = "function (s,e) { RefreshGrid(GrdMain); }";
                    }).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(s =>
					{
						s.Name = "btnNew";
						s.Text = Model.LabelText(200036);
						s.UseSubmitBehavior = false;
						s.ClientSideEvents.Click = "function(s,e) { New(); }";
					}).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NegativeButton(s =>
					{
						s.Name = "btnDelete";
						s.Text = Model.LabelText(200324);
						s.UseSubmitBehavior = false;
						s.ClientSideEvents.Click = "function(s,e) { Delete(); }";
					}).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
   <table id="grid-container">
		<tr>
			<td id="grid-pane">
			<%
				Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);
			%>
			</td>
				<td class="cell">
				<div id="edit-pane">
					<label class="heading"><%= Model.Label(200325)%></label>       <!-- Edit Details -->
					<input id="UserKey" type="hidden" />
					
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(300098)%>:</td>     <!-- Object ID: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "UserID";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ValidationGroup = "SAVE";
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300099)%>:</td>     <!-- Entity ID: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "EntityID";
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300100)%>:</td>     <!-- Display name: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "DisplayName";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ValidationGroup = "SAVE";
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right"><%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);      // Cancel
								s.UseSubmitBehavior = false;
								s.CausesValidation = false;
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
