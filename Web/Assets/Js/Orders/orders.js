var selectedStores

// Refresh Main Grid
function RefreshGridWithArgs(grid) {

	var args = FormatDate(FromDate.GetDate()) + '~' + FormatDate(ToDate.GetDate()) + '~' + StoreID_ListBox.GetSelectedValues() + '~' + OrderStatus.GetValue() + '~' + FailedEDIDelivery.GetChecked() + '~' + IncompleteItemLines.GetChecked();

    grid.PerformCallback(args);
}

function DateRangeChanged(grid) {

	var args = DateRange.GetValue() + '~' + StoreID_ListBox.GetSelectedValues() + '~' + OrderStatus.GetValue() + '~' + FailedEDIDelivery.GetChecked() + '~' + IncompleteItemLines.GetChecked();

	grid.PerformCallback(args);

	LoadDateRanges();
}

function LoadDateRanges() {

	var id = DateRange.GetValue();
    if (!id) return;

    $.ajax({
        cache: false,
        url: '/Orders/Orders/LoadDateRange',
		data: { pageID: GetPageID(), dateRangeID: id },
        success: function (json) {
			if (!json) return;
			FromDate.SetDate(ParseMSDate(json.FromDate));
			ToDate.SetDate(ParseMSDate(json.ToDate));
        }
    });
}

// Controls
function Store_BeginCallback(s, e) {

	e.customArgs['PageID'] = GetPageID();
}

function Store_EndCallback(s, e) {

	var listbox = StoreID_ListBox;
	var items = listbox.GetItemCount();

	if (items === 0) return;

	if (items === 1) {
		listbox.SelectAll();
	}
	else {
		if (selectedStores != null) {
			listbox.SelectValues(selectedStores);
			selectedStores = null;
		}
		else {
			listbox.SetSelectedIndex(1);
		}
	}

	cxUpdateText(StoreID, listbox);
}

// Popups
function ShowErrorMessage(errorMessage) {
	var width = 500;
	$('#error-message-popup').width(width);
	$('#error-message-popup').css('left', (window.innerWidth / 2) - (width / 2));
	$('#error-message-popup').css('top', '40%');
	$('#error-message-buttons').width(width - 160);

	$('#error-message-popup-message').html(errorMessage);
	ShowPopup('#error-message-popup');
}

// General Page
function ActiveTabChanged(s, e) {

	var selectedOrder = GetRowID(GrdOrders);

	UpdateAllTabs(selectedOrder);
}

function UpdateAllTabs(selectedOrder) {

	$.ajax({
		cache: false,
		url: '/Orders/Orders/UpdateTabs',
		data: { pageID: GetPageID(), salesOrderID: selectedOrder },
		success: function (json) {
			if (!json) return;
			GrdHistory.PerformCallback(json.SalesOrderID);
		}
	});
}

function DetailRowExpanding(s, e) {
	GrdOrders.SetFocusedRowIndex(e.visibleIndex);
}

function ExpandOrderLine(salesOrderID, salesOrderLineID) {

	var hidden = false;
	var firstRow = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + "-row1";
	var secondRow = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + "-row2";
	var orderRows = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + "-row";
	var orderExpandLink = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-expand';
	var orderLines = $("[id ^= " + orderRows + "]");

	orderLines.each(function () {
		if (!this.id.match(firstRow) && !this.id.match(secondRow))
		{
			if (this.classList.contains('hide')) {
				this.classList.remove('hide');
				hidden = true;
			}
			else {
				this.classList.add('hide');
				hidden = false;
			}
		}
	});

	if (hidden) $('#' + orderExpandLink).text('-');
	else $('#' + orderExpandLink).text('+');
}

