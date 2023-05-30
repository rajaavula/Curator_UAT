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
        url: '/Admin/Stores/Detail',
        data: { pageID: GetPageID(), id: id },
        success: function (json) {
            $('#StoreID').val(json.StoreID);
            StoreName.SetText(json.StoreName);
            StoreUrl.SetText(json.StoreUrl);
            StoreApiKey.SetText(json.StoreApiKey);
            StorePassword.SetText(json.StorePassword);
            StoreSharedSecret.SetText(json.StoreSharedSecret);
            EnableAutomaticNetSuiteUpdate.SetChecked(json.EnableAutomaticNetSuiteUpdate);
            DoNotUpdateRRP.SetChecked(json.DoNotUpdateRRP);
            DoNotUpdateCostPrice.SetChecked(json.DoNotUpdateCostPrice);
            DoNotUpdateInventory.SetChecked(json.DoNotUpdateInventory);
            ShopifyID.SetText(json.ShopifyID);
            $('#logo-preview').attr('src', '/Admin/Stores/StoreLogoPreview/' + GetPageID() + '?' + GetUnique());
        }
    });
}

function ClearDetail() {
    $('#StoreID').val('');
    StoreName.SetText('');
    StoreUrl.SetText('');
    StoreApiKey.SetText('');
    StorePassword.SetText('');
    StoreSharedSecret.SetText('');
    EnableAutomaticNetSuiteUpdate.SetChecked(false);
    DoNotUpdateRRP.SetChecked(false);
    DoNotUpdateCostPrice.SetChecked(false);
    DoNotUpdateInventory.SetChecked(false);
    ShopifyID.SetText('');
}

function New() {
    ClearDetail();
}

function Save() {
    if (!StoreUrl.GetIsValid() || !StoreApiKey.GetIsValid() || !StorePassword.GetIsValid() || !StoreSharedSecret.GetIsValid() || !ShopifyID.GetIsValid()) return;
    ShowLoadingPanel("Saving..");

    var id = $('#StoreID').val();

    $.ajax({
        cache: false,
        url: '/Admin/Stores/Save',
        data: {
            pageID: GetPageID(),
            StoreID: id,
            storeName: StoreName.GetText(),
            storeUrl: StoreUrl.GetText(),
            storeApiKey: StoreApiKey.GetText(),
            storePassword: StorePassword.GetText(),
            storeSharedSecret: StoreSharedSecret.GetText(),
            EnableAutomaticNetSuiteUpdate: EnableAutomaticNetSuiteUpdate.GetChecked(),
            DoNotUpdateRRP: DoNotUpdateRRP.GetChecked(),
            DoNotUpdateCostPrice: DoNotUpdateCostPrice.GetChecked(),
            DoNotUpdateInventory: DoNotUpdateInventory.GetChecked(),
            shopifyID: ShopifyID.GetText()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
        }
    });
}

function Delete() {
    if (confirm('Are you sure you want to delete this store?') == false) return;

    $.ajax({
        cache: false,
        url: '/Admin/Stores/Delete',
        data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
        success: function (msg) {
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
            ClearDetail();
        }
    });
}

function LogoUploadComplete() {
    $('#logo-preview').attr('src', '/Admin/Stores/StoreLogoPreview' + '?' + GetUnique());
}