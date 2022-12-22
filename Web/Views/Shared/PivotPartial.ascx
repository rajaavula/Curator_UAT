<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PivotModel>" %>
<% Html.DevExpress().PivotGrid(Model.Settings).Bind(Model.Data).Render(); %>