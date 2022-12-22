<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ERPIntegrationsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Products/erp-integrations.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "ERPIntegrations", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option tab-button tab-vendors">
				<%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.CausesValidation = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGrdMainWithArgs(true); }";
					}).Render();
				%>
				</td>
				<td class="option tab-button tab-products hide">
				<%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "btnSend";
						settings.Text = Model.LabelText(300081);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { Send(); }";
					}).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

		<%
		    Html.Cortex().PageControl(settings =>
			{
				settings.Name = "tabMain";
				settings.ClientSideEvents.ActiveTabChanged = "function(s,e) { TabChanged(s,e); }";
		
				settings.TabPages.Add(t =>
				{
					t.Text = "Vendors";
					t.SetContent(() =>
					{
					%>
						<table class="grid-container">
							<tr>
								<td class="grid-pane">
									<% Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]); %>
								</td>
								<td class="cell">
									<div id="edit-pane">
										<label class="heading"><%= Model.Label(200325) %></label>       <!-- Edit Details -->
										<input id="VendorID" type="hidden" />
										<table class="fields">
											<tr>
												<td class="label"><%= Model.Label(300080) %>:</td>     <!-- Vendor Name: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "VendorName";
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Width = 280;
														s.ClientEnabled = false;
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="label"><%= Model.Label(300073) %>:</td>     <!-- Magento ID: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "MagentoID";
														s.Width = 280;
														s.Properties.MaxLength = 10;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Properties.ValidationSettings.RegularExpression.ValidationExpression = "^[0-9]*$";
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.EnableClientSideAPI = true;
													}).Render();
												%>
												</td>
											</tr>									
											<tr>
												<td class="field required">
												<%
													Html.Cortex().CheckBox(s =>
													{
														s.Name = "MagentoEnabled";
														s.Text = Model.Label(300074);
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="label"><%= Model.Label(300075) %>:</td>     <!-- NetSuite Internal ID: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "NetSuiteInternalID";
														s.Width = 280;
														s.Properties.MaxLength = 50;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Properties.ValidationSettings.RegularExpression.ValidationExpression = "^[0-9]*$";
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.EnableClientSideAPI = true;
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="label"><%= Model.Label(300076) %>:</td>     <!-- NetSuite Entity ID: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "NetSuiteEntityID";
														s.Width = 280;
														s.Properties.MaxLength = 50;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.EnableClientSideAPI = true;
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="label"><%= Model.Label(300077) %>:</td>     <!-- NetSuite Code: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "NetSuiteCode";
														s.Width = 280;
														s.Properties.MaxLength = 10;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.EnableClientSideAPI = true;
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="label"><%= Model.Label(300078) %>:</td>     <!-- NetSuite Name: -->
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().TextBox(s =>
													{
														s.Name = "NetSuiteName";
														s.Width = 280;
														s.Properties.MaxLength = 200;
														s.Properties.ValidationSettings.RequiredField.IsRequired = true;
														s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
														s.Properties.EnableClientSideAPI = true;
													}).Render();
												%>
												</td>
											</tr>
											<tr>
												<td class="field required">
												<%
													Html.Cortex().CheckBox(s =>
													{
														s.Name = "NetSuiteEnabled";
														s.Text = Model.Label(300079);
													}).Render();
												%>
												</td>
											</tr>
										</table>
										<div class="button-container">
											<div class="button-right">
											<%
												Html.Cortex().NegativeButton(s =>
												{
													s.Name = "btnCancel";
													s.Text = Model.LabelText(200245);      // Cancel
													s.UseSubmitBehavior = false;
													s.CausesValidation = false;
													s.ClientSideEvents.Click = "function (s,e) { Get(); }";
												}).Render();
											%>
											</div>
											<div class="button-right">
											<%
												Html.Cortex().PositiveButton(s =>
												{
													s.Name = "btnSave";
													s.Text = Model.LabelText(200247);      // Save;
													s.UseSubmitBehavior = false;
													s.ClientSideEvents.Click = "function (s,e) { Save(); }";
												}).Render();
											%>
											</div>
											<div class="clear"></div>
										</div>
									</div>
								</td>
							</tr>
						</table>
					<%
					});
				});

				settings.TabPages.Add(t =>
				{
					t.Text = "Products";
					t.ClientEnabled = false;
					t.SetContent(() =>
					{
					%>
						<table class="grid-container">
							<tr>
								<td class="grid-pane">
									<% Html.RenderPartial("GridViewPartial", Model.Grids["GrdProducts"]); %>
								</td>
							</tr>
						</table>
					<%
					});
				});
			
			}).Render();
	    %>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
	<%= Html.Cortex().EndForm() %>
</asp:Content>
