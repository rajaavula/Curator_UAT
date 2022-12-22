<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<CompanyEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/companies.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Companies", "Edit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><%
					Html.Cortex().NeutralButton(settings =>
					{
						settings.Name = "btnReload";
						settings.Text = Model.LabelText(200246);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { Reload('" + Model.ReloadUrl  + "'); }";
					}).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "btnSave";
						settings.Text = Model.LabelText(200247);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { Save(); }";
					}).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="overflow-y">   
	<table>
		<tr>
			<td>
				<input id="txtCompanyID" type="hidden" name="CompanyID" value="<%= Model.CompanyID %>" /><%
				if (Model.Exception != null) { %><div class="error"><%= Model.Exception.Message %></div><% }
				if (Model.Success) { %><div class="success"><%= Model.Label(200463)%></div><% } // Company updated successfully
				%><div class="group expanded">
					<a class="heading" href="#"><%= Model.Label(200069)%></a> <!-- General -->
					<div class="content">
						<table class="edit-section">
							<tr>
								<td class="label"><%= Model.Label(200446)%>:</td> <!-- Company Name: -->
								<td class="field"><%
									Html.Cortex().TextBox(s => {
										s.Name = "CompanyName";
										s.Width = 180;
										s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
										s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									}).Bind(Model.CompanyName).Render();
								%></td>
								<td></td>
								<td></td>
							</tr>
							<tr>
								<td class="label"><%= Model.Label(200076)%>:</td> <!-- Notes: -->
								<td class="field" colspan="3"><%
									Html.Cortex().Memo(s => 
									{
										s.Name = "Notes";
										s.Height = 100;
										s.Width = 400;
									}).Bind(Model.Notes).Render();
								%></td>
							</tr>
							<tr>
								<td class="label"></td>
								<td class="field"><%
									Html.Cortex().CheckBox(s => {
										s.Name = "Live";
										s.Text = Model.LabelText(200447); // Company is live?
									}).Bind(Model.Live).Render();
								%></td>
								<td class="field" colspan="2"></td>
							</tr>
							<tr>
								<td class="label"><%= Model.Label(200452)%>:</td> <!-- Company Logo: -->
								<td colspan="2"><% Html.Cortex().UploadControl(Model.CompanyLogoUploadControlSettings).Render(); %></td>
								<td>
								<%
									Html.Cortex().PositiveButton(s => 
									{
										s.Name = "btnUpload";
										s.Text = Model.LabelText(200414); // Upload
										s.UseSubmitBehavior = false;
										s.ClientSideEvents.Click = "function(s,e) { uplCompanyLogo.Upload(); }";
									}).Render();
								%>
								</td>
							</tr>
							<tr>
								<td></td>
								<td colspan="3">
									<img id="logo-preview" src="/Home/Home/Logo/<%= Model.CompanyID %>" alt="" />
								</td>
							</tr>
						</table>
					</div>
				</div>
				<div class="group expanded">
					<a class="heading" href="#"><%= Model.Label(200474)%></a> <!-- Licensing -->
					<div class="content">
						<table class="edit-section">
							<tr>
								<td class="label"><%= Model.Label(200475)%>:</td> <!-- Maximum sessions -->
								<td class="field">
								<%
									Html.Cortex().SpinEdit(s => 
									{
										s.Name = "MaximumSessions";
										s.Width = 80;
										s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
										s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									}).Bind(Model.MaximumSessions).Render();
								%>
								</td>
								<td class="field">
								<%
									Html.Cortex().CheckBox(s => 
									{
										s.Name = "RestrictSessions";
										s.Text = Model.LabelText(200476);  // Restrict sessions?
									}).Bind(Model.RestrictSessions).Render();
								%>
								</td>
								<td></td>
							</tr>
						</table>
					</div>
				</div>
				<div class="group expanded">
					<a class="heading" href="#"><%= Model.Label(200458)%></a> <!-- Email Recipients -->
					<div class="content">
						<table class="edit-section">
							<tr>
								<td class="label"><%= Model.Label(200459)%>:</td>  <!-- Security Admin Email: -->
								<td class="field">
								<%
									Html.Cortex().TextBox(s => 
									{
										s.Name = "SecurityAdminEmail";
										s.Width = 180;
										s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
										s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
									}).Bind(Model.SecurityAdminEmail).Render();
								%>
								</td>
							</tr>
							<tr>
								<td class="label"><%= Model.Label(200460)%>:</td>  <!-- Copy Emails To: -->
								<td class="field">
								<%
									Html.Cortex().TextBox(s => 
									{
										s.Name = "CopyEmailTo";
										s.Width = 400;
										s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
										s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailListRegex;
									}).Bind(Model.CopyEmailTo).Render();
								%>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</td>
		</tr>
	</table>
</div> 
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>