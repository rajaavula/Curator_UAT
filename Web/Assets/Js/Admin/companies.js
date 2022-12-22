$(function () {

	$('.reload').on('click', function (e) {
		e.preventDefault();
		ShowLoadingPanel();
		window.location = $(e.currentTarget).attr('href');
	});

});

function Back()
{
	window.location = '/Admin/Companies/List';
}

function Reload(url)
{
	ShowLoadingPanel();
	window.location = url;
}

function Save()
{
	if (ASPxClientEdit.ValidateEditorsInContainer() == false) return;
	Submit('Saving...');
}

function SaveNewCompany() {
	if (ASPxClientEdit.ValidateGroup() == false) return;
	ShowLoadingPanel();

	$.ajax({
		cache: false,
		url: '/Admin/Companies/ListNewCreate',
		data:
		{
			PageID: GetPageID(),
			Name: NewCompany_CompanyName.GetText(),
			OwnerName: NewCompany_OwnerName.GetText(),
			OwnerLogin: NewCompany_OwnerLogin.GetText(),
			OwnerEmail: NewCompany_OwnerEmail.GetText(),
			OwnerPassword: NewCompany_OwnerPassword.GetText()
		},
		success: function (error) {
			HideLoadingPanel();

			if (error && error.length > 0) {
				alert(error);
				return;
			}

			ppNewCompany.Hide();
			GrdMain.PerformCallback();
		}
	});
}

function Get()
{
	var id = GetRowID(GrdMain);
	if (!id || id.length < 1) { if (btnEdit.GetEnabled() == true) btnEdit.SetEnabled(false); return; }
	if (btnEdit.GetEnabled() == false) btnEdit.SetEnabled(true);

	$.ajax({
		cache: false,
		url: '/Admin/Companies/ListPreview',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
			if (!json) return;
			$("#txtName").text(json.Name);
			$("#txtLive").val(json.Live ? "Yes" : "No");
			$("#txtTheme").val(json.Theme);
			$('#memNotes').val(json.Notes);
		}
	});
}

function LogoUploadComplete()
{
	$('#logo-preview').attr('src', '/Admin/Companies/CompanyLogoPreview/' + GetPageID() + '?' + GetUnique());
}

function Edit()
{
	if (btnEdit.GetEnabled() == false) return;
	window.location = '/Admin/Companies/Edit/' + GetRowID();
}

function New() {
	ppNewCompany.PerformCallback();
}

function ppNewCompany_BeginCallback() {
	ShowLoadingPanel();
}

function ppNewCompany_EndCallback() {
	HideLoadingPanel();
	ppNewCompany.Show();
}

function HidePopUp() {
	ppNewCompany.Hide();
}

