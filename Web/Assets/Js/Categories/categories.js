function Get() {
    var id = GetRowID(GrdMain);
    if (id === null) return;

    $.ajax({
        cache: false,
        url: '/Products/Categories/Detail',
        data: { pageID: GetPageID(), id: id },
        success: function (json) {
            $('#CategoryKey').val(json.CategoryKey);
            Name.SetText(json.Name);
            Description.SetText(json.Description);
            ParentKey.SetValue(json.ParentKey);

            Name.SetIsValid(true);
            Description.SetIsValid(true);             
        }
    });
}

function GrdMain_EndCallback(s, e)
{
    Grid_EndCallback(s, true);
    ParentKey.PerformCallback();
}

function ParentCategory_BeginCallback(s, e) {

	e.customArgs['PageID'] = GetPageID();
}

function ParentCategory_EndCallback(s, e) {

	s.SetSelectedIndex(0);    
    Get();
}

function ClearDetail() {
    $('#CategoryKey').val(null);
    Name.SetText('');				// DevExpress controls
    Description.SetText('');
    ParentKey.SetValue(null);	

    Name.SetIsValid(false);
    Description.SetIsValid(false);
}

function New() {
    ClearDetail();
}

function Save() {
    if (!Name.GetIsValid() || !Description.GetIsValid()) return;
    ShowLoadingPanel("Saving..");

    $.ajax({
        cache: false,
        url: '/Products/Categories/Save',
        data: {
            pageID: GetPageID(),
            categoryKey: $('#CategoryKey').val(),
            name: Name.GetText(),
            description: Description.GetText(),
            parentKey: ParentKey.GetValue()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); }
            GrdMain.PerformCallback();
        }
    });
}

function Delete()
{
	if (confirm('Are you sure you want to delete this category?') == false) return;

	$.ajax({
		cache: false,
		url: '/Products/Categories/Delete',
		data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
		success: function (msg)
		{
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
			ClearDetail();
		}
	});
}