// Shipping Address Popup
function ShowShippingAddress(salesOrderID) {

	var width = 500;
	$('#shipping-address-popup').width(width);
	$('#shipping-address-popup').css('left', (window.innerWidth / 2) - (width / 2));
	$('#shipping-address-popup').css('top', '20%');
	$('#shipping-address-buttons').width(width - 50);
	$('#shipping-address-popup-sales-order').val(salesOrderID);

	$.ajax({
		cache: false,
		url: '/Orders/Orders/GetShippingAddress',
		data: {
			salesOrderID: salesOrderID
		},
		success: function (json) {
			if (!json) return;

			ShippingAddressFirstName.SetText(json.ShippingAddressFirstName);
			ShippingAddressLastName.SetText(json.ShippingAddressLastName);
			ShippingAddressStreet1.SetText(json.ShippingAddressStreet1);
			ShippingAddressStreet2.SetText(json.ShippingAddressStreet2);
			ShippingAddressCity.SetText(json.ShippingAddressCity);
			ShippingAddressRegion.SetText(json.ShippingAddressRegion);
			ShippingAddressZip.SetText(json.ShippingAddressZip);
			ShippingAddressCountry.SetText(json.ShippingAddressCountry);
			ShippingAddressEmail.SetText(json.ShippingAddressEmail);
			ShippingAddressPhone.SetText(json.ShippingAddressPhone);
			ShippingAddressCompany.SetText(json.ShippingAddressCompany);
			$('#shipping-address-popup-shipping-address-id').val(json.ShippingAddressID);

			ShowPopup('#shipping-address-popup');
		}
	});
}

function SaveShippingAddress() {

	var salesOrderID = $('#shipping-address-popup-sales-order').val();

	var shippingAddressID = $('#shipping-address-popup-shipping-address-id').val();

	$.ajax({
		cache: false,
		url: '/Orders/Orders/SaveShippingAddress',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			shippingAddressID: shippingAddressID,
			shippingAddressFirstName: ShippingAddressFirstName.GetValue(),
			shippingAddressLastName: ShippingAddressLastName.GetValue(),
			shippingAddressStreet1: ShippingAddressStreet1.GetValue(),
			shippingAddressStreet2: ShippingAddressStreet2.GetValue(),
			shippingAddressCity: ShippingAddressCity.GetValue(),
			shippingAddressRegion: ShippingAddressRegion.GetValue(),
			shippingAddressZip: ShippingAddressZip.GetValue(),
			shippingAddressCountry: ShippingAddressCountry.GetValue(),
			shippingAddressEmail: ShippingAddressEmail.GetValue(),
			shippingAddressPhone: ShippingAddressPhone.GetValue(),
			shippingAddressCompany: ShippingAddressCompany.GetValue()
		},
		success: function (json) {
			if (json) {
				ReloadShippingAddress(salesOrderID);
				HidePopup('#shipping-address-popup');
			}
			else {
				HidePopup('#shipping-address-popup');
				ShowErrorMessage('Failed to save shipping address fields');
			}
		}
	});
}

function ReloadShippingAddress(salesOrderID) {

	$.ajax({
		cache: false,
		url: '/Orders/Orders/GetShippingAddress',
		data: {
			salesOrderID: salesOrderID
		},
		success: function (json) {
			if (!json) return;

			var shippingAddressMemo = '#sales-order' + salesOrderID + '-shipping-address-memo';

			$(shippingAddressMemo).val(json.ShippingAddressFormatted);
		}
	});
}

// Fraud Check Popup
function ShowFraudCheck(salesOrderID) {

	var width = 280;
	$('#fraud-check-popup').width(width);
	$('#fraud-check-popup').css('left', (window.innerWidth / 2) - (width / 2));
	$('#fraud-check-popup').css('top', '20%');
	$('#fraud-check-buttons').width(width - 50);
	$('#fraud-check-popup-sales-order').val(salesOrderID);

	$.ajax({
		cache: false,
		url: '/Orders/Orders/GetFraudCheck',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID
		},
		success: function (json) {
			if (!json) return;

			PaymentMethod.SetText(json.PaymentMethod);
			FraudScore.SetText(json.FraudScore);
			CustomerIsNew.SetChecked(json.CustomerIsNew);
			ShippingAddressChecked.SetChecked(json.ShippingAddressChecked);
			CustomerIPAddressChecked.SetChecked(json.CustomerIPAddressChecked);
			FraudChecked.SetChecked(json.FraudChecked);
			$('#fraud-check-popup-ip-address').val(json.IPAddress);

			ShowPopup('#fraud-check-popup');
		}
	});
}

