<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GridModel>" %>
<div id="preview-pane">
	<h3><%: Model.Label(200968) %></h3>
	<div class="long-desc">
		<textarea id="LongDescription" readonly="readonly" class="dx-controls-read-only"></textarea>
	</div>
	
</div>