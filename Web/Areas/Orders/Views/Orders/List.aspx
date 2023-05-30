<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<OrdersList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/Orders/orders.js?<%= App.Version %>"></script>
    <script src="/Assets/Js/cx.js" type="text/javascript"></script>	
	<link href="/Assets/Css/orders.css?<%= App.Version %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Orders", "Orders", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(300160)%>:</label><!-- Date range --></td>
				<td class="option">
				<%
					Html.Cortex().RadioButtonList(s =>
					{
						s.Name = "DateRange";
						s.SelectedIndex = 0;
						s.Properties.TextField = "Description";
						s.Properties.ValueField = "FromToDateID";
						s.Properties.RepeatDirection = RepeatDirection.Horizontal;
						s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { DateRangeChanged(GrdOrders); }";
					}).BindList(Model.DateRanges).Render();
				%>
				</td>
				<td class="option"><label><%= Model.Label(201026)%>:</label><!-- Orders from --></td>
				<td class="option">
				<%
					Html.Cortex().DateEdit(s => 
					{
						s.Name = "FromDate";
						s.Width = 110;
					}).Bind(Model.FromDate).Render();
				%>
				</td>
				<td class="option"><label><%= Model.Label(201027)%>:</label><!--  Orders to --></td>
				<td class="option">
				<%
					Html.Cortex().DateEdit(s =>
					{
						s.Name = "ToDate";
						s.Width = 110;
					}).Bind(Model.ToDate).Render();
				%>
				</td>
				<td class="option"><label><%= Model.Label(300064)%>:</label><!-- Stores --></td>
				<td class="option">
					<div>                    
					    <% Html.RenderPartial("CheckComboPartial", Model.StoresModel); %>								  					                 
					</div>
				</td>
				<td class="option"><label><%= Model.Label(300162)%>:</label><!-- Order status --></td>
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => 
					{
						s.Name = "OrderStatus";
						s.Properties.ValueField = "SalesOrderLineStatusID";
						s.Properties.TextField = "StatusName";
						s.Width = 125;
						s.SelectedIndex = 0;
					}).BindList(Model.SalesOrderStatusesFilter).Render();
				%>
				</td>
				<td class="option">
				<%
					Html.Cortex().CheckBox(s => 
					{
						s.Name = "FailedEDIDelivery";
						s.Text = Model.LabelText(300144); // Failed EDI delivery?
					}).Bind(Model.FailedEDIDelivery).Render();
				%>
				</td>
				<td class="option">
				<%
					Html.Cortex().CheckBox(s => 
					{
						s.Name = "IncompleteItemLines";
						s.Text = Model.LabelText(300156); // Incomplete items lines
					}).Bind(Model.IncompleteItemLines).Render();
				%>
				</td>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true, s =>
					{
						s.Name = "Refresh";
						s.Text = Model.LabelText(200034);
						s.UseSubmitBehavior = false;
						s.CausesValidation = false;
						s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdOrders); }";
					}).Render();
				%>
				</td>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true, s =>
					{
						s.Name = "btnQueueNetSuiteUpdate";
						s.Text = Model.LabelText(300189); // Queue NetSuite update
						s.ClientEnabled = false;
						s.UseSubmitBehavior = false;
						s.CausesValidation = false;
						s.ClientSideEvents.Click = "function (s,e) { QueueNetSuiteUpdate(); }";
					}).Render();
				%>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<table id="grid-container">
		<tr> 
			<td id="grid-header">
				<%
				Html.Cortex().PageControl(s =>
				{
					s.Name = "tabMain";
					s.ClientSideEvents.ActiveTabChanged = "function(s,e) { ActiveTabChanged(s,e); }";

					s.TabPages.Add(t =>
					{
						t.Text = "Orders";
						s.Width = Unit.Percentage(100);					
						t.SetContent(() =>
						{ %><div class="grid-pane"><% Html.RenderPartial("GridViewPartial", Model.Grids["GrdOrders"]); %></div><% });
					});

					s.TabPages.Add(t =>
					{
						t.Text = "History";
						s.Width = Unit.Percentage(100);					
						t.SetContent(() =>
						{ %><div class="grid-pane"><% Html.RenderPartial("GridViewPartial", Model.Grids["GrdHistory"]); %></div><%});
					});

				}).Render();
				%>
			</td>
		</tr>
	</table>

	<div class="popup-background hide"></div>
	<div id="fraud-check-popup" class="popup-container hide">
		<p class="title"><label><%= Model.Label(300157)%></label><!-- Fraud check --></p>
		<div class="popup-content">
			<table class="popup-table">
				<tr class="hide">
					<td><input id="fraud-check-popup-sales-order"/></td>
				</tr>
				<tr>
					<td class="label" colspan="2">
						<label><%= Model.Label(300158)%>:</label><!-- Payment type -->
					</td>
				</tr>
				<tr>
					<td class="data" colspan="2">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "PaymentMethod";
							s.Width = 200;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label" colspan="2">
						<label><%= Model.Label(300140)%>:</label><!-- Fraud score -->
					</td>
				</tr>
				<tr>
					<td class="data" colspan="2">
					<%
						Html.Cortex().SpinEdit(s =>
						{
							s.Name = "FraudScore";
							s.Width = 200;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="data-label" colspan="2">
					<%
						Html.Cortex().CheckBox(s =>
						{
							s.Name = "CustomerIsNew";
							s.Text = Model.LabelText(300141); // New customer?
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="data-label" colspan="2">
					<%
						Html.Cortex().CheckBox(s =>
						{
							s.Name = "CustomerIPAddressChecked";
							s.Text = Model.LabelText(300142); // IP address checked?
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="data-label" colspan="2">
						<textarea id="fraud-check-popup-ip-address" rows="3" style="width:200px" readonly="readonly"></textarea>
					</td>
				</tr>
				<tr>
					<td class="data-label" colspan="2">
					<%
						Html.Cortex().CheckBox(s =>
						{
							s.Name = "ShippingAddressChecked";
							s.Text = Model.LabelText(300143); // Shipping address checked?
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="data-label" colspan="2">
					<%
						Html.Cortex().CheckBox(s =>
						{
							s.Name = "FraudChecked";
							s.Text = Model.LabelText(300159); // Fraud check completed?
						}).Render();
					%>
					</td>
				</tr>
			</table>
		</div>
		<div id="fraud-check-buttons" class="button-container">
			<div class="button-right">
			<%
				Html.Cortex().NeutralButton(s =>
				{
					s.Name = "btnFraudCheckCancel";
					s.Text = "Cancel";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { HidePopup('#fraud-check-popup'); }";
				}).Render();
			%>
			</div>
			<div class="button-right">
			<%
				Html.Cortex().PositiveButton(s =>
				{
					s.Name = "btnFraudCheckSave";
					s.Text = "Save";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { SaveFraudCheckPopup(); }";
				}).Render();
			%>
			</div>
			<div class="clear"></div>
		</div>
	</div>

	<div class="popup-background hide"></div>
	<div id="shipping-address-popup" class="popup-container hide">
		<p class="title"><label><%= Model.Label(201017)%></label><!-- Shipping address --></p>
		<div class="popup-content">
			<table class="popup-table">
				<tr class="hide">
					<td><input id="shipping-address-popup-sales-order"/></td>
					<td><input id="shipping-address-popup-shipping-address-id"/></td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(300164)%>:</label><!-- First name -->
					</td>
					<td class="label">
						<label><%= Model.Label(300165)%>:</label><!-- Last name -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressFirstName";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressLastName";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(300166)%>:</label><!-- Street 1 -->
					</td>					
					<td class="label">
						<label><%= Model.Label(300167)%>:</label><!-- Street 2 -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressStreet1";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressStreet2";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(200727)%>:</label><!-- City -->
					</td>
					<td class="label">
						<label><%= Model.Label(200021)%>:</label><!-- Region -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressCity";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressRegion";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(300168)%>:</label><!-- Zip -->
					</td>
					<td class="label">
						<label><%= Model.Label(200694)%>:</label><!-- Country -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressZip";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressCountry";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(200022)%>:</label><!-- Email -->
					</td>
					<td class="label">
						<label><%= Model.Label(200672)%>:</label><!-- Phone -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressEmail";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressPhone";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
				<tr>
					<td class="label">
						<label><%= Model.Label(200020)%>:</label><!-- Company -->
					</td>
				</tr>
				<tr>
					<td class="data">
					<%
						Html.Cortex().TextBox(s =>
						{
							s.Name = "ShippingAddressCompany";
							s.Width = 200;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ValidationGroup = "SHIPPING_ADDRESS_EDIT";
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).Render();
					%>
					</td>
				</tr>
			</table>
		</div>
		<div id="shipping-address-buttons" class="button-container">
			<div class="button-right">
			<%
				Html.Cortex().NeutralButton(s =>
				{
					s.Name = "btnShippingAddressCancel";
					s.Text = "Cancel";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { HidePopup('#shipping-address-popup'); }";
				}).Render();
			%>
			</div>
			<div class="button-right">
			<%
				Html.Cortex().PositiveButton(s =>
				{
					s.Name = "btnShippingAddressSave";
					s.Text = "Save";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { SaveShippingAddress(); }";
				}).Render();
			%>
			</div>
			<div class="clear"></div>
		</div>
	</div>

	<div class="popup-background hide"></div>
	<div id="push-to-supplier-popup" class="popup-container hide">
		<p class="title"><label><%= Model.Label(300161)%></label><!-- Push to supplier --></p>
		<div class="popup-content">
			<table class="popup-table">
				<tr class="hide">
					<td><input id="push-to-supplier-popup-sales-order"/></td>
					<td><input id="push-to-supplier-popup-sales-order-line"/></td>
				</tr>
				<tr>
					<td class="data-label" id="push-to-supplier-message"></td>
				</tr>
			</table>
		</div>
		<div id="push-to-supplier-buttons" class="button-container">
			<div class="button-right">
			<%
				Html.Cortex().NeutralButton(s =>
				{
					s.Name = "btnPushToSupplierCancel";
					s.Text = "Cancel";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { HidePopup('#push-to-supplier-popup'); }";
				}).Render();
			%>
			</div>
			<div class="button-right">
			<%
				Html.Cortex().PositiveButton(s =>
				{
					s.Name = "btnPushToSupplier";
					s.Text = "Push to supplier";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { PushToSupplier(); }";
				}).Render();
			%>
			</div>
			<div class="clear"></div>
		</div>
	</div>

	<div class="popup-background hide"></div>
	<div id="error-message-popup" class="popup-container hide">
		<p class="title"><label><%= Model.Label(300174)%></label><!-- Error --></p>
		<div class="popup-content">
			<table class="popup-table">
				<tr>
					<td class="data-label" id="error-message-popup-message"></td>
				</tr>
			</table>
		</div>
		<div id="error-message-buttons" class="button-container">
			<div class="button-right">
			<%
				Html.Cortex().NeutralButton(s =>
				{
					s.Name = "btnErrorMessageClose";
					s.Text = "Close";
					s.CausesValidation = false;
					s.UseSubmitBehavior = false;
					s.ClientSideEvents.Click = "function(s,e) { HidePopup('#error-message-popup'); }";
				}).Render();
			%>
			</div>
			<div class="clear"></div>
		</div>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
	<%= Html.Cortex().EndForm() %>
</asp:Content>