function GetFraudCheckValue(salesOrderID) {

	var fraudCheck = '#sales-order' + salesOrderID + '-fraud-check-checkbox';

	return $(fraudCheck).is(":checked"); //Returns true/false
}

function SetFraudCheckFields(salesOrderID, fraudScore, customerIsNew, shippingAddressChecked, customerIPAddressChecked, fraudChecked) {
	var prefixID = '#sales-order' + salesOrderID;

	// Payment Method
	$.ajax({
		cache: false,
		url: '/Orders/Orders/GetBillingAddress',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID
		},
		success: function (json) {
			if (!json) return;

			var billingAddressMemo = prefixID + '-billing-address-memo';

			$(billingAddressMemo).val(json);
		}
	});

	// Fraud Score
	var fraudScoreField = prefixID + '-fraud-score-box';
	$(fraudScoreField).val(fraudScore);

	// New Customer
	var customerIsNewField = prefixID + '-customer-is-new-checkbox';

	if (customerIsNew) {
		$(customerIsNewField).attr('checked', 'checked');
	}
	else {
		$(customerIsNewField).removeAttr('checked');
	}

	// Shipping Address Checked
	var shippingAddressCheckedField = prefixID + '-shipping-address-checkbox';

	if (shippingAddressChecked) {
		$(shippingAddressCheckedField).attr('checked', 'checked');
	}
	else {
		$(shippingAddressCheckedField).removeAttr('checked');
	}

	// Customer IP Address Checked
	var customerIPAddressCheckedField = prefixID + '-customer-ip-address-checkbox';

	if (customerIPAddressChecked) {
		$(customerIPAddressCheckedField).attr('checked', 'checked');
	}
	else {
		$(customerIPAddressCheckedField).removeAttr('checked');
	}

	// Fraud Check Completed
	var fraudCheckedField = prefixID + '-fraud-check-checkbox';

	if (fraudChecked) {
		$(fraudCheckedField).attr('checked', 'checked');
	}
	else {
		$(fraudCheckedField).removeAttr('checked');
	}
}

function SaveFraudCheckPopup() {

	var salesOrderID = $('#fraud-check-popup-sales-order').val();

	var paymentMethod = PaymentMethod.GetValue();
	var fraudScore = FraudScore.GetValue();
	var customerIsNew = CustomerIsNew.GetChecked();
	var shippingAddressChecked = ShippingAddressChecked.GetChecked();
	var customerIPAddressChecked = CustomerIPAddressChecked.GetChecked();
	var fraudChecked = FraudChecked.GetChecked();

	$.ajax({
		cache: false,
		url: '/Orders/Orders/SaveFraudCheck',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			paymentMethod: paymentMethod,
			fraudScore: fraudScore,
			customerIsNew: customerIsNew,
			shippingAddressChecked: shippingAddressChecked,
			customerIPAddressChecked: customerIPAddressChecked,
			fraudChecked: fraudChecked
		},
		success: function (json) {
			if (json) {

				SetFraudCheckFields(salesOrderID, fraudScore, customerIsNew, shippingAddressChecked, customerIPAddressChecked, fraudChecked);

				ValidateAllPushToSupplierButtons(salesOrderID);

				HidePopup('#fraud-check-popup');
			}
			else {
				HidePopup('#fraud-check-popup');

				ShowErrorMessage('Failed to save fraud check fields');
			}
		}
	});
}

// New Supplier Drop Down
function GetNewSupplier(salesOrderID, salesOrderLineID) {
	var newSupplierDropDownID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-new-supplier-dropdown';

	return $(newSupplierDropDownID).val();
}

function UpdateNewSupplier(salesOrderID, salesOrderLineID) {

	UpdateNewSupplierReadOnlyFields(salesOrderID, salesOrderLineID);

	UpdateNewSupplierPricing(salesOrderID, salesOrderLineID);

	ValidatePushToSupplierButton(salesOrderID, salesOrderLineID);
}

