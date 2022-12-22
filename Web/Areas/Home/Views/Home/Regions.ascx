<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HomeRegions>" %>
<h2 class="title">Change region</h2>
<div id="account-regions-success" style="display: none;">
	<p class="nowrap">Your region has been updated successfully.</p>
	<div class="button-container">
		<div class="button-right"><%
			Html.Cortex().NeutralButton(settings => {
				settings.Name = "btnAccountRegionsClose";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Close";
				settings.ClientSideEvents.Click = "function(s,e) { ppAccountRegions.Hide(); window.location.reload(); }";
			}).Render();
		%></div>
		<div class="clear"></div>
	</div>
</div>
<div id="account-regions-update">
	<table class="fields">
		<tr>
			<td class="label">Region:</td>
			<td class="field"><%
				Html.Cortex().ComboBox(settings => {
					settings.Name = "txtAccountRegionID";
					settings.Width = 220;
					settings.Properties.ValueField = "RegionID";
					settings.Properties.TextField = "Name";
					settings.Properties.ValueType = typeof (int);
					settings.Properties.ValidationSettings.RequiredField.IsRequired = true;
					settings.Properties.ValidationSettings.ValidationGroup = "ACCOUNT_REGIONS";
					settings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
				}).BindList(Model.Regions).Bind(Model.SI.CurrentRegion.RegionID).Render();
			%></td>
		</tr>
	</table>
	<div id="account-regions-error" class="error"></div>
	<div class="button-container">
		<div class="button-right"><%
			Html.Cortex().NegativeButton(settings => {
				settings.Name = "btnAccountRegionsCancel";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Cancel";
				settings.ClientSideEvents.Click = "function(s,e) { ppAccountRegions.Hide(); }";
			}).Render();
		%></div>
		<div class="button-right"><%
			Html.Cortex().PositiveButton(settings => {
				settings.Name = "btnAccountRegionsSave";
				settings.Width = Unit.Pixel(100);
				settings.Text = "Save";
				settings.ClientSideEvents.Click = "function(s,e) { SaveAccountRegion(); }";
			}).Render();
		%></div>
		<div class="clear"></div>
	</div>
</div>
