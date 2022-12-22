<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<FormEditor>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/form-editor.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Form", "FormEditor", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option">
					<%
                        Html.Cortex().NeutralButton(true,settings =>
                        {
                            settings.Name = "Refresh";
                            settings.Text = Model.LabelText(200034);
                            settings.UseSubmitBehavior = false;
                            settings.ClientSideEvents.Click = "function (s,e) { RefreshGrid(GrdMain); }";
                        }).Render();
					%>
				</td>
					
				<%if (SI.UserGroup.IsOwner)
				{
					%><td class="option"><%
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
					%></td><%
				}
			%></tr>
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
					<label class="heading"><%= Model.Label(200325)%></label>			<!-- Edit Details -->
					<input id="FormID" type="hidden" />
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200430)%>:</td>			<!-- Group: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormGroup";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200016)%>:</td>			<!-- Name: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormName";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200429)%>:</td>			<!-- Area: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormArea";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200486)%>:</td>			<!-- Controller: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormController";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200487)%>:</td>			<!-- Action: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormAction";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200488)%>:</td>			<!-- Parameters: -->
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormParameters";
									if (!SI.UserGroup.IsOwner)
									{
										s.ReadOnly = true;
										s.Properties.Style.ForeColor = Color.Gray;
									}
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200331)%>:</td>			<!-- Placeholder ID: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormPlaceholderID";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200186)%>:</td>			<!-- Display Order: -->
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "FormDisplayOrder";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = @"^-?\d+$";
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200076)%>:</td>			<!-- Notes: -->
						</tr>
						<tr>
							<td class="field"><%
								Html.Cortex().Memo(s => 
								{
									s.Name = "FormNotes";
									s.Height = 100;
									s.Width = Unit.Percentage(100);
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="field required"><%
								Html.Cortex().CheckBox(s => {
									s.Name = "FormOwnerOnly";
									s.Text = Model.Label(200431);
								}).Render();
							%></td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right"><%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);						// Cancel
								s.CausesValidation = false;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Get(); }";
							}).Render();
						%></div>
						<div class="button-right"><%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);						// Save;
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