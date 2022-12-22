$(function () {

	var oGrid_EndCallback = Grid_EndCallback;

	Grid_EndCallback = function() {
		oGrid_EndCallback();
		Get();
	};

});

function New()
{
	Clear();
	Name.SetIsValid(false);
}

function Delete()
{
	if (confirm('Are you sure you want to delete this region?') == false) return;
	$.ajax({
		cache: false,
		url: '/Admin/CompanyRegions/Delete',
		data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
		success: function (msg)
		{
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
			Clear();
		}
	});
}

function Get()
{
	var id = GetRowID(GrdMain);
	if (!id || id.length < 1) return;
	
	$.ajax({
		cache: false,
		url: '/Admin/CompanyRegions/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
			$('#RegionID').val(json.RegionID);
			Name.SetText(json.Name);
			CopyRegionalEmailTo.SetText(json.CopyRegionalEmailTo);
			SalesSupportEmailAddress.SetText(json.SalesSupportEmailAddress);
			PurchasingDeptEmailAddress.SetText(json.PurchasingDeptEmailAddress);
			EmailServer.SetText(json.EmailServer);
			EmailUsername.SetText(json.EmailUsername);
			EmailPassword.SetText(json.EmailPassword);
			EmailFromEmail.SetText(json.EmailFromEmail);
			EmailFromName.SetText(json.EmailFromName);
			Notes.SetText(json.Notes);

			Name.SetIsValid(true);
		}
	});
}

function Clear()
{
	$('#RegionID').val('');
	Name.SetText('');
	CopyRegionalEmailTo.SetText('');
	SalesSupportEmailAddress.SetText('');
	PurchasingDeptEmailAddress.SetText('');
	Notes.SetText('');
	EmailServer.SetText();
	EmailUsername.SetText();
	EmailPassword.SetText();
	EmailFromEmail.SetText();
	EmailFromName.SetText();
}

function Save()
{
	if (!Name.GetIsValid()) return;
	ShowLoadingPanel("Saving..");

	$.ajax({
		cache: false,
		url: '/Admin/CompanyRegions/Save',
		data: {
			pageID: GetPageID(),
			regionID: $('#RegionID').val(),
			name: Name.GetText(),
			notes: Notes.GetText(),
			copyRegionalEmailTo: CopyRegionalEmailTo.GetText(),
			salesSupportEmailAddress: SalesSupportEmailAddress.GetText(),
			PurchasingDeptEmailAddress: PurchasingDeptEmailAddress.GetText(),
			EmailServer: EmailServer.GetText(),
			EmailUsername: EmailUsername.GetText(),
			EmailPassword: EmailPassword.GetText(),
			EmailFromEmail: EmailFromEmail.GetText(),
			EmailFromName: EmailFromName.GetText()
		},
		success: function (msg)
		{
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
		}
	});
}