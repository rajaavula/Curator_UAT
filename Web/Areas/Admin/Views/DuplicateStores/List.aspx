<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<DuplicateStoresList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/duplicate-stores.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "DuplicateStores", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(300129)%>:</label></td><!-- Source store -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "StoreID";
						s.Properties.ValueField = "StoreID";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "StoreName";
						s.Width = Unit.Pixel(150);
					}).BindList(Model.Stores).Render();
				%>
				</td>
				<td class="option"><%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
					}).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Duplicate";
						settings.Text = Model.LabelText(300206);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { DuplicateStores(); }";
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
		</tr>
	</table>

	<div class="popup-background hide"></div>
	<div id="error-message-popup" class="popup-container hide">
		<p class="title"><label><%= Model.Label(200563)%></label><!-- Status --></p>
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
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>