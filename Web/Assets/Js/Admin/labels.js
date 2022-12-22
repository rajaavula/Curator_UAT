$(function () {

	var oGrid_EndCallback = Grid_EndCallback;

	Grid_EndCallback = function() {
		oGrid_EndCallback();
		Get();
	};

});

function Get()
{
	var id = GetRowID(GrdMain);
	if (id == null) return;
	
	$.ajax({
		cache: false,
		url: '/Admin/Labels/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
		    $('#LabelID').val(json.LabelID);
		    $('#LabelType').val(json.LabelType);
		    PlaceholderID.SetText(json.PlaceholderID);
			PlaceholderID.SetEnabled(false);
			PlaceholderID.SetIsValid(true);
			
			LanguageID.SetValue(json.LanguageID);			
			LanguageID.SetEnabled(false);
			LanguageID.SetIsValid(true);

			LabelText.SetText(json.LabelText);
			LabelText.SetIsValid(true);

			ToolTipText.SetText(json.ToolTipText);
		}
	});
}

function ClearDetail()
{
    $('#LabelID').val('');
    $('#LabelType').val('');
	PlaceholderID.SetText('');
	LabelText.SetText('');
	ToolTipText.SetText('');
}

function New()
{
	ClearDetail();
	PlaceholderID.SetEnabled(true);
	LanguageID.SetEnabled(true);
	PlaceholderID.SetIsValid(false);
	LabelText.SetIsValid(false);
}

function Save()
{
	if (!PlaceholderID.GetIsValid() || !LabelText.GetIsValid()) return;
	ShowLoadingPanel("Saving..");

	var id = $('#LabelID').val();
	var type = $('#LabelType').val();
	PlaceholderID.SetEnabled(false);
	LanguageID.SetEnabled(false);

	$.ajax({
		cache: false,
		url: '/Admin/Labels/Save',
		data: {
			pageID: GetPageID(),
			LabelID: id,
			LabelType: type,
			placeholderID: PlaceholderID.GetText(),
			languageID: LanguageID.GetValue(),
			labelText: LabelText.GetText(),
			toolTipText: ToolTipText.GetText()
		},
		success: function (msg) {
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
		}
	});
}

function Delete()
{
	if (confirm('Are you sure you want to delete this label?') == false) return;

	$.ajax({
		cache: false,
		url: '/Admin/Labels/Delete',
		data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
		success: function (msg)
		{
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
			ClearDetail();
		}
	});
}

function ChangeGroup()
{
    ShowLoadingPanel();

    setTimeout(function ()
    {
        Submit();
    }, 800);
}