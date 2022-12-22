<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ProductDashboard>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/ProductDashboard/product-dashboard.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server">
    <%= Html.Cortex().BeginForm("Products", "ProductDashboard", "ProductDashboard", Model.PageID) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="title-pane">
        <h2 class="page-title"><%= Model.TranslatedName %></h2>
    </div>
    <table>
        <tr>
            <td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { RefreshSupplierUpdates(); }";
					}).Render();
				%></td>
        </tr>
    </table>
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

<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
    <%=	Html.Cortex().EndForm()%>
</asp:Content>
