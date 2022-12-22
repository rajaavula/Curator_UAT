<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GridModel>" %>
<% Html.DevExpress().GridView(Model.Settings).Bind(Model.Data).Render(); %>