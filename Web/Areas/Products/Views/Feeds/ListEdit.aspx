<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<FeedsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/Feeds/feeds.js?<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server"><%=
	Html.Cortex().BeginForm("Products", "Feeds", "ListEdit", Model.PageID)
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
					<input id="FeedID" type="hidden" /> 
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
									s.ReadOnly = true;
								}).Render();
							%>
							</td>
						</tr>
						<tr>                                            
							<td class="field">
							<%
								Html.Cortex().CheckBox(s =>
								{
									s.Name = "IncludeZeroStock";
									s.Text = Model.Label(300110);
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
								s.ClientSideEvents.Click = "function (s,e) { SaveSupplier(); }";
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