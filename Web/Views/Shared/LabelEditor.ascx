<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
	Html.Cortex().PopupControl(s => {
		s.Name = "ppLabelEditor";
		s.Width = 380;
		s.HeaderText = Model.LabelText(200417);                        // Label Editor
		s.Modal = false;
		s.PopupHorizontalAlign = PopupHorizontalAlign.OutsideRight;
		s.PopupVerticalAlign = PopupVerticalAlign.Below;
		s.PopupHorizontalOffset = 12;
		s.PopupVerticalOffset = 12;
		s.AutoUpdatePosition = true;
		s.SetContent(() => {
%>
<input id="LabelEditor_PlaceholderID" type="hidden" value="" />
<table class="fields">
	<tr>
		<td class="label"><%= Model.Label(200332)%></td>          <!-- Label Text -->
		<td class="field">
		<%
			Html.Cortex().TextBox(settings => {
				settings.Name = "LabelEditor_LabelText";
				settings.Width = 250;
			}).Render();
		%>
		</td>
	</tr>
	<tr>
		<td class="label"><%= Model.Label(200333)%></td>          <!-- Tool Tip Text -->
		<td class="field">
		<%
			Html.Cortex().TextBox(settings => {
				settings.Name = "LabelEditor_ToolTipText";
				settings.Width = 250;
			}).Render();
		%>
		</td>
	</tr>
</table>
<div class="button-container">
	<div class="button-right">
		<%
			Html.Cortex().NegativeButton(settings =>
			{
				settings.Name = "LabelEditor_Cancel";
				settings.Text = Model.LabelText(200245);                // Cancel
				settings.CausesValidation = false;
				settings.UseSubmitBehavior = false;
				settings.ClientSideEvents.Click = "function (s,e) { ppLabelEditor.Hide(); }";
			}).Render();
		%>
	</div>
	<div class="button-right">
		<%
		Html.Cortex().PositiveButton(settings => {
			settings.Name = "LabelEditor_Save";
            settings.Text = Model.LabelText(200247);                   // Save
			settings.ClientSideEvents.Click = "function(s,e) { SaveLabel(); }";
		}).Render();
	%>
	</div>
</div>
<div class="clear"></div>
<%
		});
	}).Render();
%>
