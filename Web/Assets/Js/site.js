var registeredGridViews = [], registeredTabControls = [], esia = false, tocs = false, cpgh = false, menuExpanded = false, tabsHeight = 20;
function GetUnique() { var cxd = new Date(); return (cxd.getFullYear().toString() + cxd.getMonth().toString() + cxd.getDay().toString() + cxd.getHours().toString() + cxd.getMinutes().toString() + cxd.getSeconds().toString() + cxd.getMilliseconds()); }
function RefreshGrid(grid) { grid.PerformCallback(); }
function ResetGrid(c) {
    if (!confirm('This will reset the grid layout.\r\n Are you sure?')) return;
    ASPxClientUtils.DeleteCookie(c);
    $.post('/Home/Home/GridReset', { ModelID: ModelID }, function () { location.reload(); });
}
function RegisterGridView(g) { registeredGridViews[registeredGridViews.length] = g; }
function RegisterTabControl(t) { registeredTabControls[registeredTabControls.length] = t; AdjustTabSize(t); if (registeredGridViews && registeredGridViews.length > 0) { for (var i = 0; i < registeredGridViews.length; i++) { AdjustGridSize(registeredGridViews[i]); } } }
function GetPageID() { var ctrl = document.getElementById('PageID'); if (ctrl) { return ctrl.value; } else { return null; } }
function SetGridArgs(s, model) { try { s.callbackCustomArgs['ModelName'] = model; s.callbackCustomArgs['PageID'] = GetPageID(); } catch (e) { } }
function GetRowID(grid) { try { return grid.GetRowKey(grid.GetFocusedRowIndex()); } catch (ex) { return null; } }
function ShowLoadingPanel(msg) { if (!pnlLoading) return; if (msg === undefined) msg = 'Loading...'; pnlLoading.SetText(msg); pnlLoading.Show(); }
function HideLoadingPanel() { if (pnlLoading) pnlLoading.Hide(); }
function Submit(msg) { if (document.forms.length < 1) return; ShowLoadingPanel(msg); document.forms[0].submit(); }
function Print() { $("#PrintButton").click(); }
function AdjustGridSize(s) {
    try {
        var gridHeight = $('#content-pane').height() - tabsHeight;
        var editPane = $(s.GetMainElement()).closest('#grid-container, .grid-container').find('#edit-pane');
        var gridWidth = $('#content-pane').width();
        if (registeredTabControls && registeredTabControls.length > 0) gridWidth -= 16;
        if (editPane && editPane.length > 0) gridWidth -= (editPane.width() + 48);
        s.SetHeight(gridHeight);
        s.SetWidth(gridWidth);
        s.AdjustControl();
        s.SetClientVisible(true);
    } catch (e) { }
}
function AdjustTabSize(s) { try { t.SetHeight($('#content-pane').height() - tabsHeight); s.AdjustControl(); } catch (e) { } }
function Grid_Init(s, model, autoResize) { if (autoResize) { RegisterGridView(s); AdjustGridSize(s); } SetGridArgs(s, model); }
function Grid_BeginCallback(s, model) { SetGridArgs(s, model); }
function Grid_EndCallback(s, autoResize) { if (autoResize) { AdjustGridSize(s); } }
$.fn.hasAttr = function (name) { return this.attr(name) !== undefined; };
String.prototype.lpad = function (padString, length) { var str = this; while (str.length < length) { str = padString + str; } return str; };
function IsDefined(s) { return (typeof s === 'undefined'); }
function FormatDate(d) { if (d === null) return null; var yr = (d.getFullYear()).toString(); var mo = (d.getMonth() + 1).toString(); var dy = (d.getDate()).toString(); var hr = (d.getHours()).toString(); var mi = (d.getMinutes()).toString(); return yr + "/" + mo.lpad("0", 2) + "/" + dy.lpad("0", 2) + " " + hr.lpad("0", 2) + ":" + mi.lpad("0", 2); };
function Dropdown_BeginCallback(s, e) { e.customArgs['PageID'] = GetPageID(); }
function Dropdown_EndCallback(s, e) { }
function IsMobile() { try { return (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))); } catch (e) { return false; } };
function Pivot_Init(s, model, autoResize) { if (autoResize) { RegisterGridView(s); AdjustPivotSize(s); } SetGridArgs(s, model); }
function Pivot_BeginCallback(s, model) { SetGridArgs(s, model); }
function Pivot_EndCallback(s, autoResize) { if (autoResize) { AdjustPivotSize(s); } }
function AdjustPivotSize(s) { try { s.SetHeight($('#content-pane').height() - tabsHeight); s.AdjustControl(); } catch (e) { } }
function ParseMSDate(s) { return new Date(parseInt(s.substr(6))); }

