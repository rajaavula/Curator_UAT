<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GridModel>" %>
<div id="preview-pane" class="member-products-preview-pane">
	<h3><%: Model.Label(300013) %></h3>
	<table>
		<tr >			
			<td>
				<label class="label-padding"><%= Model.Label(201033)%>:</label> <!-- Pricing Rule -->
			</td>
			<td>
				<input id="txtPricingRuleText" class="value dx-controls-read-only" type="text" readonly="readonly" />
			</td>
			<td>
				<label class="label-padding"><%= Model.Label(201039)%>:</label> <!-- Price value -->
			</td>
			<td>
				<input id="txtPriceValue" class="value dx-controls-read-only tright" type="text" readonly="readonly" />
			</td>
			<td>
				<label class="label-padding"><%= Model.Label(201040)%>:</label> <!-- Retail rounding -->
			</td>	
			<td>
				<input id="txtRetailRounding" class="value dx-controls-read-only" type="text" readonly="readonly" />
			</td>
			<td>
				<label class="label-padding"><%= Model.Label(201036)%>:</label> <!-- New RRP -->
			</td>
			<td>
                <input id="txtNewRRP" class="value dx-controls-read-only tright" type="text" readonly="readonly" />
			</td>
		</tr>		
	</table>
</div>