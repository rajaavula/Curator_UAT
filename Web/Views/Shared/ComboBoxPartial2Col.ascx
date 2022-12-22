<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComboBoxModel>" %>
<% Html.Cortex().ComboBox(Model.Settings, 2).BindList(Model.Data).Bind(Model.Value).Render(); %>