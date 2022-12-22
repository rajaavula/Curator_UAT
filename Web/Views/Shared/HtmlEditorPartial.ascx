<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HtmlEditorModel>" %><%
	Html.DevExpress().HtmlEditor(Model.Settings).Render();
%>