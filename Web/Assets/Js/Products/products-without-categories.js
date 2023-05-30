
function RefreshGridWithArgs(grid, refresh) {

    var args = refresh + '~' + FeedKey.GetValue();

    grid.PerformCallback(args);
}

function AssignCategory() {

    var list = GrdMain.GetSelectedKeysOnPage();
    if (list.length == 0) return;

    if (CategoryKey.GetIsValid() == false) return;

    ShowLoadingPanel("Updating categories..");
    
    $.ajax({
        cache: false,
        url: '/Products/ProductsWithoutCategories/Assign',
        type: 'POST',
        data: {
            pageID: GetPageID(),
            categoryKey: CategoryKey.GetValue(),
            ids: list.join()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); return; }
            RefreshGridWithArgs(GrdMain, false); 
        }
    });
}
