<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<CategoryMappingsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/CategoryMappings/category-mappings.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "CategoryMappings", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200966)%>:</label></td><!-- Feed -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s =>
					{
						s.Name = "FeedID";
						s.Width = Unit.Pixel(200);
						s.Properties.ValueField = "Value";
						s.Properties.ValueType = typeof(int?);
						s.Properties.TextField = "Description";
					}).BindList(Model.Feeds).Bind(Model.FeedID).Render(); 
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
						s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";	// true = ensure we go to DB
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
					<label class="heading"><%= Model.Label(200325)%></label> <!-- Edit details -->
					<input id="CategoryMappingID" type="hidden" /> 			
					<table class="fields">
						<tr>
							<td class="label"><%= Model.Label(200998)%>:</td> <!-- LE category -->
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().ComboBox(s =>
								{
									s.Name = "CategoryID";
									s.Width = Unit.Pixel(300);
									s.Properties.ValueField = "Value";
									s.Properties.ValueType = typeof(int?);
									s.Properties.TextField = "Description";
								}).BindList(Model.Categories).Render();
							%>
							</td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(200618)%>:</td>  <!-- Brand -->
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "ManufacturerName";
									s.Width = Unit.Pixel(300);
									s.ReadOnly = true;
								}).Render();
							%>
							</td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300111)%>:</td>  <!-- Feed category 1 -->
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "Category1";
									s.Width = Unit.Pixel(300);
									s.ReadOnly = true;
								}).Render();
							%>
							</td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300112)%>:</td>  <!-- Feed category 2 -->
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "Category2";
									s.Width = Unit.Pixel(300);
									s.ReadOnly = true;
								}).Render();
							%>
							</td>
						</tr>
						<tr>
							<td class="label"><%= Model.Label(300113)%>:</td>  <!-- Feed category 3 -->
						</tr>
						<tr>
							<td class="field">
							<%
								Html.Cortex().TextBox(s =>
								{
									s.Name = "Category3";
									s.Width = Unit.Pixel(300);
									s.ReadOnly = true;
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
								s.Text = Model.LabelText(200245); // Cancel
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
								s.Text = Model.LabelText(200247); // Save;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Check(); }";
							}).Render();
						%>
						</div>
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
<asp:Content ContentPlaceHolderID="PopupContent" runat="server">
	<% Html.Cortex().PopupControl(Model.ConfirmPopupSettings).Render(); %>
</asp:Content>