function UpdateNewSupplierReadOnlyFields(salesOrderID, salesOrderLineID) {

	var newSupplierDropDownID = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-new-supplier-dropdown';

	var newSupplierDropDown = document.getElementById(newSupplierDropDownID); // don't use jquery here

	var nonEDI = newSupplierDropDown.options[newSupplierDropDown.selectedIndex].getAttribute("data-non-edi"); // because of this part

	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;

	var shippingCarrierID = prefixID + '-shipping-carrier';
	var shippingOrderNoID = prefixID + '-shipping-order-no';
	var deliveryCourierID = prefixID + '-delivery-courier';
	var trackingNumberID = prefixID + '-tracking-number';
	var nonEDISupplierBox = prefixID + '-non-edi-checkbox';

	if (nonEDI === 'False') {
		$(nonEDISupplierBox).removeAttr('checked');
		$(shippingCarrierID).attr('disabled', 'disabled');
		$(shippingOrderNoID).attr('disabled', 'disabled');
		$(deliveryCourierID).attr('disabled', 'disabled');
		$(trackingNumberID).attr('disabled', 'disabled');
	}
	else {
		$(nonEDISupplierBox).attr('checked', 'checked');
		$(shippingCarrierID).removeAttr('disabled');
		$(shippingOrderNoID).removeAttr('disabled');
		$(deliveryCourierID).removeAttr('disabled');
		$(trackingNumberID).removeAttr('disabled');
	}
}

function ClearNewSupplierPricing(salesOrderID, salesOrderLineID) {
	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;
	var newCostField = prefixID + '-new-cost';
	var newStockField = prefixID + '-new-stock';
	var weightField = prefixID + '-weight';

	$(newCostField).val(0.00);
	$(newStockField).val(0.00);
	$(weightField).val(0.00);

	UpdateSelectedQuantity(salesOrderID, salesOrderLineID);
}

function UpdateNewSupplierPricing(salesOrderID, salesOrderLineID) {
	var feedKey = GetNewSupplier(salesOrderID, salesOrderLineID);

	if (feedKey === 'None') {
		ClearNewSupplierPricing(salesOrderID, salesOrderLineID);
		return;
	}
	
	$.ajax({
		cache: false,
		url: '/Orders/Orders/GetSelectedSupplierLine',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			salesOrderLineID: salesOrderLineID,
			feedKey: feedKey
		},
		success: function (json) {
			if (json) {

				var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;
				var newCostField = prefixID + '-new-cost';
				var newStockField = prefixID + '-new-stock';
				var weightField = prefixID + '-weight';

				$(newCostField).val(json.ResellerBuyEx);
				$(newStockField).val(json.Stock);
				$(weightField).val(json.WeightGrams);

				UpdateSelectedQuantity(salesOrderID, salesOrderLineID);
			}
			else {
				ShowErrorMessage('Failed to load selected supplier details');
			}
		}
	});
}

// Selected Quantity Field
function GetSelectedQuantity(salesOrderID, salesOrderLineID) {
	var selectedQuantityID = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-selected-qty';
	return document.getElementById(selectedQuantityID).valueAsNumber;
}

function UpdateSelectedQuantity(salesOrderID, salesOrderLineID) {
	
	// Recalculate order line values
	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;

	var newCostField = prefixID + '-new-cost';
	var newCostValue = parseFloat($(newCostField).val());

	var selectedQuantityField = prefixID + '-selected-qty';
	var selectedQuantityValue = parseInt($(selectedQuantityField).val());

	var newSubtotalValue = newCostValue * selectedQuantityValue;
	var newSubtotalField = prefixID + '-subtotal';

	$(newSubtotalField).val(newSubtotalValue);

	var discountField = prefixID + '-discount';
	var discountValue = parseFloat($(discountField).val());

	var shippingField = prefixID + '-shipping';
	var shippingValue = parseFloat($(shippingField).val());

	var totalValue = newSubtotalValue - discountValue + shippingValue;
	var totalField = prefixID + '-total';

	$(totalField).val(totalValue);

	// Validate push to supplier button
	ValidatePushToSupplierButton(salesOrderID, salesOrderLineID);
}

