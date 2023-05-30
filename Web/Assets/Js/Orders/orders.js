var selectedStores, statusNotSent = '1', statusSupplierSelected = '2', statusPurchased = '3', statusPurchaseFailed = '4', statusShipped = '5', statusCancelled = '6', statusBackOrdered = '7'
var selectedOrderIDs = null, selectedOrderIDsUpdate = false, selectedOrderIDsUpdateQueue = null;
const CurrencyFormat = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2, useGrouping: false });

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

function GrdOrders_SelectionChanged(s, e) {
	if (selectedOrderIDsUpdate)
	{
		if (selectedOrderIDsUpdateQueue === null)
		{
			selectedOrderIDsUpdateQueue = setInterval(function ()
			{
				if (!selectedOrderIDsUpdateQueue)
				{
					selectedOrderIDsUpdateQueue = true;
					clearInterval(selectedOrderIDsUpdateQueue);
					selectedOrderIDsUpdateQueue = null;
					s.GetSelectedFieldValues("SalesOrderID", GrdOrders_GetSelectedFieldValuesCallback);
				}
			}, 10);
		};
	}
	else
	{
		selectedOrderIDsUpdate = true;
		s.GetSelectedFieldValues("SalesOrderID", GrdOrders_GetSelectedFieldValuesCallback);
	}
}

