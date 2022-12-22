function RefreshGridWithArgs(grid, refresh) {

    var args = refresh;

    grid.PerformCallback(args);
}

function Get() {
	var id = GetRowID(GrdMain);
	if (!id) return;

	$.ajax({
		cache: false,
		url: '/Products/Feeds/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json) {
			if (!json) return;
			$('#FeedID').val(json.FeedKey);
			FeedName.SetText(json.FeedName);
			IncludeZeroStock.SetChecked(json.IncludeZeroStock);
			PushToSupplierEmail.SetText(json.PushToSupplierEmail);
		}
	});
}

function SaveSupplier() {
	var id = $('#FeedID').val();
	if (id === "" || id === 0) return;

	$.ajax({
		cache: false,
		async: false,
		url: '/Products/Feeds/Update',
		data: {
			FeedKey: id,
			IncludeZeroStock: IncludeZeroStock.GetChecked(),
			PushToSupplierEmail: PushToSupplierEmail.GetText()
		},
		success: function (msg) {
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback(true);
		}
	});
}