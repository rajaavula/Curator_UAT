$(function () {

	$('.reload').on('click', function (e) {
		e.preventDefault();
		ShowLoadingPanel();
		window.location = $(e.currentTarget).attr('href');
	});

});

function ActiveTabChanged(s, e) {

	window.onresize();
}

function Back() {
	window.location = '/Admin/Customers/List';
}

function Reload(url) {
	ShowLoadingPanel();
	window.location = url;
}

function Save() {
	if (ASPxClientEdit.ValidateEditorsInContainer() == false) return;
	Submit('Saving...');
}

function Edit() {
	if (btnEdit.GetEnabled() == false) return;
	var url = '/Admin/Customers/Edit/' + GetRowID();
	window.open(url, '_blank');
}

function New() {
	var url = '/Admin/Customers/New';
	window.open(url, '_blank');
}

function Delete() {
	if (btnDelete.GetEnabled() == false) return;
	if (confirm('Are you sure you want to delete this customer?') == false) return;
	$.ajax({
		cache: false,
		url: '/Admin/Customers/Delete',
		data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
		success: function (msg) {
			if (msg) { alert(msg); return; }
			RefreshGrid(GrdMain, null);
		}
	});
}

function CancelUpload() {
	uplFileUpload.ClearText();
	ShowUploadPopup(false);
}

function UploadFile() {
	uplFileUpload.Upload();
}

function FileUploadStart(s, e)
{
	ShowLoadingPanel();
}

function FileUploadComplete() {

	$.ajax({
		cache: false,
		url: '/Admin/Customers/FileUploadComplete',
		data: { PageID: GetPageID() },
		success: function (json)
		{
			HideLoadingPanel();
			if (!json) return;

			if (json.OK)
			{
                ShowUploadPopup(false);
				RefreshGrid(GrdFiles);
				RefreshGrid(GrdHistory);

				return;
			}

			var errMsg = '';
			for (var i = 0; i < json.Errors.length; i++)
			{
				errMsg += (json.Errors[i] + '\n');
			}

			if (!errMsg || errMsg.length < 1) return;

			alert(errMsg);
		}
	});	
}

function ShowUploadPopup(show) {

	if (show) {
		FileUploadPopup.Show();
	}
	else {
		FileUploadPopup.Hide();
	}
}

function DeleteDocument(companyID, regionID, id, userID) {

	if (confirm('Are you sure you want to delete this file?') == false) return;
	$.ajax({
		cache: false,
		url: '/Home/Home/DeleteDocument',
		data: {
			companyID: companyID,
			regionID: regionID,
			id: id,
			userID: userID
		},
		success: function (msg) {
			if (msg) { alert(msg); return; }
			RefreshGrid(GrdFiles);
			RefreshGrid(GrdHistory);
		}
	});

}
