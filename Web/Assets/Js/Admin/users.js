var TagChangeList = [], $select;

$(function ()
{
    InitTagSelectize();
    var list = $("#MemberStoreIDList").val();
    var m = list.split(',');
    for (var i = 0; i < m.length; i++)
    {
        TagChangeList.push(m[i]);
    }

    $('#recent-bookings').find('a.label').unbind().on('click', function (e) {
        e.preventDefault();

        var rb = $('#recent-bookings'), c = rb.find('.content');

        if (rb.hasClass('expanded')) {
            c.stop().slideToggle(300, function () { rb.toggleClass('expanded'); });
            return;
        }

        c.html('');

        var success = function (html) {
            rb.html(html).find('.content').stop().slideToggle(300, function () {
                rb.toggleClass('expanded');
            });
        };

    });

});

function New()
{
    var url = '/Admin/Users/New';
    window.open(url, '_blank');
}

function Back()
{
    window.location = '/Admin/Users/List';
}

function Reload(url)
{
    ShowLoadingPanel();
    window.location = url;
}

function Delete()
{
    if (btnDelete.GetEnabled() == false) return;
    if (confirm('Are you sure you want to delete this user?') == false) return;
    $.ajax({
        cache: false,
        url: '/Admin/Users/Delete',
        data: { pageID: GetPageID(), id: GetRowID(GrdMain) },
        success: function (msg) {
            if (msg) { alert(msg); return; }
            GrdMain.PerformCallback();
        }
    });
}

function Save()
{

    if (ASPxClientEdit.ValidateEditorsInContainer() == false) return;

    var list = TagChangeList.join(',');
    $('#MemberStoreIDList').val(list);

    Submit('Saving...');
}

function Get()
{
    var id = GetRowID(GrdMain);
    if (!id || id.length < 1) {
        if (btnEdit.GetEnabled() == true && btnDelete.GetEnabled() == true) {
            btnEdit.SetEnabled(false);
            btnDelete.SetEnabled(false);
            return;
        }
    }

    if (btnEdit.GetEnabled() == false && btnDelete.GetEnabled() == false) {
        btnEdit.SetEnabled(true);
        btnDelete.SetEnabled(true);
    }

}

function Edit()
{
    if (btnEdit.GetEnabled() == false) return;
    var url = '/Admin/Users/Edit/' + GetRowID();
    window.open(url, '_blank');
}

function AddTag(value, $item)
{
    for (var i = 0; i < TagChangeList.length; i++)
    {
        if (TagChangeList[i] === value) return;
    }

    TagChangeList.push(value);
}

function RemoveTag(value, $item) {
    TagChangeList = TagChangeList.filter(x => x !== value);
}

function InitTagSelectize()
{
    $select = $('.member-store-list').selectize({ create: false, maxItems: null, onItemAdd: AddTag, onItemRemove: RemoveTag });
}
