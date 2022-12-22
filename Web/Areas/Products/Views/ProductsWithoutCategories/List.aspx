<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ProductsWithoutCategoriesList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/Products/products-without-categories.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "ProductsWithoutCategories", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200966)%>:</label></td><!-- Feed -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "FeedKey";
						s.Properties.ValueField = "FeedKey";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "FeedName";
						s.Width = Unit.Pixel(200);
						s.SelectedIndex = 0;
					}).BindList(Model.Feeds).Render(); 
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
				<td class="label"><%= Model.Label(200998)%>:</td>		<!-- LE category -->
				<td class="field">
					<div style="padding-top: 5px;">
					<%
						Html.Cortex().ComboBox(s => 
						{
							s.Name = "CategoryKey";
							s.Properties.TextField = "CategoryPath";
							s.Properties.ValueField = "CategoryKey";
							s.Properties.ValueType = typeof(int);
							s.Width = 500;
							s.Properties.AllowNull = false;
							s.Properties.ValidationSettings.RequiredField.IsRequired = true;
							s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
						}).BindList(Model.Categories).Render();
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
							s.ClientSideEvents.Click = "function (s,e) { AssignCategory(); }";
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
