<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryMappingsListEdit>" %>
<table class="fields">
	<tr>
		<td class="field">
		<%
		if (Model.Products != null)
		{
		%>
			<%= Model.Products %> <%= Model.Label(300115) %> <%-- N products are affected by this mapping. Are you sure you want to continue? --%>
		<%
		}
		else
		{
		%>
			<%= Model.Label(300116) %> <%-- There was an error confirming the products affected by this mapping. --%>
		<%
		}
		%>
		</td>
	</tr>
</table>
<div class="button-container">
	<div class="button-right">
	<%
		Html.Cortex().NeutralButton(s =>
		{
			s.Name = "btnConfirmCancel";
			s.Text = Model.LabelText(200407); // Cancel
			s.Width = Unit.Pixel(100);
			s.CausesValidation = false;
			s.UseSubmitBehavior = false;
			s.ClientSideEvents.Click = "function(s,e) { ppConfirm.Hide(); }";
		}).Render();
	%>
	</div>
	<%
	if (Model.Products != null)
	{
	%>
		<div class="button-right">
		<%
			Html.Cortex().NeutralButton(s =>
			{
				s.Name = "btnConfirmSave";
				s.Text = Model.LabelText(200247); // Save
				s.Width = Unit.Pixel(100);
				s.CausesValidation = false;
				s.UseSubmitBehavior = false;
				s.ClientSideEvents.Click = "function(s,e) { Save(); }";
			}).Render();
		%>
		</div>
	<%
	}
	%>
	<div class="clear"></div>
</div>