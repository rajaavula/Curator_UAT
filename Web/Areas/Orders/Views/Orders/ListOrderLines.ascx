<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderLineList>" %>
<div class="detail-row-container">
<table class="detail-table order-header">
    <tr>
        <td class="detail-row-bold-heading">Billing address</td>
        <td class="detail-row-bold-heading">Shipping address 
            <% 
            if (Model.CanEdit)
            {
            %>
                <input class="detail-button-ship" type="button" value="Edit" onclick="ShowShippingAddress('<%=Model.SalesOrderID%>');">
            <%
            }
            %>
        </td>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-customer-is-new-checkbox" class="detail-checkbox" <%if (Model.Order.CustomerIsNew) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-customer-is-new-checkbox">New customer?</label></td>
        <td class="detail-row-bold-heading-spaced">Fraud score</td>
        <td class="detail-row-bold-heading-spaced">No of items</td>
        <td class="detail-row-bold-heading-spaced">Subtotal</td>
        <td class="detail-row-bold-heading-spaced">Shipping</td>
        <td class="detail-row-bold-heading">Customer note</td>
    </tr>
    <tr>
        <td rowspan="3"><textarea id="sales-order<%=Model.SalesOrderID%>-billing-address-memo" rows="5" style="width:250px;" readonly="readonly"><%=Model.Order.BillingAddressFormatted%></textarea></td>
        <td rowspan="3"><textarea id="sales-order<%=Model.SalesOrderID%>-shipping-address-memo" rows="5" style="width:250px;"><%=Model.ShippingAddress%></textarea></td>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-customer-ip-address-checkbox" class="detail-checkbox" <%if (Model.Order.CustomerIPAddressChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-customer-ip-address-checkbox">IP address checked?</label></td>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="sales-order<%=Model.SalesOrderID%>-fraud-score-box" disabled value="<%=Model.Order.FraudScore%>"></td>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="order-number-of-items-box" disabled value="<%=Model.Order.TotalItems%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="subtotal-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.SubtotalAmount)%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="shipping-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.ShippingAmount)%>"></td>
        <td rowspan="3"><textarea id="CustomerNoteMemo" rows="5" style="width:280px;" readonly="readonly"><%=Model.Order.CustomerNote%></textarea></td>
    </tr>
    <tr>
        <td colspan="2" class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-shipping-address-checkbox" class="detail-checkbox" <%if (Model.Order.ShippingAddressChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-shipping-address-checkbox">Shipping address checked?</label></td>
        <td class="detail-row-bold-heading-spaced">Open lines</td>
        <td class="detail-row-bold-heading-spaced">Discount</td>
        <td class="detail-row-bold-heading-spaced">Weight</td>
    </tr>
    <tr>
        <td class="td-spaced"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-fraud-check-checkbox" class="detail-checkbox" <%if (Model.Order.FraudChecked) {%>checked<%};%> disabled> <label for="sales-order<%=Model.SalesOrderID%>-fraud-check-checkbox">Fraud check completed?</label></td>   
        <% 
        if (Model.CanEdit)
        {
        %>
            <td class="td-spaced"><input class="detail-button-tall-fraud" type="button" value="Fraud check" onclick="ShowFraudCheck('<%=Model.SalesOrderID%>');"></td>
        <%
        }
        %>
        <td class="td-spaced"><input class="detail-number-wide-input" size="7" type="text" id="open-item-lines-box" disabled value="<%=Model.Order.OpenItemLines%>"></td>   
        <td class="td-spaced"><input class="detail-number-input" type="text" id="weight-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.TotalWeightGrams)%>"></td>
        <td class="td-spaced"><input class="detail-number-input" type="text" id="discount-box" disabled value="<%=string.Format("{0:0.00}", Model.Order.DiscountAmount)%>"></td>
    </tr>
