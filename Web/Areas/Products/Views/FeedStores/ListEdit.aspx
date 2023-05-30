<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<FeedStoresListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/FeedStores/feedstores.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "FeedStores", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true, s =>
					{
						s.Name = "Refresh";
						s.Text = Model.LabelText(200034);
						s.UseSubmitBehavior = false;
						s.CausesValidation = false;
						s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain, true); }";  // true = ensure we go to DB
					}).Render();
				%></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <table id="grid-container">
		<tr>
			<td id="grid-pane"><%
				Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);
			%></td>
			<td class="cell">
				<div id="edit-pane">
					<label class="heading"><%= Model.Label(200325)%></label><!-- Edit details -->
					<input id="FeedStoreID" type="hidden" />
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200966)%>:</td>  <!-- Feed -->    
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "FeedName";
									s.Width = Unit.Pixel(300);
									s.ClientEnabled = false;
								}).Render();
							%>
							</td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300071)%>:</td>  <!-- Store -->    
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "StoreName";
									s.Width = Unit.Pixel(300);
									s.ClientEnabled = false;
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials email-supplier d-none">
							<td class="label"><%= Model.Label(300176)%>:</td>  <!-- Push to supplier email -->    
						</tr>
						<tr class="credentials email-supplier d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "PushToSupplierEmail";
									s.Width = Unit.Pixel(300);
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailListRegex;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'EMAIL') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials edi-supplier d-none">
							<td class="label"><%= Model.Label(300181)%>:</td>  <!-- EDI Credentials Provided -->
						</tr>
						<tr class="credentials edi-supplier d-none">
							<td class="field">
							<%
								Html.Cortex().CheckBox(s =>
								{
									s.Name = "CredentialsProvided";
									s.ClientEnabled = false;
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials ingram d-none">
							<td class="label"><%= Model.Label(300190)%>:</td>  <!-- Client ID -->
						</tr>
						<tr class="credentials ingram d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "ClientID";
									s.Width = Unit.Pixel(300);
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'INGRAM') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials ingram d-none">
							<td class="label"><%= Model.Label(300191)%>:</td>  <!-- Client Secret -->
						</tr>
						<tr class="credentials ingram d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "ClientSecret";
									s.Width = Unit.Pixel(300);
									s.Properties.Password = true;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'INGRAM') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials ingram d-none">
							<td class="label"><%= Model.Label(300192)%>:</td>  <!-- Customer Number -->
						</tr>
						<tr class="credentials ingram d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "CustomerNumber";
									s.Width = Unit.Pixel(300);
									s.Properties.MaxLength = 10;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'INGRAM') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials ingram d-none">
							<td class="label"><%= Model.Label(300193)%>:</td>  <!-- Country Code -->
						</tr>
						<tr class="credentials ingram d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "CountryCode";
									s.Width = Unit.Pixel(300);
									s.Properties.MaxLength = 2;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'INGRAM') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials synnex d-none">
							<td class="label"><%= Model.Label(300194)%>:</td>  <!-- Username -->
						</tr>
						<tr class="credentials synnex d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "Username";
									s.Width = Unit.Pixel(300);
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'SYNNEX') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials synnex d-none">
							<td class="label"><%= Model.Label(300195)%>:</td>  <!-- Password -->
						</tr>
						<tr class="credentials synnex d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "Password";
									s.Width = Unit.Pixel(300);
									s.Properties.Password = true;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'SYNNEX') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials synnex d-none">
							<td class="label"><%= Model.Label(300196)%>:</td>  <!-- Source Identifier -->
						</tr>
						<tr class="credentials synnex d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "SourceIdentifier";
									s.Width = Unit.Pixel(300);
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'SYNNEX') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials synnex d-none">
							<td class="label"><%= Model.Label(300197)%>:</td>  <!-- Consumer Key -->
						</tr>
						<tr class="credentials synnex d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "ConsumerKey";
									s.Width = Unit.Pixel(300);
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'SYNNEX') }";
								}).Render();
							%>
							</td>
						</tr>
						<tr class="credentials synnex d-none">
							<td class="label"><%= Model.Label(300198)%>:</td>  <!-- Consumer Secret -->
						</tr>
						<tr class="credentials synnex d-none">
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "ConsumerSecret";
									s.Width = Unit.Pixel(300);
									s.Properties.Password = true;
									s.Properties.ValidationSettings.RequiredField.IsRequired = true;
									s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
									s.Properties.ClientSideEvents.Validation = "function (s, e) { Validate(s, e, 'SYNNEX') }";
								}).Render();
							%>
							</td>
						</tr>
					</table>
					<div class="button-container">
						<div class="button-right">
						<%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);
								s.UseSubmitBehavior = false;
								s.CausesValidation = false;
								s.ClientSideEvents.Click = "function (s,e) { SaveFeedStore(); }";
							}).Render();
						%></div>
						<div class="button-right enter-credentials">
						<%
							Html.Cortex().NeutralButton(s =>
							{
								s.Name = "btnCredentials";
								s.Text = Model.LabelText(300199); // Enter credentials
								s.UseSubmitBehavior = false;
								s.CausesValidation = false;
								s.ClientSideEvents.Click = "function (s,e) { Credentials(); }";
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