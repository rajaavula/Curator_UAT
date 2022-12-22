$(function () {

	window.setInterval(RefreshSupplierUpdates, 300000);
});

function RefreshSupplierUpdates() {

	GrdMain.PerformCallback();
}

