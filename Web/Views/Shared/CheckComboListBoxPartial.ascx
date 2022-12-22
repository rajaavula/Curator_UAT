<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CheckComboModel>" %>
<%

    Html.DevExpress().ListBox(Model.Settings.ListBoxSettings).BindList(Model.Data).Render();

%>