// Push To Supplier Popup
function ValidatePushToSupplierButton(salesOrderID, salesOrderLineID) {
	var pushToSupplierButton = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-push-to-supplier-button';
	var orderLineMultiSelect = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-select-order-line';

	// Check Fraud Checked checkbox
	var fraudCheck = GetFraudCheckValue(salesOrderID);
	if (fraudCheck === false) {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect)
		return;
	};

	// Check Selected Supplier dropdown
	var newSupplier = GetNewSupplier(salesOrderID, salesOrderLineID);
	if (newSupplier === 'None') {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect)
		return;
	}

	// Check Selected Quantity
	var selectedQuantity = GetSelectedQuantity(salesOrderID, salesOrderLineID);
	if (isNaN(selectedQuantity) || selectedQuantity < 1) {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect)
		return;
	}

	// Enable if passed all validation
	$(pushToSupplierButton).removeAttr('disabled');
	$(pushToSupplierButton).removeClass('detail-button-tall-push-disabled');
	$(pushToSupplierButton).addClass('detail-button-tall-push');
	$(orderLineMultiSelect).removeAttr('disabled');
}

function DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect) {
	$(pushToSupplierButton).attr('disabled', 'disabled');
	$(pushToSupplierButton).removeClass('detail-button-tall-push');
	$(pushToSupplierButton).addClass('detail-button-tall-push-disabled');
	$(orderLineMultiSelect).prop('checked', false);
	$(orderLineMultiSelect).attr('disabled', 'disabled');
	return;
}

function ValidateAllPushToSupplierButtons(salesOrderID) {
	// Set up query constants
	var salesOrderIDFilter = 'sales-order' + salesOrderID;
	var supplierButtonIDFilter = '-push-to-supplier-button';

	// Find push to supplier buttons
	var pushToSupplierLines = $("input[id ^= " + salesOrderIDFilter + "][id $= " + supplierButtonIDFilter + "]"); //Looking for all push to supplier buttons for the order

	pushToSupplierLines.each(function () {

		var supplierButtonID = this.id;

		var idComponents = supplierButtonID.split('-');

		ValidatePushToSupplierButton(salesOrderID, idComponents[2].substring(9));
	});
}

function GetAllSelectedOrderLines(salesOrderID) {
	// Set up query constants
	var salesOrderIDFilter = 'sales-order' + salesOrderID;
	var orderLineCheckboxIDFilter = '-select-order-line';

	// Find order lines
	var orderLines = $("input[id ^= " + salesOrderIDFilter + "][id $= " + orderLineCheckboxIDFilter + "]"); // Looking for all order line multi select checkboxes

	var selectedOrderLines = "";

	orderLines.each(function () {

		if (this.checked === true) {
			var orderLine = this.id;

			var idComponents = orderLine.split('-');

			selectedOrderLines = selectedOrderLines + (idComponents[2].substring(9)) + ',';
		}
	});

	return selectedOrderLines;
}

function GetAllSelectedOrderLinesArray(salesOrderID, salesOrderLineID) {
	// Set up query constants
	var salesOrderIDFilter = 'sales-order' + salesOrderID;
	var orderLineCheckboxIDFilter = '-select-order-line';

	// Encheck order line checkbox
	var pushToSupplierItemLine = '#' + salesOrderIDFilter + '-orderline' + salesOrderLineID + orderLineCheckboxIDFilter;
	$(pushToSupplierItemLine).prop('checked', true);

	// Find order lines
	var orderLines = $("input[id ^= " + salesOrderIDFilter + "][id $= " + orderLineCheckboxIDFilter + "]"); // Looking for all order line multi select checkboxes

	var selectedOrderLines = [];

	orderLines.each(function () {

		if (this.checked === true) {
			var orderLine = this.id;

			var idComponents = orderLine.split('-');

			selectedOrderLines.push(idComponents[2].substring(9));
		}
	});

	return selectedOrderLines;
}

