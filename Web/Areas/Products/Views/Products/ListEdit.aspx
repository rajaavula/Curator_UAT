<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<ProductsListEdit>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/Products/products.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server">
	<%= Html.Cortex().BeginForm("Products", "Products", "ListEdit", Model.PageID) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
				<td class="option"><label><%= Model.Label(200966)%>:</label></td>	<!-- Feed -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "FeedKey";
						s.Properties.ValueField = "FeedKey";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "FeedName";
						s.Width = Unit.Pixel(200);
                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeFeed(); }";
					}).BindList(Model.Feeds).Bind(Model.FeedKey).Render();
				%></td>
				<td class="option"><label><%= Model.Label(200618)%>:</label></td>	<!-- Brand -->
				<td class="option">
				<% Html.RenderPartial("ComboBoxPartial", Model.BrandModel); %>
				</td>
				<td class="option">
				<%
					Html.Cortex().NeutralButton(true,settings =>
					{
						settings.Name = "Refresh";
						settings.Text = Model.LabelText(200034);
						settings.UseSubmitBehavior = false;
						settings.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); }";
					}).Render();
				%>
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
			<td class="cell">
				<div id="edit-pane">
					<label class="heading"><%= Model.Label(200325)%></label>       <!-- Edit details -->					
                    <input id="ProductID" type="hidden" /> 			
					<table class="fields">						
						<tr>
                            <td colspan="2" class="label"><%= Model.Label(200998)%>:</td>  <!-- LE category -->                           
						</tr>
						<tr>
                            <td colspan="2" class="field required">
                            <%
                                Html.Cortex().ComboBox(s =>
                                {
                                    s.Name = "CategoryKey";
                                    s.Properties.TextField = "CategoryPath";
                                    s.Properties.ValueField = "CategoryKey";
                                    s.Properties.ValueType = typeof(int);
                                    s.Properties.AllowNull = true;
                                }).BindList(Model.Categories).Render();
                            %>                   
                            </td>
                        </tr>						
						<tr>
							<td class="label"><%= Model.Label(200978)%>:</td> <!-- Buy price: -->							
							<td class="label"><%= Model.Label(200993)%>:</td> <!-- RRP: -->								
						</tr>
						<tr>
							<td class="field required">
							<%
                                Html.Cortex().SpinEdit(s => {
                                    s.Name = "BuyPrice";
                                    s.Width = 130;
									s.Properties.DecimalPlaces = 2;
                                }).Render();
							%>							
							</td>
							<td class="field required">
							<%
								Html.Cortex().SpinEdit(s => { 
									s.Name = "RecommendedRetailPrice";	
									s.Width = 130;	
                                    s.Properties.DecimalPlaces = 2;
								}).Render();
							%>
							</td>							
					   </tr>
						<tr>
							<td class="label"><%= Model.Label(200979)%>:</td> <!-- MRRP: -->	
							<td class="label"><%= Model.Label(200980)%>:</td> <!-- Markup: -->		
						</tr>
						<tr>
							<td class="field required">
							<%
							Html.Cortex().SpinEdit(s => { 
									s.Name = "MemberRecommendedRetailPrice";
                                    s.Width = 130;	
									s.Properties.DecimalPlaces = 2;
								}).Render();
							%>
							</td>
							<td class="field required">
							<%
                                Html.Cortex().SpinEdit(s => {
                                    s.Name = "Markup";
                                    s.Width = 130;
									s.Properties.DecimalPlaces = 2;
                                }).Render();
							%>
							</td>
						</tr>
						<tr>
							<td colspan="2" class="label"><%= Model.Label(200981)%>:</td> <!-- SOH: -->		
						</tr>
						<tr>
							<td colspan="2" class="field required">
							<%
								Html.Cortex().TextBox(s => { 
									s.Name = "StockOnHand";	
									s.Width = 130;
									s.ControlStyle.HorizontalAlign = HorizontalAlign.Right;
								}).Render();
							%>
							</td>
					   </tr>
						<tr>
							<td colspan="2" class="label">
								<%= Model.Label(300110)%>:  <!-- Include Zero Stock: -->
								<%
									Html.Cortex().CheckBox(s =>
									{
										s.Name = "IncludeZeroStock";
									}).Render();
								%>
							</td>
					   </tr>
					</table>
					<div class="button-container">
						<div class="button-right">
						<%
							Html.Cortex().NegativeButton(s => {
								s.Name = "btnCancel";
								s.Text = Model.LabelText(200245);      // Cancel
								s.UseSubmitBehavior = false;
								s.CausesValidation = false;
								s.ClientSideEvents.Click = "function (s,e) { Get(); }";
							}).Render();
						%></div>
						<div class="button-right">
						<%
							Html.Cortex().PositiveButton(s => {
								s.Name = "btnSave";
								s.Text = Model.LabelText(200247);      // Save;
								s.UseSubmitBehavior = false;
								s.ClientSideEvents.Click = "function (s,e) { Save(); }";
							}).Render();
						%></div>
						<div class="clear"></div>
						</div>
					</div>
				</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
	<%= Html.Cortex().EndForm() %>
</asp:Content>
