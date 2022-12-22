<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="BasePage<CategoriesListEdit>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Assets/Js/Categories/categories.js?v<%= App.Version %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BeginFormContent" runat="server">
    <%= Html.Cortex().BeginForm("Products", "Categories", "ListEdit", Model.PageID) %>
</asp:Content>
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
                            s.ClientSideEvents.Click = "function (s,e) { RefreshGrid(GrdMain); }";
                        }).Render();
                    %>
                </td>
                <td class="option">
                    <%
                        Html.Cortex().PositiveButton(s =>
                        {
                            s.Name = "btnNew";
                            s.Text = Model.LabelText(200036);
                            s.UseSubmitBehavior = false;
                            s.ClientSideEvents.Click = "function(s,e) { New(); }";
                        }).Render();
                    %>
                </td>
                <td class="option">
                    <%
                        Html.Cortex().NegativeButton(s =>
                        {
                            s.Name = "btnDelete";
                            s.Text = Model.LabelText(200324);
                            s.UseSubmitBehavior = false;
                            s.ClientSideEvents.Click = "function(s,e) { Delete(); }";
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
            <td class="cell">
                <div id="grid-pane">
                    <%Html.RenderPartial("GridViewPartial", Model.Grids["GrdMain"]);%>
                </div>
            </td>
            <td class="cell">
                <div id="edit-pane">
                    <label class="heading"><%= Model.Label(200325)%></label>                    <!-- Edit details -->
                    <input id="CategoryKey" type="hidden" />
                    <table class="fields">
                        <tr>
                            <td class="label"><%= Model.Label(200016)%>:</td>                            <!-- Name -->                            
                        </tr>
                        <tr>
                            <td class="field required">
                                <%
                                    Html.Cortex().TextBox(s =>
                                    {
                                        s.Name = "Name";
                                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                                    }).Render();
                                %>
                            </td>
                        </tr>
                        <tr>
                            <td class="label"><%= Model.Label(200820)%>:</td>                            <!-- Description -->                         
                        </tr>
                        <tr>
                            <td class="field required">
                                <%
                                    Html.Cortex().TextBox(s =>
                                    {
                                        s.Name = "Description";
                                        s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                                        s.Properties.ValidationSettings.RequiredField.IsRequired = true;
                                    }).Render();
                                %>
                            </td>
                        </tr>
                        <tr>
                            <td class="label"><%=Model.Label(200988) %>:</td>                            <!--  Parent category -->                         
                        </tr>
                        <tr>
                            <td class="field required">
                                <% Html.RenderPartial("ComboBoxPartial", Model.ParentCategoryModel); %>								  
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
                                    s.Text = Model.LabelText(200247);      // Save;
                                    s.UseSubmitBehavior = false;
                                    s.ClientSideEvents.Click = "function (s,e) { Save(); }";
                                }).Render();
                            %>
                        </div>
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
