<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CheckComboModel>" %>
<%

    var dropDownSettings = Model.Settings.DropDownSettings;

    dropDownSettings.SetDropDownWindowTemplateContent(c =>
        Html.RenderPartial("CheckComboListBoxPartial")
    );;

    Html.DevExpress().DropDownEdit(dropDownSettings).Render();
%>