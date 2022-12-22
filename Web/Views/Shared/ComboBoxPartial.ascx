<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComboBoxModel>" %>

<% Html.Cortex().ComboBox(Model.Settings).BindList(Model.Data).Bind(Model.Value).Render(); %>