function GrdOrders_GetSelectedFieldValuesCallback(values) {
	var ids = values.join(',');

	selectedOrderIDs = ids;

	var enabled = selectedOrderIDs && selectedOrderIDs.length > 0;

	btnQueueNetSuiteUpdate.SetEnabled(enabled);

	selectedOrderIDsUpdate = false;
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

function ExpandOrderLine(sender) {
	var container = $(sender).closest('.detail-table');
	var lines = $(container).find('.order-line');
	var link = $(container).find('.orderline-expand');

    if ($(lines).hasClass('hide')) {
		$(lines).removeClass('hide');
		$(link).text('-');
    }
	else
    {
		$(lines).addClass('hide');
		$(link).text('+');
    }
}

// Sales Orders
function QueueNetSuiteUpdate() {
	ShowLoadingPanel();

	$.ajax({
		cache: false,
		url: '/Orders/Orders/QueueNetSuiteUpdate',
		data:
		{
			PageID: GetPageID(),
			SalesOrderIDs: selectedOrderIDs
		},
		complete: function ()
		{
			HideLoadingPanel();
    },
		success: function (error)
		{
			if (error && error.length > 0)
			{
				ShowErrorMessage(error);
				return;
			}

			alert('The selected orders have been queued to be sent to NetSuite.');
			RefreshGridWithArgs(GrdOrders);
		}
	});
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
	if (ASPxClientEdit.ValidateGroup('SHIPPING_ADDRESS_EDIT') == false) return;

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

function GetNewSupplierEDI(salesOrderID, salesOrderLineID) {
	var newSupplierDropDownID = 'sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-new-supplier-dropdown';

	var newSupplierDropDown = document.getElementById(newSupplierDropDownID); // don't use jquery here

	var isEDI = newSupplierDropDown.options[newSupplierDropDown.selectedIndex].getAttribute("data-is-edi"); // because of this part

	return isEDI;
}

function UpdateNewSupplier(salesOrderID, salesOrderLineID) {
	UpdateNewSupplierReadOnlyFields(salesOrderID, salesOrderLineID);

	UpdateNewSupplierPricing(salesOrderID, salesOrderLineID);

	ValidatePushToSupplierButton(salesOrderID, salesOrderLineID);
}

function UpdateNewSupplierReadOnlyFields(salesOrderID, salesOrderLineID) {
	var isEDI = GetNewSupplierEDI(salesOrderID, salesOrderLineID);

	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;

	var pushToSupplierButton = prefixID + '-push-to-supplier-button';
	var supplierID = prefixID + '-new-supplier-dropdown';
	var deliveryCourierID = prefixID + '-delivery-courier';
	var trackingNumberID = prefixID + '-tracking-number';
	var isEDISupplierBox = prefixID + '-is-edi-checkbox';
	var statusDropDown = prefixID + '-status-dropdown';

	var pushed = $(pushToSupplierButton).hasClass('pushed');

	if (isEDI === 'True') {
		$(isEDISupplierBox).attr('checked', 'checked');
		$(deliveryCourierID).attr('disabled', 'disabled');
		$(trackingNumberID).attr('disabled', 'disabled');
		$(statusDropDown).attr('disabled', 'disabled');
	}
	else {
		$(isEDISupplierBox).removeAttr('checked');
		$(statusDropDown).removeAttr('disabled');

		if (pushed) {
			$(deliveryCourierID).attr('disabled', 'disabled');
			$(trackingNumberID).attr('disabled', 'disabled');
		} else {
			$(deliveryCourierID).removeAttr('disabled');
			$(trackingNumberID).removeAttr('disabled');
    }
	}

	if (pushed) {
		$(supplierID).attr('disabled', 'disabled');
	} else {
		$(supplierID).removeAttr('disabled');
  }
}

function ClearNewSupplierPricing(salesOrderID, salesOrderLineID) {
	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;
	var newCostField = prefixID + '-new-cost';
	var newStockField = prefixID + '-new-stock';

	$(newCostField).val(null);
	$(newStockField).val(null);

	UpdateSelectedQuantity(salesOrderID, salesOrderLineID);
}

function UpdateNewSupplierPricing(salesOrderID, salesOrderLineID) {
	var feedKey = GetNewSupplier(salesOrderID, salesOrderLineID);

	if (feedKey === '') {
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

				$(newCostField).val(CurrencyFormat.format(json.ResellerBuyEx));
				$(newStockField).val(json.Stock);

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
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect);
		return;
	};

	// Check Selected Supplier dropdown
	var newSupplier = GetNewSupplier(salesOrderID, salesOrderLineID);
	if (newSupplier === 'None') {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect);
		return;
	}

	// Check Selected Quantity
	var selectedQuantity = GetSelectedQuantity(salesOrderID, salesOrderLineID);
	if (isNaN(selectedQuantity) || selectedQuantity < 1) {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect);
		return;
	}

	// Check Pushed
	var pushed = $(pushToSupplierButton).hasClass('pushed');
	if (pushed) {
		DisablePushToSupplierButton(pushToSupplierButton, orderLineMultiSelect);
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

function ValidateAllPushToSupplierButtons(orderID) {
	// Find push to supplier buttons
	var lines = $('input.order-' + orderID + '-push-button');  // Looking for all order line multi select checkboxes

	lines.each(function ()
	{
		ValidatePushToSupplierButton(orderID, $(this).data('id'));
	});
}

function GetAllSelectedOrderLines(orderID) {
	// Find order lines
	var lines = $('input.order-' + orderID + '-line-select:checked');  // Looking for all order line multi select checkboxes

	var selected = [];

	lines.each(function ()
	{
		selected.push($(this).data('id'))
	});

	return selected;
}

function UncheckOrderLines(orderID) {
	// Find order lines
	var lines = $('input.order-' + orderID + '-line-select:checked'); // Looking for all order line multi select checkboxes

	lines.each(() => $(this).prop('checked', false));
}

function ShowPushToSupplier(salesOrderID, salesOrderLineID) {
	var width = 700;
	$('#push-to-supplier-popup').width(width);
	$('#push-to-supplier-popup').css('left', (window.innerWidth / 2) - (width / 2));
	$('#push-to-supplier-popup').css('top', '40%');
	$('#push-to-supplier-buttons').width(width - 160);
	$('#push-to-supplier-popup-sales-order').val(salesOrderID);
	$('#push-to-supplier-popup-sales-order-line').val(salesOrderLineID);

	// Check order line checkbox
	var pushToSupplierItemLine = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID + '-select-order-line';
	$(pushToSupplierItemLine).prop('checked', true);

	var selectedOrderLineIDs = GetAllSelectedOrderLines(salesOrderID);
	var message = "";

	selectedOrderLineIDs.forEach((orderLineID) => {
		var preFixID = '#sales-order' + salesOrderID + '-orderline' + orderLineID;

		var newSupplierDropDown = preFixID + '-new-supplier-dropdown';
		var skuField = preFixID + '-sku';
		var statusDropDown = preFixID + '-status-dropdown';

		var newSupplier = $(newSupplierDropDown + ' option:selected').text();
		var sku = $(skuField).val();
		var supplierStatus = $(statusDropDown).val();

		if (supplierStatus === statusSupplierSelected) {
			message = message + 'Item ' + sku + ' has already been sent to ' + newSupplier + '. This will be sent again. <br>';
		}
		else {
			message = message + 'Item ' + sku + ' will be sent to ' + newSupplier + '. <br>';
		}
	});

	$('#push-to-supplier-message').html(message);
	ShowPopup('#push-to-supplier-popup');
}

async function PushToSupplier() {
	// Get selected supplier lines
	var salesOrderID = $('#push-to-supplier-popup-sales-order').val();
	var salesOrderLineID = $('#push-to-supplier-popup-sales-order-line').val();

	if (salesOrderID === null || salesOrderID === '') return;
	if (salesOrderLineID === null || salesOrderLineID === '') return;

	// Save selected order lines
	var selectedOrderLineIDs = GetAllSelectedOrderLines(salesOrderID);

	ShowLoadingPanel();

	await SaveAllOrderLines(salesOrderID, selectedOrderLineIDs);

	// Push to supplier email
	$.ajax({
		cache: false,
		url: '/Orders/Orders/PushToSupplier',
		data:
		{
			pageID: GetPageID(),
			salesOrderID: salesOrderID,
			salesOrderLineIDs: selectedOrderLineIDs.join(',')
		},
		complete: function ()
		{
			HideLoadingPanel();
		},
		success: function (error)
		{
			if (error && error.length > 0)
			{
				HidePopup('#push-to-supplier-popup');
				ShowErrorMessage(error);
				return;
			}

			UncheckOrderLines(salesOrderID);
			HidePopup('#push-to-supplier-popup');

			GrdOrders.PerformCallback();
		}
	});
}

// Saving
async function SaveOrderLine(salesOrderID, salesOrderLineID, disableLoad) {
	if (!disableLoad) ShowLoadingPanel("Saving..");

	var prefixID = '#sales-order' + salesOrderID + '-orderline' + salesOrderLineID;

	var supplierFreightCode = prefixID + '-supplier-freight-code';
	var supplierFreightCost = prefixID + '-supplier-freight-cost';
	var statusDropDownID = prefixID + '-status-dropdown';
	var deliveryCourier = prefixID + '-delivery-courier';
	var trackingNumber = prefixID + '-tracking-number';
	var selectedSupplierID = GetNewSupplier(salesOrderID, salesOrderLineID);

	try {
		const error = await $.ajax({
			cache: false,
			url: '/Orders/Orders/SaveOrderLine',
			data:
			{
				pageID: GetPageID(),
				salesOrderID: salesOrderID,
				salesOrderLineID: salesOrderLineID,
				supplierFreightCode: $(supplierFreightCode).val(),
				supplierFreightCost: $(supplierFreightCost).val(),
				supplierID: selectedSupplierID,
				carrierName: $(deliveryCourier).val(),
				trackingNumber: $(trackingNumber).val(),
				salesOrderLineStatusID: $(statusDropDownID).val()
			}
		});

		if (error && error.length > 0) {
			ShowErrorMessage(error);
			return;
		}
	} catch (error) {
		console.error(error);
	} finally {
		if (!disableLoad) HideLoadingPanel();
	}
}

async function SaveAllOrderLines(salesOrderID, selectedOrderLineIDArray) {
	for (let i = 0; i < selectedOrderLineIDArray.length; i++) {
		await SaveOrderLine(salesOrderID, selectedOrderLineIDArray[i], true);
	}
}