
function AssignCategory()
{
    var allIds = GrdMain.GetKeyValues();
    var ids = GrdMain.GetSelectedKeysOnPage();

    ShowLoadingPanel("Assigning categories..");
    
    $.ajax({
        cache: false,
        url: '/Products/MemberCategories/Assign',
        data: {
            pageID: GetPageID(),
            all: allIds,
            ids: ids.join()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); }
            RefreshGridWithArgs(GrdMain); 
        }
    });
}

function RefreshGridWithArgs(grid) {
    var args = StoreID.GetValue();
    grid.PerformCallback(args);
}

var GrdMain_Refreshing = false;
var GrdMain_ResetSelection = false;

function GrdMain_BeginCallback(s, e, modelName) {
    Grid_BeginCallback(s, e, modelName);

    if (e.command == 'CUSTOMCALLBACK') {
        GrdMain_Refreshing = true;
    }
}

function GrdMain_EndCallback(s, autoResize) {
    Grid_EndCallback(s, autoResize)

    if (GrdMain_Refreshing) {
        s.UnselectRows();

        GrdMain_Refreshing = false;
        GrdMain_ResetSelection = true;
        return;
    }

    if (GrdMain_ResetSelection) {
        s.SelectRowsByKey(s.cpSelected);

        GrdMain_ResetSelection = false;
        return;
    }
}


