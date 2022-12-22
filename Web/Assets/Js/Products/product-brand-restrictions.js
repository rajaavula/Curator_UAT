function RefreshGridWithArgs(grid, refresh) {

    var args = refresh + '~' + BrandKey.GetValue();

    grid.PerformCallback(args);
}

function AssignCatalog() {

    var list = GrdMain.GetSelectedKeysOnPage();
    if (list.length == 0) return;

    if (CatalogKey.GetIsValid() == false) return;

    ShowLoadingPanel("Updating catalogs..");
    
    $.ajax({
        cache: false,
        url: '/Products/ProductBrandRestrictions/Assign',
        data: {
            pageID: GetPageID(),
            CatalogKey: CatalogKey.GetValue(),
            ids: list.join()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); return; }
            RefreshGridWithArgs(GrdMain, false); 
        }
    });
}