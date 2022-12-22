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
	if (!id || id.length < 1) return;
	
	$.ajax({
		cache: false,
		url: '/Admin/UserGroups/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json) {
			$('#UserGroupID').val(json.UserGroupID);
			UserGroupName.SetText(json.Name);
			Description.SetText(json.Description);
			IsOwner.SetChecked(json.IsOwner);
			IsWorker.SetChecked(json.IsWorker);
			UserGroupName.SetIsValid(true);
		}
	});
}

function New()
{
	Clear();
	UserGroupName.SetIsValid(false);
}

function Delete()
{
	if (confirm('Are you sure you want to delete this user group?') == false) return;

	$.ajax({
		cache: false,
		url: '/Admin/UserGroups/Delete',
		data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
		success: function (msg)
		{
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
			Clear();
		}
	});
}

function Clear()
{
	$('#UserGroupID').val('');
	UserGroupName.SetText('');
	Description.SetText('');
	IsOwner.SetChecked(false);
}

function Save()
{
	if (!UserGroupName.GetIsValid()) return;
	ShowLoadingPanel("Saving..");

	$.ajax({
		cache: false,
		url: '/Admin/UserGroups/Save',
		data: {
			pageID: GetPageID(),
			userGroupID: $('#UserGroupID').val(),
			Name: UserGroupName.GetText(),
			Description: Description.GetText(),
			IsOwner: IsOwner.GetChecked(),
			IsWorker: IsWorker.GetChecked()
		},
		success: function (msg) {
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
		}
	});
}