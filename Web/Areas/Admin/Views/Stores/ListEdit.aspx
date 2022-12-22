<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<StoreListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/stores.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Stores", "ListEdit", Model.PageID)
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
			<td id="grid-pane">
			<%
				Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);
			%>
			</td>
			<td class="cell">
				<div id="edit-pane">
					<label class="heading"><%= Model.Label(200325)%></label>       <!-- Edit Details -->
					<input id="StoreID" type="hidden" />
					
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(201025)%>:</td>     <!-- Store Name: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "StoreName";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(201005)%>:</td>     <!-- Shopify ID: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "ShopifyID";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
								    s.ControlStyle.HorizontalAlign = HorizontalAlign.Left;									
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = "^[0-9]*$";
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300067)%>:</td>     <!-- Store URL: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "StoreUrl";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.URLRegex;
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300066)%>:</td>     <!-- Store API key: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "StoreApiKey";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300068)%>:</td>     <!-- Store password: -->
						</tr>
						<tr>        
							<td colspan="2" class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "StorePassword";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300069)%>:</td>     <!-- Store shared secret: -->
						</tr>
						<tr>        
							<td class="field required"><%
								Html.Cortex().TextBox(s => {
									s.Name = "StoreSharedSecret";
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Width = 280;
								}).Render();
							%></td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300175)%>:</td> <!-- Store Logo: -->
						</tr>
						<tr>
							<td class="field required"><% Html.Cortex().UploadControl(Model.StoreLogoUploadControlSettings).Render(); %></td>
						</tr>
						<tr>
							<td class="field required">
								<img id="logo-preview" src="" alt="" />
							</td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right"><%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);	// Cancel
								s.UseSubmitBehavior = false;
								s.CausesValidation = false;
								s.Width = 80;
								s.ClientSideEvents.Click = "function (s,e) { Get(); }";
							}).Render();
						%></div>
						<div class="button-right"><%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);	// Save;
								s.UseSubmitBehavior = false;
								s.Width = 80;
								s.ClientSideEvents.Click = "function (s,e) { Save(); }";
							}).Render();
						%></div>
						<div class="button-right"><%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnUpload";
								s.Text = Model.LabelText(200414);	// Upload
								s.UseSubmitBehavior = false;
								s.Width = 80;
								s.ClientSideEvents.Click = "function(s,e) { uplStoreLogo.Upload(); }";
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
