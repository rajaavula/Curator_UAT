function RefreshGridWithArgs(grid) {
    var args = StoreID.GetValue();

    if (args == '' || args == null) return;

    grid.PerformCallback(args);
}

function DuplicateStores() {
    var list = GrdMain.GetSelectedKeysOnPage();
    if (list.length == 0) return;

    ShowLoadingPanel();

    $.ajax({
        cache: false,
        url: '/Admin/DuplicateStores/DuplicateSelectedStores',
        data: {
            pageID: GetPageID(),
            ids: list.join()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { ShowErrorMessage(msg); return; }
            ShowErrorMessage("Successfully dupllicated stores");
        }
    });
}

function ShowErrorMessage(errorMessage) {
    var width = 500;
    $('#error-message-popup').width(width);
    $('#error-message-popup').css('left', (window.innerWidth / 2) - (width / 2));
    $('#error-message-popup').css('top', '40%');
    $('#error-message-buttons').width(width - 160);

    $('#error-message-popup-message').html(errorMessage);
    ShowPopup('#error-message-popup');
}