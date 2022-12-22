<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderLineList>" %>
<div class="detail-row-container">
<table class="detail-table">
    <tr>
        <td class="detail-row-bold-heading">Billing address</td>
        <td class="detail-row-bold-heading">Shipping address <input class="detail-button-ship" type="button" value="Edit" onclick="ShowShippingAddress('<%=Model.SalesOrderID%>');"></td>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-customer-is-new-checkbox" class="detail-checkbox" <%if (Model.Order.CustomerIsNew) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-customer-is-new-checkbox">New customer?</label></td>
        <td class="detail-row-bold-heading-spaced">Fraud score</td>
        <td class="detail-row-bold-heading-spaced">No of items</td>
        <td class="detail-row-bold-heading-spaced">Subtotal</td>
        <td class="detail-row-bold-heading-spaced">Shipping</td>
        <td class="detail-row-bold-heading">Customer note</td>
    </tr>
    <tr>
        <td rowspan="3"><textarea id="sales-order<%=Model.SalesOrderID%>-billing-address-memo" rows="5" style="width:310px" readonly="readonly"><%=Model.Order.BillingAddressFormatted%></textarea></td>
        <td rowspan="3"><textarea id="sales-order<%=Model.SalesOrderID%>-shipping-address-memo" rows="5" style="width:310px"><%=Model.ShippingAddress%></textarea></td>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-customer-ip-address-checkbox" class="detail-checkbox" <%if (Model.Order.CustomerIPAddressChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-customer-ip-address-checkbox">IP address checked?</label></td>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="sales-order<%=Model.SalesOrderID%>-fraud-score-box" disabled value="<%=Model.Order.FraudScore%>"></td>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="order-number-of-items-box" disabled value="<%=Model.Order.TotalItems%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="subtotal-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.SubtotalAmount)%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="shipping-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.ShippingAmount)%>"></td>
        <td rowspan="3"><textarea id="CustomerNoteMemo" rows="5" style="width:310px" readonly="readonly"><%=Model.Order.CustomerNote%></textarea></td>
    </tr>
    <tr>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-shipping-address-checkbox" class="detail-checkbox" <%if (Model.Order.ShippingAddressChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-shipping-address-checkbox">Shipping address checked?</label></td>
        <td class="td-spaced"></td>
        <td class="detail-row-bold-heading-spaced">Open items</td>
        <td class="detail-row-bold-heading-spaced">Discount</td>
        <td class="detail-row-bold-heading-spaced">Weight</td>
    </tr>
    <tr>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-fraud-check-checkbox" class="detail-checkbox" <%if (Model.Order.FraudChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-fraud-check-checkbox">Fraud check completed?</label></td>   
        <td class="td-spaced"><input class="detail-button-tall-fraud" type="button" value="Fraud check" onclick="ShowFraudCheck('<%=Model.SalesOrderID%>');"></td>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="open-item-lines-box" disabled value="<%//=Model.Order.NumberOfItems%>"></td> <!-- add caluclated open item lines -->    
        <td class="td-spaced"><input class="detail-number-input" type="text" id="weight-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.TotalWeightGrams)%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="discount-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.DiscountAmount)%>"></td>
    </tr>
