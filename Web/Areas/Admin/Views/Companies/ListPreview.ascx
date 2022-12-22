<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="preview-pane">
	<h3 id="txtName"></h3>
	<table class="fields">
		<tr>
			<td>
				<label><%= Model.Label(200179)%>:</label> <!-- Live: -->
			</td>
			<td>
				<input id="txtLive" class="value" type="text" readonly="readonly" />
			</td>
			<td>
				<label><%= Model.Label(200445)%>:</label> <!-- Theme: -->
			</td>
			<td>
				<input id="txtTheme" class="value" type="text" readonly="readonly" />
			</td>
		</tr>
		<tr>
			<td class="top">
				<label><%= Model.Label(200076)%>:</label> <!-- Notes: -->
			</td>
			<td colspan="3">
				<textarea id="memNotes" class="memo" rows="5" readonly="readonly"></textarea>
			</td>
		</tr>
	</table>
</div>
