<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<VisitorLogList>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Admin/admin.js?v<%= App.Version %>"></script>
	<script type="text/javascript" src="/Assets/Js/Admin/visitor-log.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Admin", "VisitorLog", "List", Model.PageID)
%></asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200336)%>:</label><!-- Visits from --></td>
				<td class="option"><%
					Html.Cortex().DateEdit(settings => {
						settings.Name = "FromDate";
					}).Bind(Model.FromDate).Render();
				%></td>
				<td class="option"><label><%= Model.Label(200337)%>:</label><!-- Visits to --></td>
				<td class="option"><%
					Html.Cortex().DateEdit(settings =>{
						settings.Name = "ToDate";
					}).Bind(Model.ToDate).Render();
				%></td>
				<td class="option"><%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
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
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server"><%=
	Html.Cortex().EndForm()
%></asp:Content>