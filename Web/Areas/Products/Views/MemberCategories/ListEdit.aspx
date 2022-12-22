<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<MemberCategories>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/MemberCategories/membercategories.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content  ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "MemberCategories", "ListEdit", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(300071)%>:</label></td><!-- Store -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => 
					{
						s.Name = "StoreID";
						s.Properties.ValueField = "StoreID";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "StoreName";
						s.Width = Unit.Pixel(150);	
						s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
					}).BindList(Model.MemberStoreList).Bind(Model.StoreID).Render();
				%>
				</td>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true, settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
					}).Render();
				%>
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
		<td class="cell">
			<div id="grid-pane">
				<%Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);%>	
			</div>
		</td>			
	</tr>							 
</table>	
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>
