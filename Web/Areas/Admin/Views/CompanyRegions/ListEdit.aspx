<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<CompanyRegionsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/company-regions.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "CompanyRegions", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content  ContentPlaceHolderID="HeaderContent" runat="server">
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
					<label class="heading"><%= Model.Label(200325)%></label> <!-- Edit Details -->
					<input id="RegionID" type="hidden" />
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200016)%>:</td> <!-- Name: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "Name";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200464)%>:</td> <!-- Copy Regional Email To: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "CopyRegionalEmailTo";
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label wrap"><%= Model.Label(200640)%>:</td> <!-- Sales support email address: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "SalesSupportEmailAddress";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label wrap"><%= Model.Label(200698)%>:</td> <!-- Purchasing dept. email address: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "PurchasingDeptEmailAddress";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200663)%>:</td> <!-- EmailServer: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "EmailServer";
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200664)%>:</td> <!-- EmailUsername: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "EmailUsername";
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200665)%>:</td> <!-- EmailPassword: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s =>
									{
									s.Name = "EmailPassword";
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200666)%>:</td> <!-- EmailFromEmail: -->
						</tr>
						<tr>        
							<td class="field"><%
									Html.Cortex().TextBox(s =>
									{
									s.Name = "EmailFromEmail";
									s.Width = Unit.Percentage(100);
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200668)%>:</td> <!-- EmailFromName: -->
						</tr>
						<tr>        
							<td class="field"><%
									Html.Cortex().TextBox(s =>
								{
									s.Name = "EmailFromName";
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200076)%>:</td> <!-- Notes: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().Memo(s => {
									s.Name = "Notes";
									s.Height = 100;
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right"><%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);                // Cancel
								s.CausesValidation = false;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Get(); }";
							},!Model.HasPermission("RESTRICTCLIENTS")).Render();
						%></div>
						<div class="button-right"><%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);                  // Save
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Save(); }";
							},!Model.HasPermission("RESTRICTCLIENTS")).Render();
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
