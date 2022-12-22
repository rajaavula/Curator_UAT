<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<TradeServiceProducts>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/TradeServiceProducts/tradeserviceproducts.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server">
	<%= Html.Cortex().BeginForm("Products", "TradeServiceProducts", "ListEdit", Model.PageID) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
			   <td class="label"><%= Model.Label(200649)%>:</td>  <!-- Category -->                           
                
				<td class="option">
					<%					 
                        Html.Cortex().ComboBox(s =>
                        {
                            s.Name = "CategoryKey";
                            s.Properties.TextField = "Description";
                            s.Properties.ValueField = "Value";
                            s.Properties.ValueType = typeof(int);
                            s.Width = 320;
                            s.Properties.AllowNull = true;
							//s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeCategory(); }";
                        }).BindList(Model.Categories).Render();
                     %>
				</td>
                                      	
				<td class="option">
					<div style="padding-left: 25px;">
					<%
						Html.Cortex().NeutralButton(true, s =>
						{
							s.Name = "Refresh";
							s.Text = Model.LabelText(200034);
							s.UseSubmitBehavior = false;
							s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
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
            <td id="grid-pane">
                <% Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]); %>
            </td>
          
        </tr>
    </table>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
	<%= Html.Cortex().EndForm() %>
</asp:Content>