</table>
<%
    if (Model.OrderLines.Count > 0)
    {
        %>
        
        <%
        foreach (var orderLine in Model.OrderLines)
        {
            bool isEDISupplier = false;

            SupplierLineInfo selectedSupplier = orderLine.SelectableSuppliers.Find(x => x.FeedKey == orderLine.SupplierID);
            if (selectedSupplier != null) isEDISupplier = selectedSupplier.IsEDISupplier;
        %>
        <table class="detail-table order-lines">
            <tr>
                <td colspan="14">
                    <hr>
                </td>
            </tr>
            <tr class="detail-row-grey-header" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row1">
                <%
                if (Model.CanConfirm)
                {
                %>
                    <td><input type="checkbox" class="order-<%= Model.SalesOrderID %>-line-select" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-select-order-line" class="detail-checkbox" data-id="<%= orderLine.SalesOrderLineID %>"
                    <%if (!Model.Order.FraudChecked || orderLine.Quantity < 1 || !orderLine.SupplierID.HasValue) {%>disabled<%}%>></td>
                <%
                }
                else
                {
                %>
                    <td></td>
                <%
                }
                %>
                <td class="detail-row-bold-heading-white">SKU</td>
                <td class="detail-row-bold-heading-white">Item name</td>
                <td class="detail-row-bold-heading-white">Status</td>
                <td class="detail-row-bold-heading-white">Original supplier</td>
                <td class="detail-row-bold-heading-white">Cost</td>
                <td class="detail-row-bold-heading-white">New supplier</td>
                <td class="detail-row-bold-heading-white">New cost</td>
                <td class="detail-row-bold-heading-white">Stock</td>
                <td class="detail-row-bold-heading-white">Original #</td>
                <td class="detail-row-bold-heading-white">Selected #</td>
                <%--<td class="detail-row-bold-heading-white">Weight</td>--%>
                <td class="detail-row-bold-heading-white">Subtotal</td>
                <%--<td class="detail-row-bold-heading-white">Discount</td>--%>
                <td class="detail-row-bold-heading-white">Shipping</td>
                <td class="detail-row-bold-heading-white">Total</td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-salesOrderLine<%=orderLine.SalesOrderLineID%>-row2">
                <td><p class="orderline-expand" onclick="ExpandOrderLine(this);">+</p></td>
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-sku" size="20" type="text" value="<%=orderLine.SupplierPartNumber%>" disabled></td> <!-- SKU -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-item-name" size="20" type="text" value="<%=orderLine.ShortDescription%>" disabled></td> <!-- Item Name -->
                <td>
                    <select class="detail-dropdown" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-status-dropdown" <%if (isEDISupplier) {%>disabled<%};%>>
                        <%
                        foreach (SalesOrderStatusInfo salesOrderStatus in Model.SalesOrderStatuses)
                        {
                        %>
                            <option <%if (salesOrderStatus.SalesOrderLineStatusID == orderLine.SalesOrderLineStatusID) {%>selected<%};%> value="<%=salesOrderStatus.SalesOrderLineStatusID%>"><%=salesOrderStatus.StatusName%></option>
                        <%
                        }
                        %>
                    </select>
                </td> <!-- Status -->
                <td><input size="20" type="text" value="<%=orderLine.OriginalSupplier%>" disabled></td> <!-- Original Supplier -->
                <td><input class="detail-number-input" type="number" value="<%=string.Format("{0:0.00}", orderLine.ResellerBuyEx)%>" disabled></td> <!-- Supplier Cost -->
                <td>
                    <select class="detail-dropdown" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-supplier-dropdown" onchange="UpdateNewSupplier('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID %>');"  <%if (orderLine.PurchaseOrderID.HasValue) {%>disabled<%};%>>
                        <option data-is-edi="False"></option>
                        <%
                        foreach (var supplier in orderLine.SelectableSuppliers)
                        {
                        %>
                            <option <%if (supplier.FeedKey == orderLine.SupplierID) {%>selected<%};%> value="<%=supplier.FeedKey%>" data-is-edi="<%=supplier.IsEDISupplier%>"><%=supplier.FeedName%></option>
                        <%
                        }
                        %>
                    </select> 
                </td> <!-- New Supplier -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-cost" value="<%= string.Format("{0:0.00}", orderLine.PurchaseCostEx) %>" disabled></td> <!-- New Supplier Cost -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-new-stock" value="<%= orderLine.SupplierStock %>" disabled></td> <!-- Supplier Stock -->
                <td><input class="detail-number-input" type="text" value="<%=orderLine.Quantity%>" disabled></td> <!-- Original QTY -->
                <td><input class="detail-number-input" value="<%=orderLine.Quantity%>" min="1" max="999" type="number" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-selected-qty" onchange="UpdateSelectedQuantity('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID %>');" disabled></td> <!-- Selected QTY -->
                <%--<td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-weight" value="<%=orderLine.WeightGrams%>" disabled></td> <!-- Weight -->--%>
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-subtotal" value="<%=string.Format("{0:0.00}", orderLine.SubtotalAmount)%>" disabled></td> <!-- SubTotal -->
                <%--<td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-discount" value="<%=string.Format("{0:0.00}", orderLine.DiscountAmount)%>" disabled></td> <!-- Discount -->--%>
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-shipping" value="<%=string.Format("{0:0.00}", orderLine.ShippingAmount)%>" disabled></td> <!-- Shipping -->
                <td><input class="detail-number-input" type="text" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-total" value="<%=string.Format("{0:0.00}", orderLine.TotalAmount)%>" disabled></td> <!-- Total -->
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row3" class="order-line hide">
                <td class="detail-row-bold-heading"></td>
                <td class="detail-row-bold-heading">Supplier order no</td>
                <td class="detail-row-bold-heading">Delivery courier</td>
                <td class="detail-row-bold-heading">Tracking number</td>
                <td></td>
                <td colspan="3" class="detail-row-bold-heading">Order notes</td>
                <td colspan="3" class="detail-row-bold-heading">Supplier message</td>
                <td colspan="3" class="detail-row-bold-heading">Delivery instructions</td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row4" class="order-line hide">
                <td></td>
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-shipping-order-no" size="20" type="text" value="<%=orderLine.SupplierOrderNumber%>" disabled></td> <!-- Supplier order no -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-delivery-courier" size="20" type="text" value="<%=orderLine.CarrierName%>" <%if (isEDISupplier) {%>disabled<%};%>></td> <!-- Delivery courier -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-tracking-number" size="20" type="text" value="<%=orderLine.TrackingNumber%>" <%if (isEDISupplier) {%>disabled<%};%>></td> <!-- Tracking number -->
                <td></td>
                <td colspan="3"><textarea id="OrderNotesMemo" title="<%=orderLine.InternalNotes%>" style="width:280px; height:21px;" readonly="readonly"><%=orderLine.InternalNotes%></textarea></td>
                <td colspan="3"><textarea id="SupplierMessageMemo" title="<%=orderLine.PurchaseResult%>" style="width:255px; height:21px;" readonly="readonly"><%=orderLine.PurchaseResult%></textarea></td>
                <td colspan="3"><textarea id="DeliveryInstructionsMemo" title="<%=orderLine.DeliveryInstructions%>" style="width:255px; height:21px;" readonly="readonly"><%=orderLine.DeliveryInstructions%></textarea></td>
            </tr>
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row5" class="order-line hide">
                <td></td>
                <td class="detail-row-bold-heading">Supplier freight code</td>
                <td class="detail-row-bold-heading">Supplier freight cost</td>
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
            <tr id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-row6" class="order-line hide">
                <td></td>
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-supplier-freight-code" type="text" value="<%=orderLine.SupplierFreightCode%>"}></td> <!-- Supplier Freight Code -->
                <td><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-supplier-freight-cost" class="detail-number-input-wide" min="0" step="0.50" type="number" value="<%=string.Format("{0:0.00}", orderLine.SupplierFreightCost)%>" onkeyup="if(this.value < 0){this.value = this.value * -1}"></td> <!-- Supplier Freight Cost -->
                <td><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-failed-edi-checkbox" class="detail-checkbox" disabled> <label for="failed-edi-checkbox"> Failed EDI delivery?</label></td>
                <td><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-requires-shipping-checkbox"  <%if (orderLine.RequiresShipping) {%>checked<%};%> class="detail-checkbox" disabled> <label for="requires-shipping-checkbox"> Requires shipping?</label></td>
                <td colspan="2"><input type="checkbox" id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-is-edi-checkbox" class="detail-checkbox" <%if (isEDISupplier) {%>checked<%};%> disabled> <label for="is-edi-checkbox"> EDI supplier?</label></td>
                <td></td>
                <td></td>
                <td></td>
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
                if (Model.CanConfirm)
                {
                    var disabled = !Model.Order.FraudChecked || orderLine.Quantity < 1 || !orderLine.SupplierID.HasValue || orderLine.PurchaseQueueDate != null;

                    var text = orderLine.PurchaseQueueDate != null ? "Pushed to supplier" : "Push to supplier";

                    var className = disabled ? "detail-button-tall-push-disabled" : "detail-button-tall-push";

                    className = orderLine.PurchaseQueueDate != null ? "detail-button-tall-pushed pushed" : className;

                    className = string.Format("{0} order-{1}-push-button", className, Model.SalesOrderID);
                %>
                    <td colspan="2"><input id="sales-order<%=Model.SalesOrderID%>-orderline<%=orderLine.SalesOrderLineID%>-push-to-supplier-button" type="button" value="<%= text %>" onclick="ShowPushToSupplier('<%=Model.SalesOrderID%>','<%=orderLine.SalesOrderLineID%>');" data-id="<%= orderLine.SalesOrderLineID %>"
                    <%if (disabled) {%>disabled <%}%> class="<%= className %>"></td>
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
            </tr>
        </table>
        <%
        }
        %>
        <%
    }
%>
</div>