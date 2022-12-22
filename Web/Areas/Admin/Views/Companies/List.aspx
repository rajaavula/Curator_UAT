<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<CompanyList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/companies.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "Companies", "List", Model.PageID)
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
					Html.Cortex().NeutralButton(settings =>
					{
						settings.Name = "btnEdit";
						settings.Text = Model.LabelText(200037);
						settings.UseSubmitBehavior = false;
						settings.ClientEnabled = false;
						settings.ClientSideEvents.Click = "function(s,e) { Edit(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().PositiveButton(settings =>
					{
						settings.Name = "btnNew";
						settings.Text = Model.LabelText(200036);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function(s,e) { New(); }";
					},!Model.HasPermission("RESTRICTCLIENTS")).Render();
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
	<% Html.Cortex().PopupControl(Model.NewCompanyPopupSettings).Render(); %>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>
