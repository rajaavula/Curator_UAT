<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<UsersEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/selectize.min.js"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/users.js?v<%= App.Version %>"></script>
	<link href="/Assets/Css/selectize.bootstrap2.css" rel="stylesheet" type="text/css" />
	<link href="/Assets/Css/selectize.css" rel="stylesheet" type="text/css" />
	<link href="/Assets/Css/selectize.default.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Users", "Edit", Model.PageID)
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
						settings.CausesValidation = false;
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
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "btnNew";
						settings.Text = Model.LabelText(200036);
						settings.UseSubmitBehavior = false;
						settings.CausesValidation = false;
						settings.ClientSideEvents.Click = "function(s,e) { New(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="overflow-y">
	<table>
		<tr>
			<td class="cell">
				<div>
					<input id="txtUserID" type="hidden" name="UserID" value="<%= Model.UserID %>" />
					<input id="txtMemberStoreName" type="hidden" name="MemberStores" value="<%= Model.StoreName %>" />
					<%
						if (Model.Exception != null) { %><div class="error"><%= Model.Exception.Message %></div><% }
						if (Model.Success) { %><div class="success"><%= Model.Label(200436)%></div><% } // User updated successfully
					%>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(200069)%></a>  <!-- General -->
						<div class="content">
							<table class="edit-section">
								<tr>
									<td class="label"><%= Model.Label(200016)%>:</td>  <!-- Name -->
									<td class="field">
									<%
										Html.Cortex().TextBox(s => {
											s.Name = "FullName";
											s.Width = 180;
											s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
											s.Properties.ValidationSettings.RequiredField.IsRequired = true;
										}).Bind(Model.FullName).Render();
									%>
									</td>
									<td class="label"><%= Model.Label(200198)%>:</td>  <!-- Position -->
									<td class="field">
									<%
										Html.Cortex().TextBox(s => {
											s.Name = "Position";
											s.Width = 180;
										}).Bind(Model.Position).Render();
									%>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(200394)%></a>  <!-- Account -->
						<div class="content">
							<table class="edit-section">	
								<tr>
									<td class="label"><%= Model.Label(200022)%>:</td>  <!-- Email -->
									<td class="field" colspan="3">
									<%
										Html.Cortex().TextBox(s => {
											s.Name = "Email";
											s.Width = 400;
											s.Properties.ValidationSettings.RequiredField.IsRequired = true;
											s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
											s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
										}).Bind(Model.Email).Render();
									%>
									</td>
								</tr>
								<tr>
									<td class="label"><%= Model.Label(200202)%>:</td>  <!-- User group -->
									<td class="field">
									<%
										Html.Cortex().ComboBox(s => {
											s.Name = "UserGroupID";
											s.Properties.ValueType = typeof (int);
											s.Properties.ValueField = "UserGroupID";
											s.Properties.TextField = "Name";
											s.Width = 180;
										}).BindList(Model.UserGroups).Bind(Model.UserGroupID).Render();
									%>
									</td>
									<td class="label"><%= Model.Label(200024)%>:</td>  <!-- Language -->
									<td class="field">
									<%
										Html.Cortex().ComboBox(s => {
											s.Name = "LanguageID";
											s.Properties.ValueType = typeof (string);
											s.Properties.ValueField = "LanguageID";
											s.Properties.TextField = "LanguageName";
											s.Properties.ValidationSettings.RequiredField.IsRequired = true;
											s.Width = 180;
										}).BindList(Model.Languages).Bind(Model.LanguageID).Render();
									%>
									</td>
								</tr>
								<tr>
									<td class="label"></td>
									<td class="field">
									<%
										Html.Cortex().CheckBox(s => {
											s.Name = "SalesRep";
											s.Text = Model.LabelText(200660); // Sales rep?
										}).Bind(Model.SalesRep).Render();
									%>
									</td>
									<td class="label"></td>
									<td class="field"></td>
								</tr>
								<tr>
									<td class="label"></td>
									<td class="field">
									<%
										Html.Cortex().CheckBox(s => {
											s.Name = "Enabled";
											s.Text = Model.LabelText(200398); // Enabled?
										}).Bind(Model.Enabled).Render();
									%>
									</td>
									<td class="label"></td>
									<td class="field"></td>
								</tr>
							</table>
						</div>
					</div>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(200395)%></a>  <!-- Contact Details -->
						<div class="content">
							<table class="edit-section">
								<tr>
									<td class="label"><%= Model.Label(200105)%>:</td>  <!-- Mobile -->
									<td class="field">
									<%
										Html.Cortex().TextBox(s => {
											s.Name = "Mobile";
											s.Width = 180;
										}).Bind(Model.Mobile).Render();
									%>
									</td>
									<td class="label"><%= Model.Label(200102)%>:</td>  <!-- Telephone -->
									<td class="field">
									<%
										Html.Cortex().TextBox(s => {
											s.Name = "Telephone";
											s.Width = 180;
										}).Bind(Model.Telephone).Render();
									%>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(300096)%></a>  <!-- Notifications -->
						<div class="content">
                            <table class="edit-section">
								<tr>
									<td class="field">
									<%
										Html.Cortex().CheckBox(s => {
											s.Name = "NewProductNotifications";
											s.Text = Model.Label(300093);
											s.Width = 220;
										}).Bind(Model.NewProductNotifications).Render();
									%>
									</td>
									<td class="field">
									<%
										Html.Cortex().CheckBox(s => {
										s.Name = "ChangedProductNotifications";
										s.Text = Model.Label(300094);
										s.Width = 220;
									}).Bind(Model.ChangedProductNotifications).Render();
									%>
									</td>
								</tr>
								<tr>
									<td class="field">
									<%
										Html.Cortex().CheckBox(s => {
											s.Name = "DeactivatedProductNotifications";
											s.Text = Model.Label(300095);
											s.Width = 220;
										}).Bind(Model.DeactivatedProductNotifications).Render();
									%>
									</td>
									<td class="field"></td>
								</tr>
							</table>
						</div>
					</div>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(300070)%></a>  <!-- Shopify stores -->
						<div class="content">
                            <table class="edit-section">
								<tr>
									<td class="label"><%= Model.Label(300070)%>:</td>  <!-- Shopify stores -->
									<td class="field" style="width: 20px;">
                                    <input id="MemberStoreIDList" type="hidden" name="MemberStoreIDList" value="<%=Model.MemberStoreIDList %>" />
                                    <select multiple class="member-store-list" style="width: 520px; background: #fd7e14;" id='<%=string.Format("member-store-list{0}", Model.AvailableMemberStores)%>'>
                                        <%  
											foreach (var stores in Model.AvailableMemberStores) 
											{ 
											%>						
										       <option value="<%= stores.StoreID %>" <%= Model.MemberStores.Where(x => x.MemberStores == stores.StoreName) .FirstOrDefault() != null ? "selected" : "" %> > <%= stores.StoreName %></option>
											<% 
											}
										%>
                                    </select>
                                </td>
								</tr>
                            </table>
						</div>
					</div>
					<div class="group expanded">
						<a class="heading" href="#"><%= Model.Label(200396)%></a>  <!-- Regional Access -->
						<div class="content">
							<table class="edit-section">
								<tr>
									<td class="label vtop"><%= Model.Label(200400)%>:</td>  <!-- Access to regions -->
									<td><% Html.Cortex().CheckBoxList(Model.RegionCheckBoxListSettings).Render(); %></td>
								</tr>
							</table>
						</div>
					</div>
				</div>
			</td>
		</tr>
	</table>
</div>
	<%=
	Html.Cortex().EndForm()
	%>
</asp:Content>
