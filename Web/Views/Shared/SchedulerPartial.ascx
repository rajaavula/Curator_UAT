<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SchedulerModel>" %>
<%

if(string.IsNullOrEmpty(Model.TemplatePath) == false)
{
	Model.Settings.Views.DayView.SetVerticalAppointmentTemplateContent
	(
		c => Html.RenderPartial(Model.TemplatePath, c.AppointmentViewInfo)
	);
}

Model.Settings.Views.DayView.SetHorizontalResourceHeaderTemplateContent(c =>
{
	Model.CurrentResource = c.Resource;
	Html.RenderPartial("~/Areas/Scheduling/Views/WorkOrders/ResTemplate.ascx", Model);
});

Html.DevExpress().Scheduler(Model.Settings).Bind(Model.FetchAppointments, Model.Resources).Render();

%>