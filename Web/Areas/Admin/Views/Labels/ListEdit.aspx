<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<LabelsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/labels.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Labels", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200330)%></label></td><!-- Edit labels for -->
				<td class="option"><%
					Html.Cortex().ComboBox(s => {
						s.Name = "SelectedCompanyID";
						s.Properties.ValueField = "CompanyID";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "Name";
						s.Width = Unit.Pixel(140);
                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeGroup(); }";
					}).BindList(Model.Companies).Bind(Model.SelectedCompanyID).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NeutralButton(true,s =>
					{
						s.Name = "Refresh";
						s.Text = Model.LabelText(200034);
						s.UseSubmitBehavior = false;
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
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NegativeButton(s =>
					{
						s.Name = "btnDelete";
						s.Text = Model.LabelText(200324);
						s.UseSubmitBehavior = false;
						s.ClientSideEvents.Click = "function(s,e) { Delete(); }";
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
					<input id="LabelID" type="hidden" />
					<input id="LabelType" type="hidden" />
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200331)%>:</td>     <!-- Placeholder ID: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "PlaceholderID";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.ClientEnabled = false;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200024)%>:</td>     <!-- Language: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().ComboBox(s => {
									s.Name = "LanguageID";
									s.Properties.TextField = "LanguageName";
									s.Properties.ValueField = "LanguageID";
									s.Properties.ValueType = typeof(string);
									s.ClientEnabled = false;
								}).BindList(App.Languages).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200332)%>:</td>     <!-- Label Text: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "LabelText";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200333)%>:</td>     <!-- Tool Tip Text: -->
						</tr>
						<tr>        
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "ToolTipText";
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
