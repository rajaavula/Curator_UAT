var TagChangeList = [], $select, tags, selectedCategories;

$(function () {
    InitTagSelectize();
    tags = $select[0].selectize;
});

function ChangeMemberStore() {
    storeID: StoreID.GetValue();
    FeedKey.PerformCallback();
    tags.clear();
}

function ChangeMemberFeed() {
    CategoryKey_ListBox.PerformCallback();
    Brand_ListBox.PerformCallback();
    tags.clear();
}

function RefreshGridWithArgs(grid) {
    tags.clear();
    var args = StoreID.GetValue() + '~' + FeedKey.GetValue() + '~' + CategoryKey_ListBox.GetSelectedValues() + '~' + Brand_ListBox.GetSelectedValues();
    grid.PerformCallback(args);
}

function MemberFeed_BeginCallback(s, e) {

    e.customArgs['PageID'] = GetPageID();
    e.customArgs['storeID'] = StoreID.GetValue();

}

function MemberFeed_EndCallback(s, e) {

    s.SetSelectedIndex(0);
    ChangeMemberFeed();
}

function MemberCategory_BeginCallback(s, e) {

    e.customArgs['PageID'] = GetPageID();
    e.customArgs['feedKey'] = FeedKey.GetValue();
}

function MemberCategory_EndCallback(s, e) {

    var listbox = CategoryKey_ListBox;
    var items = listbox.GetItemCount();

    if (items === 0) return;

    if (items === 1) {
        listbox.SelectAll();
    }
    else {
        if (selectedCategories != null) {
            listbox.SelectValues(selectedCategories);
            selectedCategories = null;
        }
        else {
            listbox.SetSelectedIndex(1);
        }
    }

    cxUpdateText(CategoryKey, listbox);
}

function Brand_BeginCallback(s, e) {

    e.customArgs['PageID'] = GetPageID();
    e.customArgs['feedKey'] = FeedKey.GetValue();
}

function Brand_EndCallback(s, e) {

    var listbox = Brand_ListBox;
    var items = listbox.GetItemCount();

    if (items === 0) return;

    listbox.SelectAll();

    cxUpdateText(Brand, listbox);
}

function Save() {
    var list = GrdMain.GetSelectedKeysOnPage();
    if (list.length == 0) return;

    ShowLoadingPanel("Saving..");

    $.ajax(
        {
            cache: false,
            url: '/Products/MemberProducts/GetSelectedProducts',
            data: {
                pageID: GetPageID(),
                ProductID: $('#ProductID').val(),
                pricingRule: PricingRule.GetValue(),
                priceValue: PriceValue.GetValue(),
                retailRounding: RetailRounding.GetChecked(),
                ids: list.join()
            },
            success: function (msg) {
                HideLoadingPanel();
                if (msg) { alert(msg); return; }
                GrdMain.PerformCallback();
            }
        });
}

function OnRuleChanged(s, e) {

    $("#RuleLabel").text(s.GetSelectedItem().text);
}

function ReloadCategories() {
    selectedCategories = CategoryKey_ListBox.GetSelectedValues();

    CategoryKey_ListBox.PerformCallback();
}

function Get() {

    var s = GrdMain;

    Grid_EndCallback(s, true);

    ReloadCategories();

    if (s.GetVisibleRowsOnPage() === 0) {
        $('#export-download').addClass('hide');
    }
    else {
        $('#export-download').removeClass('hide');
    }

    var id = GetRowID(GrdMain);
    if (!id) return;

    $.ajax({
        cache: false,
        url: '/Products/MemberProducts/ListPreview',
        data: { pageID: GetPageID(), id: id },
        success: function (json) {
            if (!json) return;
            $("#txtPricingRuleText").val(json.PricingRuleText);
            $("#txtPriceValue").val(parseFloat(json.PriceValue).toFixed(2));
            $("#txtRetailRounding").val(json.RetailRounding);
            $("#txtNewRRP").val(parseFloat(json.NewRRP).toFixed(2));
            DefaultProductTags.SetChecked(json.DefaultProductTags);
            tags.setValue(json.ProductTags.split(','));
            TagChangeList = json.ProductTags.split(',');

            DefaultTagsChanged(DefaultProductTags);
        }
    });
}

