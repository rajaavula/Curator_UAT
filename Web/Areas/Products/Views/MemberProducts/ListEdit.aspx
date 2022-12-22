<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<MemberProducts>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Assets/Js/MemberProducts/memberproducts.js?v<%= App.Version %>"></script>
    <script type="text/javascript" src="/Assets/Js/selectize.min.js"></script>    
    <script src="/Assets/Js/cx.js" type="text/javascript"></script>
	<link href="/Assets/Css/selectize.bootstrap2.css" rel="stylesheet" type="text/css" />
	<link href="/Assets/Css/selectize.css" rel="stylesheet" type="text/css" />
	<link href="/Assets/Css/selectize.default.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server">
	<%= Html.Cortex().BeginForm("Products", "MemberProducts", "ListEdit", Model.PageID) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server">
	<div id="title-pane">
		<h2 class="page-title"><%= Model.TranslatedName %></h2>
		<table class="filters">
			<tr>
                <td class="option"><label><%= Model.Label(300071)%>:</label></td><!-- Store -->
				<td class="option">
				<%
					Html.Cortex().ComboBox(s => {
						s.Name = "StoreID";
						s.Properties.ValueField = "StoreID";
						s.Properties.ValueType = typeof(int);
						s.Properties.TextField = "StoreName";
						s.Width = Unit.Pixel(150);						
                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { ChangeMemberStore(); }";
					}).BindList(Model.MemberStoreList).Bind(Model.StoreID).Render();
				%>
				</td>
                <td class="label"><%= Model.Label(300008)%>:</td>  <!-- Member feed -->                           
                <td class="option">
					<div>
					<% Html.RenderPartial("ComboBoxPartial", Model.MemberFeedModel); %>								  					                 
					</div>	
                </td>  			
				
                <td class="label"><%= Model.Label(300009)%>:</td>  <!-- Member category -->                           
                <td class="option">
					<div>                    
					    <%Html.RenderPartial("CheckComboPartial", Model.MemberCategoryModel); %>								  					                 
					</div>	
                </td>                               
                <td class="label"><%= Model.Label(300089)%>:</td>  <!-- Brand -->                           
                <td class="option">
					<div>                    
					    <%Html.RenderPartial("CheckComboPartial", Model.BrandModel); %>								  					                 
					</div>	
                </td>       
				<td class="option">
					<div>
					<%
						Html.Cortex().NeutralButton(true, s =>
						{
							s.Name = "Refresh";
							s.Text = Model.LabelText(200034);
							s.UseSubmitBehavior = false;
							s.ClientSideEvents.Click = "function (s,e) { RefreshGridWithArgs(GrdMain); RefreshGridWithArgs(GrdAvailable); }";
						}).Render();
					%>
					</div>					
				</td>
                <td class="option">
                    <div">
					<%
						Html.Cortex().AssignButton(true, s =>
						{
							s.Name = "Update";
							s.Text = Model.LabelText(300087);
							s.UseSubmitBehavior = false;					
							s.ClientSideEvents.Click = "function (s,e) { UpdateSelection(); }";
                            s.ClientVisible = false;
						}).Render();
					%>
					</div>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <%
		Html.Cortex().PageControl(settings =>
		{
			settings.Name = "tabMain";
			settings.ClientSideEvents.ActiveTabChanged = "function(s,e) { ActiveTabChanged(s,e); }";

			settings.TabPages.Add(t =>
			{
				t.Text = "Pricing / tags";
				t.SetContent(() =>{ 
				%>
					<input id="ProductID" type="hidden" />
                    <input id="txtProductTagIDs" type="hidden" name="ProductTags" value="<%= Model.ProductTag %>" />

                    <table class="grid-container">
                        <tr>
                            <td class="grid-pane">
                                <% Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]); %>
                            </td>
                            <td class="cell">                              
                                <div id="edit-pane">                                   
                                     <label class="heading"><%= Model.Label(300090)%></label> <%--Bulk Import --%>
                                     <table>
                                         <tr id="export-download" class="hide">
			                                <td class="label"><%= Model.LabelText(300091) %>:</td> <%-- Example file --%>
			                                <td class="example-file-link">
                                                <%=@Html.ActionLink( Model.LabelText(300092), "BulkPRExportDownload", new { pageID = Model.PageID })%>
			                                </td>
		                                 </tr>
                                         <tr>
								            <td colspan="2"><% Html.Cortex().UploadControl(Model.BulkPRImportUploadControlSettings).Render(); %></td>
							            </tr>
                                         <tr>
                                             <td id="txtImportSuccess" class="hide dx-controls-disabled" colspan="2">Import successful</td>
                                         </tr>
                                     </table>

                                     <div class="button-container">
                                        <div class="button-right">
                                            <%
									            Html.Cortex().PositiveButton(s => 
									            {
										            s.Name = "btnUploadBulkImport";
										            s.Text = Model.LabelText(200714); // Import
										            s.UseSubmitBehavior = false;
										            s.ClientSideEvents.Click = "function(s,e) { uplBulkPRImport.Upload() }";
									            }).Render();
								            %>
                                        </div>                                     
                                        <div class="clear"></div>
                                    </div>

                                    <br/><br/>
                                    
                                    <label class="heading"><%= Model.Label(201033)%></label>
                                    <!-- Pricing rule -->
                                    <table class="fields">
                                        <tr>
                                            <td colspan="2" class="field required">
                                                <%
                                                    Html.Cortex().RadioButtonList(s =>
                                                    {
                                                        s.Name = "PricingRule";
                                                        s.Properties.ValueField = "ID";
                                                        s.Properties.TextField = "Name";
                                                        s.Width = Setup.DefaultWidth;
                                                        s.Properties.RepeatDirection = RepeatDirection.Vertical;
                                                        s.Properties.Items.Add(new ListEditItem("Enter new RRP $:", 1));
                                                        s.Properties.Items.Add(new ListEditItem("Apply RRP adjustment $:", 2));
                                                        s.Properties.Items.Add(new ListEditItem("Apply RRP adjustment %:", 3));
                                                        s.Properties.Items.Add(new ListEditItem("Apply Buy price adjustment $:", 4));
                                                        s.Properties.Items.Add(new ListEditItem("Apply Buy price adjustment %:", 5));
                                                        s.Properties.ClientSideEvents.SelectedIndexChanged = "function (s,e) { OnRuleChanged(s, e); }";
                                                        s.Properties.ClientSideEvents.Init = "function(s,e){ s.SetSelectedIndex(0); OnRuleChanged(s, e); }";
                                                        s.ControlStyle.CssClass = "dx-controls-default";
                                                    }).Render();
                                                %>							
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label">
                                                <div id="RuleLabel"></div>
                                            </td>
                                            <td class="fiexld reqxuired">
                                                <%
                                                    Html.Cortex().SpinEdit(s =>
                                                    {
                                                        s.Name = "PriceValue";
                                                        s.Width = 100;
                                                        s.Properties.DecimalPlaces = 2;
                                                        s.Properties.DisplayFormatString = "n2";
                                                        s.ControlStyle.HorizontalAlign = HorizontalAlign.Right;
                                                    }).Render();
                                                %>							
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="field">
                                                <%
                                                    Html.Cortex().CheckBox(s =>
                                                    {
                                                        s.Name = "RetailRounding";
                                                        s.Text = Model.LabelText(201035); // Use retail rounding?
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
                                                    s.Text = Model.LabelText(200245);      // Cancel
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
                                                    s.Text = Model.LabelText(300014);      // Save;
                                                    s.UseSubmitBehavior = false;
                                                    s.ClientSideEvents.Click = "function (s,e) { Save(); }";
                                                }).Render();
                                            %>
                                        </div>
                                        <div class="clear"></div>
                                    </div>

                                    <br/><br/>

                                    <label class="heading"><%= Model.Label(200977)%></label>    <!-- Product tags -->
                                    <table class="fields">
                                        <tr>
                                            <td colspan="2" class="label">
                                            <%
                                                Html.Cortex().CheckBox(s =>
                                                {
                                                    s.Name = "DefaultProductTags";
                                                    s.Text = Model.LabelText(300088); // Use default tags?
                                                    s.Properties.ClientSideEvents.CheckedChanged = "function(s,e) { DefaultTagsChanged(s,e); }";
                                                    s.Checked = true;
                                                }).Render();
                                            %>
                                            </td>
                                        </tr>
                                        <tr id="product-tags-row" class="hide">
                                            <td class="label"><%= Model.Label(300016)%>:</td>       <%-- Enter product tags --%>
                                            <td class="field">

                                                <select multiple class="product-tag-list" id='<%=string.Format("product-tag-list{0}", Model.MemberProductTags)%>'>
                                                    <%  foreach (var tag in Model.MemberProductTags) { %>
                                            
                                                        <option value="<%= tag.ProductTags %>"><%= tag.ProductTags %></option>

                                                    <% } %>
                                                </select>

                                            </td>
                                        </tr>

                                    </table>

                                    <div class="button-container">
                                            <div class="button-right">
                                                <%
                                                    Html.Cortex().NegativeButton(s =>
                                                    {
                                                        s.Name = "buttonCancel";
                                                        s.Text = Model.LabelText(200245);      // Cancel
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
                                                        s.Name = "buttonSave";
                                                        s.Text = Model.LabelText(300015);      // Save tags;
                                                        s.UseSubmitBehavior = false;
                                                        s.ClientSideEvents.Click = "function (s,e) { SaveProductTags(); }";
                                                    }).Render();
                                                %>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                </div>
                            </td>
                        </tr>
                    </table>
				<% 
				});
            });
			
			settings.TabPages.Add(t =>
			{
				t.Text = "Product selection";
				t.SetContent(() =>{ 
                %>
                    <table class="grid-container">
                        <tr>
                            <td class="grid-pane">
                                <% Html.RenderPartial("GridViewPartial", Model.Grids["GrdAvailable"]); %>
                            </td>
                        </tr>
                    </table>
                <% 
                });
            });
        }).Render();
	%>
    <%

    Model.BulkPRImportPopUploadSettings.SetContent(() =>
		{
			%><div id="bulk-pr-import-results">
				<textarea id="txtBulkPRImportResults" style="height: 350px; width: 568px; margin-bottom: 8px;"></textarea>
				<div id="button-container">
					<div class="button-right">
					<%
						Html.Cortex().NeutralButton(s => {
							s.Name = "btnBulkPRImportErrorsClose";
							s.Text = Model.LabelText(200407);
							s.UseSubmitBehavior = false;
							s.CausesValidation = false;
							s.ClientSideEvents.Click = "function (s,e) { ppBulkPRImportResult.Hide(); }";
						}).Render();
					%>
					</div>
					<div class="clear"></div>
				</div>
			</div><%
		}); %>
        
    <% Html.Cortex().PopupControl(Model.BulkPRImportPopUploadSettings).Render(); %>

</asp:Content>
<asp:Content ContentPlaceHolderID="EndFormContent" runat="server">
	<%= Html.Cortex().EndForm() %>
</asp:Content>
