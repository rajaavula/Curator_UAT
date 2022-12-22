function GrdMain_FocusedRowChanged()
{
	var id = GetRowID(GrdMain);
	if (!id || id.length < 1) return;

	$.ajax({
		cache: false,
		url: '/Admin/Form/FormEditor_Detail',
		data: { pageID: GetPageID(), FormID: id },
		success: function (json)
		{
			if (!json) return;
			$('#FormID').val(json.FormID);
			FormGroup.SetText(json.Group);
			FormArea.SetText(json.Area);
			FormController.SetText(json.Controller);
			FormAction.SetText(json.Action);
			FormParameters.SetText(json.Parameters);
			FormName.SetText(json.Name);
			FormPlaceholderID.SetText(json.PlaceholderID);
			FormDisplayOrder.SetText(json.DisplayOrder);
			FormNotes.SetText(json.Notes);
			FormOwnerOnly.SetChecked(json.OwnersOnly);
			SetValidated(true);
		}
	});
}

function ClearDetail()
{
	$('#FormID').val('');
	FormGroup.SetText('');
	FormArea.SetText('');
	FormController.SetText('');
	FormAction.SetText('');
	FormParameters.SetText('');
	FormName.SetText('');
	FormPlaceholderID.SetText('');
	FormDisplayOrder.SetText('');
	FormNotes.SetText('');
	FormOwnerOnly.SetChecked(false);
	SetValidated(true);
}

function New()
{
	ClearDetail();
	SetValidated(false);
}

function SetValidated(state)
{
	FormGroup.SetIsValid(state);
	FormArea.SetIsValid(state);
	FormController.SetIsValid(state);
	FormAction.SetIsValid(state);
	FormName.SetIsValid(state);
	FormPlaceholderID.SetIsValid(state);
	FormDisplayOrder.SetIsValid(state);
}

function Delete()
{
	if (confirm('Are you sure you want to delete this form?') == false) return;
	$.ajax({
		cache: false, url: '/Admin/Form/FormEditor_Delete',
		data: { pageID: GetPageID(), formID: GetRowID(GrdMain) },
		success: function (msg)
		{
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
			ClearDetail();
		}
	});
}

function Save()
{
	if (!FormGroup.GetIsValid()) return;
	if (!FormArea.GetIsValid()) return;
	if (!FormController.GetIsValid()) return;
	if (!FormAction.GetIsValid()) return;
	if (!FormName.GetIsValid()) return;
	if (!FormPlaceholderID.GetIsValid()) return;
	if (!FormDisplayOrder.GetIsValid()) return;

	ShowLoadingPanel("Saving..");
	$.ajax({
		cache: false,
		url: '/Admin/Form/FormEditor_Save',
		type: "POST",
		data: {
			PageID: GetPageID(),
			FormID: $('#FormID').val(),
			Group: FormGroup.GetText(),
			Area: FormArea.GetText(),
			Controller: FormController.GetText(),
			Action: FormAction.GetText(),
			Parameters: FormParameters.GetText(),
			Name: FormName.GetText(),
			PlaceholderID: FormPlaceholderID.GetText(),
			DisplayOrder: FormDisplayOrder.GetText(),
			Notes: FormNotes.GetText(),
			OwnersOnly: FormOwnerOnly.GetChecked()
		},
		success: function (msg)
		{
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
		}
	});
}

function Cancel()
{
	SetValidated(true);
	GrdMain_FocusedRowChanged();
}