<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CompanyList>" %>
<table id="new-company" class="popup">
	<tr>
		<td class="label"><%= Model.Label(200446)%>:</td><%-- Company name --%>
		<td class="field"><%
			Html.Cortex().TextBox(s => {
				s.Name = "NewCompany_CompanyName";
				s.Width = Unit.Pixel(308);
				s.Properties.MaxLength = 100;
				s.Properties.ValidationSettings.RequiredField.IsRequired = true;
				s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
			}).Render();
		%></td>
	</tr>
	<tr>
		<td class="label"><%= Model.Label(200579)%>:</td><%-- Owner name --%>
		<td class="field" colspan="4"><%
			Html.Cortex().TextBox(s => {
				s.Name = "NewCompany_OwnerName";
				s.Width = Unit.Pixel(308);
				s.Properties.MaxLength = 100;
				s.Properties.ValidationSettings.RequiredField.IsRequired = true;
				s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
			}).Render();
		%></td>
	</tr>
	<tr>
		<td class="label"><%= Model.Label(200580)%>:</td><%-- Owner login --%>
		<td class="field"><%
			Html.Cortex().TextBox(s => {
				s.Name = "NewCompany_OwnerLogin";
				s.Width = Unit.Pixel(308);
				s.Properties.MaxLength = 50;
				s.Properties.ValidationSettings.RequiredField.IsRequired = true;
				s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
				s.Properties.ValidationSettings.RegularExpression.ValidationExpression = "^[A-Za-z0-9_-]*$";
			}).Render();
		%></td>
	</tr>
	<tr>
		<td class="label"><%= Model.Label(200581)%>:</td><%-- Owner email --%>
		<td class="field"><%
			Html.Cortex().TextBox(s => {
				s.Name = "NewCompany_OwnerEmail";
				s.Width = Unit.Pixel(308);
				s.Properties.MaxLength = 150;
				s.Properties.ValidationSettings.RequiredField.IsRequired = true;
				s.Properties.ValidationSettings.RegularExpression.ValidationExpression = App.EmailRegex;
				s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
			}).Render();
		%></td>
	</tr>
	<tr>
		<td class="label"><%= Model.Label(200582)%>:</td><%-- Owner password --%>
		<td class="field"><%
			Html.Cortex().TextBox(s => {
				s.Name = "NewCompany_OwnerPassword";
				s.Width = Unit.Pixel(308);
				s.Properties.MaxLength = 150;
				s.Properties.ValidationSettings.RequiredField.IsRequired = true;
				s.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
			}).Render();
		%></td>
	</tr>
</table>
<div class="button-container">
	<div class="button-right">
	<%
		Html.Cortex().NeutralButton(settings => {
			settings.Name = "btnNewCompanyClose";
			settings.Text = Model.LabelText(200407); // Close
			settings.UseSubmitBehavior = false;
			settings.CausesValidation = false;
			settings.ClientSideEvents.Click = "function(s,e) { HidePopUp(); }";
		}).Render();
	%>
	</div>
	<div class="button-right">
	<%
		Html.Cortex().PositiveButton(settings => {
			settings.Name = "btnNewCompanySave";
			settings.Text = Model.LabelText(200247); // Save
			settings.UseSubmitBehavior = false;
			settings.CausesValidation = false;
			settings.ClientSideEvents.Click = "function(s,e) { SaveNewCompany(); }";
		}).Render();
	%>
	</div>
	<div class="clear"></div>
</div>