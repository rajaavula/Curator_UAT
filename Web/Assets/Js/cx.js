
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}

var textSeparator = ";";

function cxOnListBoxSelectionChanged(dropDown, listBox, args) {
    if (args.index == 0) {
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    }
    else {
        if (!args.isSelected) listBox.UnselectIndices([0]);
    }
    cxUpdateSelectAllItemState(listBox);
    cxUpdateText(dropDown, listBox);
}

function cxUpdateSelectAllItemState(listBox) {
    cxIsAllSelected(listBox) ? listBox.SelectIndices([0]) : listBox.UnselectIndices([0]);
}

function cxIsAllSelected(listBox) {
    if (listBox.GetItem(0).selected && (listBox.GetItem(0).text == "ALL" || listBox.GetItem(0).text.startsWith("ALL;"))) return true; // PL

    for (var i = 1; i < listBox.GetItemCount(); i++)
        if (!listBox.GetItem(i).selected)
            return false;
    return true;
}

function cxUpdateText(dropDown, listBox) {
    var selectedItems = listBox.GetSelectedItems();

    if (selectedItems.length > 0 && (selectedItems[0].text == "ALL" || selectedItems[0].text.startsWith("ALL;")))		// PL
    {
        dropDown.SetText("ALL");
        return;
    }

    dropDown.SetText(cxGetSelectedItemsText(selectedItems));
}

function cxSynchronizeListBoxValues(dropDown, listBox, args) {
    listBox.UnselectAll();
    var texts = dropDown.GetText().split(textSeparator);

    if (texts.length == 1 && texts[0] == "ALL")		// PL
    {
        listBox.SelectAll();
    }
    else {
        var values = cxGetValuesByTexts(listBox, texts);
        listBox.SelectValues(values);
    }

    cxUpdateSelectAllItemState(listBox);
    cxUpdateText(dropDown, listBox);
}

function cxGetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text.split(';')[0]);

    return texts.join(textSeparator);
}

function cxGetValuesByTexts(listBox, texts) {
    var actualValues = [];
    var item;

    if (texts.length == 1 && texts[0] == "") {
        return;
    }

    if (texts.length == 1 && (texts[0] == "ALL" || texts[0].startsWith("ALL;"))) {
        actualValues.push("ALL");
        return;
    }

    for (var n = 0; n < listBox.GetItemCount(); n++) {
        item = listBox.GetItem(n);
        if (item != null && item.text != null) {
            for (var i = 0; i < texts.length; i++) {
                if (item.text.toUpperCase() == texts[i].toUpperCase()) actualValues.push(item.value);
            }
        }

    }
    return actualValues;
}