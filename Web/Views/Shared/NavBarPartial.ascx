<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<% Html.DevExpress().NavBar(Model.NavBarSettings).Render(); %>