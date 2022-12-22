<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ChartModel>" %>
<% Html.DevExpress().Chart(Model.Settings).Render(); %>