$(function () {

    var oGrid_EndCallback = Grid_EndCallback;

    Grid_EndCallback = function () {
        oGrid_EndCallback();
        Get();
    };

});

function Get() {
    var id = GetRowID(GrdMain);
    if (id == null) return;

    $.ajax({
        cache: false,
        url: '/Admin/ApiCredentials/Detail',
        data: { pageID: GetPageID(), id: id },
        success: function (json) {
            $('#UserKey').val(json.UserKey);
            UserID.SetText(json.UserID);
            UserID.SetEnabled(false);
            EntityID.SetText(json.EntityID);
            DisplayName.SetText(json.DisplayName);

            ASPxClientEdit.ValidateGroup('SAVE');
        }
    });
}

function ClearDetail() {
    $('#UserKey').val('');
    UserID.SetText('');
    UserID.SetEnabled(true);
    EntityID.SetText('');
    DisplayName.SetText('');

    ASPxClientEdit.ValidateGroup('SAVE');
}

function New() {
    ClearDetail();
}

function Save() {
    if (!ASPxClientEdit.ValidateGroup('SAVE')) return;
    ShowLoadingPanel("Saving..");

    var id = $('#UserKey').val();

    $.ajax({
        cache: false,
        url: '/Admin/ApiCredentials/Save',
        data: {
            pageID: GetPageID(),
            UserKey: id,
            UserID: UserID.GetText(),
            EntityID: EntityID.GetText(),
            DisplayName: DisplayName.GetText()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
        }
    });
}

function Delete() {
    if (confirm('Are you sure you want to delete this api credential?') == false) return;

    $.ajax({
        cache: false,
        url: '/Admin/ApiCredentials/Delete',
        data: { id: GetRowID(GrdMain) },
        success: function (msg) {
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
            ClearDetail();
        }
    });
}

