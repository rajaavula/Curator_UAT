<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HomeDetails>" %>
<h2 class="title">Account options</h2>
<div id="account-detail-success" style="display: none;">
	<p class="nowrap">Your account options have been updated successfully.</p>
	<div class="button-container">
		<div class="button-right"><%
			Html.Cortex().NeutralButton(settings => {
				settings.Name = "btnAccountDetailsClose";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Close";
				settings.ClientSideEvents.Click = "function(s,e) { ppAccountDetails.Hide(); }";
			}).Render();
		%></div>
		<div class="clear"></div>
	</div>
</div>
<div id="account-details-update">
	<table class="fields">
		<tr>
			<td class="field">
			<%
				Html.Cortex().CheckBox(s => {
					s.Name = "NewProductNotifications";
					s.Text = Model.Label(300093);
					s.Width = 220;
				}).Bind(Model.NewProductNotifications).Render();
			%></td>
		</tr>
		<tr>
			<td class="field"><%
				Html.Cortex().CheckBox(s => {
					s.Name = "ChangedProductNotifications";
					s.Text = Model.Label(300094);
					s.Width = 220;
				}).Bind(Model.ChangedProductNotifications).Render();
			%></td>
		</tr>
		<tr>
			<td class="field"><%
				Html.Cortex().CheckBox(s => {
					s.Name = "DeactivatedProductNotifications";
					s.Text = Model.Label(300095);
					s.Width = 220;
				}).Bind(Model.DeactivatedProductNotifications).Render();
			%></td>
		</tr>
	</table>
	<div id="account-details-error" class="error"></div>
	<div class="button-container">
		<div class="button-right">
		<%
			Html.Cortex().NegativeButton(settings => {
				settings.Name = "btnAccountDetailsCancel";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Cancel";
				settings.ClientSideEvents.Click = "function(s,e) { ppAccountDetails.Hide(); }";
			}).Render();
		%>
		</div>
		<div class="button-right">
		<%
			Html.Cortex().PositiveButton(settings => {
				settings.Name = "btnAccountDetailsSave";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Save";
				settings.ClientSideEvents.Click = "function(s,e) { SaveAccountDetails(); }";
			}).Render();
		%>
		</div>
		<div class="clear"></div>
	</div>
</div>
