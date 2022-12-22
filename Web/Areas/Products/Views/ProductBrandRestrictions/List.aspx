<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ProductBrandRestrictionsList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/Products/product-brand-restrictions.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "ProductBrandRestrictions", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200618)%>:</label></td><!-- Brand -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "BrandKey";
						s.Properties.ValueField = "BrandKey";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "BrandName";
						s.Width = Unit.Pixel(200);
						s.SelectedIndex = 0;
					}).BindList(Model.Brands).Render(); 
				%></td>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true, s =>
					{
						s.Name = "Refresh";
						s.Text = Model.LabelText(200034);
						s.UseSubmitBehavior = false;
						s.CausesValidation = false;
						s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain, true); }";	// true = ensure we go to DB
					}).Render();
				%></td>					
				<td class="label"><%= Model.Label(300104)%>:</td>		<!-- Catalog -->
				<td class="field">
					<div style="padding-top: 5px;">
					<%
						Html.Cortex().ComboBox(s =>
						{
							s.Name = "CatalogKey";
							s.Properties.TextField = "Catalog";
							s.Properties.ValueField = "CatalogID";
							s.Properties.ValueType = typeof(int);
							s.Width = 500;
							s.Properties.AllowNull = false;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).BindList(Model.Catalogs).Render();
					%>
				    </div>
				</td>								
				<td class="option">
                    <div style="padding-left: 15px;">
					<%
						Html.Cortex().AssignButton(true, s =>
						{
							s.Name = "Assign";
							s.Text = Model.LabelText(200997);
							s.UseSubmitBehavior = false;					
							s.ClientSideEvents.Click = "function (s,e) { AssignCatalog(); }";
						}).Render();
					%>
					</div>
				</td>
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
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>