</table>
<%
    if (Model.OrderLines.Count > 0)
    {
        %>
        <table class="detail-table">
        <%
        foreach (var orderLine in Model.OrderLines)
        {
            bool nonEDISupplier = true;

            SupplierLineInfo selectedSupplier = orderLine.SelectableSuppliers.Find(x => x.FeedKey == orderLine.SupplierID);
            if (selectedSupplier != null) nonEDISupplier = selectedSupplier.NonEDI;
        %>
            <tr>
                <td colspan="16">
                    <hr>
                </td>
            </tr>
            <tr class="detail-row-grey-header" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row1">
                <td><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-select-order-line" class="detail-checkbox" disabled></td>
                <td class="detail-row-bold-heading">SKU</td>
                <td class="detail-row-bold-heading">Item name</td>
                <td class="detail-row-bold-heading">Status</td>
                <td class="detail-row-bold-heading">Original supplier</td>
                <td class="detail-row-bold-heading">Cost</td>
                <td class="detail-row-bold-heading">New supplier</td>
                <td class="detail-row-bold-heading">New cost</td>
                <td class="detail-row-bold-heading">Stock</td>
                <td class="detail-row-bold-heading">Original #</td>
                <td class="detail-row-bold-heading">Selected #</td>
                <td class="detail-row-bold-heading">Weight</td>
                <td class="detail-row-bold-heading">Subtotal</td>
                <td class="detail-row-bold-heading">Discount</td>
                <td class="detail-row-bold-heading">Shipping</td>
                <td class="detail-row-bold-heading">Total</td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-salesOrderLine<%=orderLine.SalesOrderLineID%>-row2">
                <td><p id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-expand" class="orderline-expand" onclick="ExpandOrderLine('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID %>');">-</p></td>
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-sku" size="20" type="text" value="<%=orderLine.SupplierPartNumber%>" disabled></td> <!-- SKU -->
                <td><input size="20" type="text" value="<%=orderLine.ShortDescription%>" disabled></td> <!-- Item Name -->
                <td>
                    <select class="detail-dropdown" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-status-dropdown">
                        <option value="1">Awaiting fulfillment</option>
                        <option value="2">Sent to supplier</option>
                        <option value="3">Shipped</option>
                        <option value="4">Back ordered</option>
                        <option value="5">Cancelled</option>
                    </select>
                </td> <!-- Status -->
                <td><input size="20" type="text" value="<%=orderLine.OriginalSupplier%>" disabled></td> <!-- Original Supplier -->
                <td><input class="detail-number-input" type="number" value="<%=string.Format("{0:0.00}", orderLine.ResellerBuyEx)%>" disabled></td> <!-- Supplier Cost -->
                <td>
                    <select class="detail-dropdown" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-supplier-dropdown" onchange="UpdateNewSupplier('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID %>');">
                        <option value="None" data-edi="False"></option>
                        <%
                            foreach (SupplierLineInfo supplier in orderLine.SelectableSuppliers)
                            {
                            %>
                                <option <%if (supplier.FeedKey == orderLine.SupplierID) {%>selected<%};%> value="<%=supplier.FeedKey%>" data-non-edi="<%=supplier.NonEDI%>"><%=supplier.FeedName%></option>
                            <%
                            }
                        %>
                    </select>
                </td> <!-- New Supplier -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-cost" disabled></td> <!-- New Supplier Cost -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-stock" disabled></td> <!-- Supplier Stock -->
                <td><input class="detail-number-input" type="text" value="<%=orderLine.Quantity%>" disabled></td> <!-- Original QTY -->
                <td><input class="detail-number-input" value="1" min="1" max="999" type="number" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-selected-qty" onchange="UpdateSelectedQuantity('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID %>');"></td> <!-- Selected QTY -->  <!-- The value field will need to change later to be what the supplier sends back in the purchase order table--> 
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-weight" value="<%=orderLine.WeightGrams%>" disabled></td> <!-- Weight -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-subtotal" value="<%=string.Format("{0:0.00}", orderLine.SubtotalAmount)%>" disabled></td> <!-- SubTotal -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-discount" value="<%=string.Format("{0:0.00}", orderLine.DiscountAmount)%>" disabled></td> <!-- Discount -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-shipping" value="<%=string.Format("{0:0.00}", orderLine.ShippingAmount)%>" disabled></td> <!-- Shipping -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-total" value="<%=string.Format("{0:0.00}", orderLine.TotalAmount)%>" disabled></td> <!-- Total -->
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row3">
                <td></td>
                <td class="detail-row-bold-heading">Shipping carrier</td>
                <td class="detail-row-bold-heading">Supplier order no</td>
                <td class="detail-row-bold-heading">Delivery courier</td>
                <td class="detail-row-bold-heading">Tracking number</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row4">
                <td></td>
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-shipping-carrier" size="20" type="text" value="<%//=orderLine.ShippingCarrier%>" <%if (!nonEDISupplier) {%>disabled<%};%>></td> <!-- Shipping carrier -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-shipping-order-no" size="20" type="text" <%if (!nonEDISupplier) {%>disabled<%};%>></td> <!-- Supplier order no -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-delivery-courier" size="20" type="text" <%if (!nonEDISupplier) {%>disabled<%};%>></td> <!-- Delivery courier -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-tracking-number" size="20" type="text" value="<%//=orderLine.TrackingNumber%>" <%if (!nonEDISupplier) {%>disabled<%};%>></td> <!-- Tracking number -->
                <td></td>
                <td><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-failed-edi-checkbox" class="detail-checkbox" disabled> <label for="failed-edi-checkbox"> Failed EDI delivery?</label></td>
                <td colspan="2"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-non-edi-checkbox" class="detail-checkbox" disabled> <label for="non-edi-checkbox"> Non EDI supplier?</label></td>
                <td colspan="2"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-requires-shipping-checkbox"  <%if (orderLine.RequiresShipping) {%>checked<%};%> class="detail-checkbox" disabled> <label for="requires-shipping-checkbox"> Requires shipping?</label></td>
                <td></td>
                <% 
                if (Model.CanEdit)
                {
                %>
                    <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-save" class="detail-button-tall-save" type="button" value="Save" onclick="SaveOrderLine('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID%>');"></td>
                <%
                }
                else
                {
                %>
                    <td></td>
                <%
                }
                if (Model.CanEdit)
                {
                %>
                    <td colspan="2"><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-push-to-supplier-button" type="button" value="Push to supplier" onclick="ShowPushToSupplier('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID%>');" 
                    <%if (!Model.Order.FraudChecked || orderLine.Quantity < 1 || !orderLine.SupplierID.HasValue) {%>disabled class="detail-button-tall-push-disabled"<%} else { %>class="detail-button-tall-push"<%};%>></td>
                <%
                }
                else
                {
                %>
                    <td colspan="2"></td>
                <%
                }
                %>
                <%--<td><input class="detail-button-tall-invoice" type="button" value="Invoice"></td>--%> <!--This will be added later-->
                <td></td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row5">
                <td></td>
                <td class="detail-row-bold-heading">Member message</td>
                <td></td>
                <td></td>
                <td class="detail-row-bold-heading">Supplier message</td>
                <td></td>
                <td></td>
                <td></td>
                <td colspan="2" class="detail-row-bold-heading">Delivery instructions</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row6">
                <td></td>
                <td colspan="3"><textarea id="MemberMessageMemo" style="width:500px" rows="2" readonly="readonly"><%=orderLine.InternalNotes%></textarea></td>
                <td colspan="4"><textarea id="SupplierMessageMemo" style="width:500px" rows="2" readonly="readonly"><%=orderLine.PurchaseResult%></textarea></td>
                <td colspan="8"><textarea id="DeliveryInstructionsMemo" style="width:650px" rows="2" readonly="readonly"><%=orderLine.DeliveryInstructions%></textarea></td>
            </tr>
        <%
        }
        %>
        </table>
        <%
    }
%>
</div>