function UncheckOrderLines(salesOrderID) {
	// Set up query constants
	var salesOrderIDFilter = 'sales-order' + salesOrderID;
	var orderLineCheckboxIDFilter = '-select-order-line';

	// Find order lines
	var orderLines = $("input[id ^= " + salesOrderIDFilter + "][id $= " + orderLineCheckboxIDFilter + "]"); // Looking for all order line multi select checkboxes

	orderLines.each(function () {

		if (this.checked === true) {

			var orderLine = this.id;

			$(orderLine).prop('checked', false);
		}
	});
}

function ShowPushToSupplier(salesOrderID, salesOrderLineID) {
	var width = 700;
	$('#push-to-supplier-popup').width(width);
	$('#push-to-supplier-popup').css('left', (window.innerWidth / 2) - (width / 2));
	$('#push-to-supplier-popup').css('top', '40%');
	$('#push-to-supplier-buttons').width(width - 160);
	$('#push-to-supplier-popup-sales-order').val(salesOrderID);
	$('#push-to-supplier-popup-sales-order-line').val(salesOrderLineID);

	var selectedOrderLineIDs = GetAllSelectedOrderLinesArray(salesOrderID, salesOrderLineID);
	var message = "";

	selectedOrderLineIDs.forEach((orderLineID) => {
		var preFixID = '#sales-order' + salesOrderID + '-orderline' + orderLineID;

		var newSupplierDropDown = preFixID + '-new-supplier-dropdown';
		var skuField = preFixID + '-sku';
		var statusDropDown = preFixID + '-status-dropdown';

		var newSupplier = $(newSupplierDropDown).text();
		var sku = $(skuField).val();
		var supplierStatus = $(statusDropDown).val();

		if (supplierStatus === '2') { // Sent to supplier. Note this id will change depending on the purchase status table and what the ids are
			message = message + 'Item ' + sku + ' has already been sent to ' + newSupplier + '. This will be sent again. <br>';
		}
		else {
			message = message + 'Item ' + sku + ' will be sent to ' + newSupplier + '. <br>';
		}
	});

	$('#push-to-supplier-message').html(message);
	ShowPopup('#push-to-supplier-popup');
}

function PushToSupplier() {
	var salesOrderID = $('#push-to-supplier-popup-sales-order').val();
	var salesOrderLineID = $('#push-to-supplier-popup-sales-order-line').val();

	if (salesOrderID === null || salesOrderID === '') return;
	if (salesOrderLineID === null || salesOrderLineID === '') return;

	var statusDropDown = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-status-dropdown';

	var selectedOrderLineIDs = GetAllSelectedOrderLines(salesOrderID);

	$.ajax({
		cache: false,
		url: '/Orders/Orders/PushToSupplier',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			salesOrderLineIDs: selectedOrderLineIDs
		},
		success: function (json) {
			if (json) {
				UncheckOrderLines(salesOrderID);
				$(statusDropDown).val('2'); // Sent to supplier. Note this id will change depending on the purchase status table and what the ids are
				HidePopup('#push-to-supplier-popup');
			}
			else {
				HidePopup('#push-to-supplier-popup');
				ShowErrorMessage('Failed to send selected lines to chosen supplier, please contact support');
			}
		}
	});
}

// Save Order Line
function SaveOrderLine(salesOrderID, salesOrderLineID) {
	ShowLoadingPanel("Saving..");

	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;

	var weightField = prefixID + '-weight';
	var subtotalField = prefixID + '-subtotal';
	var discountField = prefixID + '-discount';
	var shippingField = prefixID + '-shipping';
	var totalField = prefixID + '-total';
	var selectedSupplierID = GetNewSupplier(salesOrderID, salesOrderLineID);

	$.ajax({
		cache: false,
		url: '/Orders/Orders/SaveOrderLine',
		data: {
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			salesOrderLineID: salesOrderLineID,
			weightGrams: $(weightField).val(),
			subtotalAmount: $(subtotalField).val(),
			discountAmount: $(discountField).val(),
			shippingAmount: $(shippingField).val(),
			totalAmount: $(totalField).val(),
			supplierID: selectedSupplierID
		},
		success: function (json) {
			HideLoadingPanel();

			if (!json) {
				ShowErrorMessage('Failed to save order line changes');
			}
		}
	});
}