window.onresize = function () {
    var windowHeight = $(window).height();
    var contentPaneHeight = windowHeight;

    var mainContent = document.getElementById("main-content");
    var menuWidth = 56;
    if (menuExpanded) menuWidth = 250; 
    var styleWidth = "calc( 100% - " + menuWidth + "px )";

    mainContent.style.width = styleWidth;

    var contentPane = $('#content-pane'), verticalMargin = 12;
    if (contentPane.length > 0) {
        contentPaneHeight = (windowHeight - contentPane.position().top - verticalMargin);
        contentPane.height(contentPaneHeight);
    }

    if (registeredTabControls && registeredTabControls.length > 0) {
        for (var i = 0; i < registeredTabControls.length; i++) AdjustTabSize(registeredTabControls[i]);
    }

    var editPane = $('#edit-pane'), editPaneMargin = 18;
    if (editPane.length > 0) {
        editPane.height(contentPaneHeight - tabsHeight - (editPaneMargin * 2));
    }

    var gridPane = $('#grid-pane, .grid-pane');
    if (gridPane.length > 0) {
        gridPane.height(contentPaneHeight - tabsHeight);
    }

    if (registeredGridViews && registeredGridViews.length > 0) {
        for (var n = 0; n < registeredGridViews.length; n++) AdjustGridSize(registeredGridViews[n]);
    }
};

function SaveLabel() {
    var placeholderID = $('#LabelEditor_PlaceholderID').val(), labelText = LabelEditor_LabelText.GetText(), toolTipText = LabelEditor_ToolTipText.GetText();
    if (!placeholderID || placeholderID.length < 1) return;
    if (!labelText || labelText.length < 1) { LabelEditor_LabelText.SetIsValid(false); return; }
    $.ajax({
        cache: false, url: '/Home/Home/UpdateLabel',
        data: { PlaceHolderID: placeholderID, LabelText: labelText, ToolTipText: toolTipText },
        success: function (error) {
            if (error && error.length > 0) { alert(error); return; }
            ppLabelEditor.Hide();
            window.location = window.location.href;
        }
    });
};

function SaveAccountDetails() {
    if (ASPxClientEdit.ValidateGroup('ACCOUNT_DETAILS') == false) return;

    btnAccountDetailsSave.SetEnabled(false);

    $.ajax({
        cache: false,
        url: '/Home/Home/UpdateDetails',
        data: {
            NewProductNotifications: NewProductNotifications.GetChecked(),
            ChangedProductNotifications: ChangedProductNotifications.GetChecked(),
            DeactivatedProductNotifications: DeactivatedProductNotifications.GetChecked()
        },
        success: function (err) {
            if (err && err.length > 0) {
                $('#account-details-error').html('<p>' + err + '</p>');
                btnAccountDetailsSave.SetEnabled(true);
                return;
            }

            $('#account-details-update, #account-detail-success').toggle();
        }
    });
}

function SaveAccountRegion() {
    if (ASPxClientEdit.ValidateGroup('ACCOUNT_REGIONS') == false) return;

    ShowLoadingPanel();

    btnAccountRegionsSave.SetEnabled(false);

    $.ajax({
        cache: false,
        url: '/Home/Home/ChangeRegion',
        data: { RegionID: txtAccountRegionID.GetValue() },
        complete: function () { HideLoadingPanel(); },
        success: function (error) {
            if (error && error.length > 0) {
                $('#account-regions-error').html('<p>' + error + '</p>');
                btnAccountRegionsSave.SetEnabled(true);
                return;
            }

            $('#account-regions-update, #account-regions-success').toggle();
        }
    });
}

$(function () {
    if (registeredTabControls && registeredTabControls.length > 0) tabsHeight = 60;
  
    //*******************************************************//
    // Nav bar menu
    menuExpanded = $('#main-menu').hasClass("expanded");
    UpdateMenuUI(menuExpanded);

    $('#main-menu').hover(
        function (e) {
            if (menuExpanded) return;
            UpdateMenuUI(true);
        },
        function (e) {
            if (menuExpanded) return;
            UpdateMenuUI(false);
        }
    );

    var pin = $('.dx-navbar-pin');
    pin.hover(
        function (e) {
            var pinnedClass = 'dx-navbar-pin-selected';
            pin.find('.dx-navbar-icon').toggleClass(pinnedClass);
        },
        function (e) {
            SetMenuPinned(menuExpanded);
        }
    );

    pin.click(function (e) {
        menuExpanded = !menuExpanded;
        SetMenuPinned(menuExpanded);
        SetMenuExpanded(menuExpanded);

        window.onresize();
    });

    //*******************************************************//

    window.onresize();

    /* Keep alive */
    setInterval(function () { $.ajax({ cache: false, url: '/Home/Home/KeepAlive', data: { id: ModelID }, success: function (json) { if (json && json.OK) return; window.location = '/Login'; } }); }, 300000); /* 5 minutes */

    $('.edit-label').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var placeholderID = $(e.currentTarget).attr('id');
        $('#LabelEditor_PlaceholderID').val('');
        LabelEditor_LabelText.SetValue('');
        LabelEditor_ToolTipText.SetValue('');
        $.ajax({
            cache: false,
            url: '/Home/Home/GetLabel',
            data: { PlaceholderID: placeholderID },
            success: function (label) {
                $('#LabelEditor_PlaceholderID').val(label.PlaceholderID);
                LabelEditor_LabelText.SetValue(label.LabelText);
                LabelEditor_ToolTipText.SetValue(label.ToolTipText);
            }
        });
        ppLabelEditor.SetPopupElementID(placeholderID);
        ppLabelEditor.Show();
    });

    var tooltipDiv = $('div#tool-tip-content'), isShowing = false;
    $('.tool-tip').mouseover(function (e) {
        if (isShowing) return;
        isShowing = true;
        tooltipDiv.html('');
        $.ajax({
            cache: false, url: '/Home/Home/GetLabel',
            data: { PlaceholderID: $(e.currentTarget).attr('id') },
            success: function (label) {
                tooltipDiv.html(label.ToolTipText).css({ left: e.pageX + 10, top: e.pageY + 10, opacity: 0.9, width: "auto", display: "none" });
                if (tooltipDiv.width() > 500) tooltipDiv.width(500);
                tooltipDiv.stop(true, true).show();
            }
        });
    }).mouseout(function () {
        if (isShowing == false) return;
        isShowing = false;
        tooltipDiv.stop(true, true).hide().html('');
    });

    var phc = $('#phc');

    $('#account-username').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        phc.toggleClass('selected');
    });

    phc.find('.account-details').on('click', function (e) {
        e.preventDefault();
        ppAccountDetails.PerformCallback();
        phc.removeClass('selected');
    });

    phc.find('.account-change-region').on('click', function (e) {
        e.preventDefault();
        ppAccountRegions.PerformCallback();
        phc.removeClass('selected');
    });

    $(document).on('click', function () { phc.removeClass('selected'); });

    LoadJiraHelp();
});