function SaveProductTags() {
    var list = GrdMain.GetSelectedKeysOnPage();
    if (list.length == 0) return;

    ShowLoadingPanel("Saving..");

    $.ajax({
        cache: false,
        url: '/Products/MemberProducts/SaveProductTags',
        data: {
            pageID: GetPageID(),
            ProductID: $('#ProductID').val(),
            ids: list.join(),
            productTags: TagChangeList.join(','),
            defaultProductTags: DefaultProductTags.GetChecked()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
        }
    });
}

function InitTagSelectize() {

    $select = $('.product-tag-list').selectize({ create: true, maxItems: null, onItemAdd: AddTag, onItemRemove: RemoveTag });

}

function AddTag(value, $item) {

    for (var i = 0; i < TagChangeList.length; i++) {
        if (TagChangeList[i] === value) return;
    }

    TagChangeList.push(value);
}

function RemoveTag(value, $item) {
    TagChangeList = TagChangeList.filter(x => x !== value);
}

/*** Update selected items ***/

var GrdAvailable_Refreshing = false;
var GrdAvailable_ResetSelection = false;

function GrdAvailable_BeginCallback(s, e, modelName) {
    Grid_BeginCallback(s, e, modelName);

    if (e.command == 'CUSTOMCALLBACK') {
        GrdAvailable_Refreshing = true;
    }
}

function GrdAvailable_EndCallback(s, autoResize) {
    Grid_EndCallback(s, autoResize);

    if (GrdAvailable_Refreshing) {
        s.UnselectRows();

        GrdAvailable_Refreshing = false;
        GrdAvailable_ResetSelection = true;
        return;
    }

    if (GrdAvailable_ResetSelection) {
        s.SelectRowsByKey(s.cpSelected);

        GrdAvailable_ResetSelection = false;
        return;
    }
}

function UpdateSelection() {
    var allIds = GrdAvailable.GetKeyValues();
    var ids = GrdAvailable.GetSelectedKeysOnPage();

    ShowLoadingPanel("Updating selection..");

    $.ajax({
        cache: false,
        url: '/Products/MemberProducts/UpdateSelection',
        data: {
            pageID: GetPageID(),
            all: allIds,
            ids: ids.join()
        },
        success: function (msg) {
            HideLoadingPanel();
            if (msg) { alert(msg); }
            RefreshGridWithArgs(GrdMain);
            RefreshGridWithArgs(GrdAvailable);
        }
    });
}

function ActiveTabChanged(s, e) {

    Update.SetVisible(e.tab.index === 1);
    window.onresize();
}

function DefaultTagsChanged(s, e) {

    if (s.GetChecked() === true) {
        $('#product-tags-row').addClass('hide');
        return;
    }

    $('#product-tags-row').removeClass('hide');
}

function BulkPRImportStart(s, e) {
    ShowLoadingPanel();
    btnUploadBulkImport.SetEnabled(false);
    btnUploadBulkImport.SetText('Uploading...');
}

function BulkPRImportComplete(s, e) {
    HideLoadingPanel();

    btnUploadBulkImport.SetText('Import');
    btnUploadBulkImport.SetEnabled(true);

    if (e.errorText && e.errorText.length > 0) {
        $('#txtBulkPRImportResults').val(e.errorText);
        $('#txtImportSuccess').addClass('hide');
        $('#txtImportSuccess').removeClass('label');
        ppBulkPRImportResult.Show();
    }
    else {
        GrdMain.PerformCallback();
        $('#txtImportSuccess').removeClass('hide');
        $('#txtImportSuccess').addClass('label');
    }
}

function BulkPRImportTextChanged(s, e) {
    btnUploadBulkImport.SetEnabled(uplBulkPRImport.GetText(0) != "");
}