async function LoadJiraHelp() {
    var jsd = $('#jsd-widget');
    jsd.css({ top: '-5px' });
    jsd.css({ right: '300px' });
    jsd.css({ bottom: 'unset' });

    var helpButton = document.getElementById("jsd-widget").contentWindow.document.getElementById('help-button');

    if (helpButton != null && helpButton != undefined) {
        helpButton.style.height = '35px';
        helpButton.style.lineHeight = '35px';
        helpButton.style.marginBottom = '8px';
        helpButton.style.fontSize = '14px';
        helpButton.style.fontWeight = '400';
        AfterJiraLoad();
        return;
    };

    window.setTimeout(LoadJiraHelp, 100);
}

function AfterJiraLoad() {
    var buttonContainer = document.getElementById("jsd-widget").contentWindow.document.getElementById('button-container');
    buttonContainer.onclick = function () {
        var jsd = $('#jsd-widget');
        jsd.css({ top: 'unset' });
        jsd.css({ right: '0' });
        jsd.css({ bottom: '0' });

        SetCloseJiraFunction();
    };
}

async function SetCloseJiraFunction() {
    var closebuttonContainer = document.getElementById("jsd-widget").contentWindow.document.getElementsByClassName('header-close-icon-container');

    if (closebuttonContainer.length >= 1) {
        closebuttonContainer[0].onclick = function () {
            LoadJiraHelp();
        };

        return;
    };

    window.setTimeout(SetCloseJiraFunction, 100);
}

function LoadingExport(s, e) {
    ShowLoadingPanel('Please wait while your export is being generated.');
    setTimeout(function () { HideLoadingPanel(); }, 5000);
}

function UpdateMenuUI(expanded) {
    if (expanded) {
        $('.dx-navbar-burger').addClass('hide');
        $('.dx-navbar-pin').removeClass('hide');
        $('.dx-navbar-container').removeClass('hide');
        $('.dx-navbar-group-header span').addClass('expanded');
        $('#main-menu').addClass('expanded');
    }
    else {
        $('#main-menu').removeClass('expanded');
        $('.dx-navbar-container').addClass('hide');
        $('.dx-navbar-burger').removeClass('hide');
        $('.dx-navbar-pin').addClass('hide');
        $('.dx-navbar-group-header span').removeClass('expanded');
    }
}

function SetMenuPinned(pinned) {
    var pin = $('.dx-navbar-pin').find('.dx-navbar-icon');
    var pinnedClass = 'dx-navbar-pin-selected';

    if (pinned && !pin.hasClass(pinnedClass)) {
        pin.addClass(pinnedClass);
    }
    else {
        pin.removeClass(pinnedClass);
    }
}

function SetMenuExpanded(expanded) {
    $.ajax({
        cache: false,
        url: '/Home/Home/UpdateMenuExpanded',
        data: { MenuExpanded: menuExpanded },
        success: function (json) {
            return;
        }
    });
}

function HidePopup(selector) {
    var popup = $(selector);
    if (!popup || popup === undefined || popup.length === 0) return;

    popup.addClass('hide');
    popup.prev('.popup-background').addClass('hide');
}

function ShowPopup(selector) {
    var popup = $(selector);
    if (!popup || popup === undefined || popup.length === 0) return;

    popup.removeClass('hide');
    popup.prev('.popup-background').removeClass('hide');
}