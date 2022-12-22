function GrdMain_EndCallback(s, e) {
  Grid_EndCallback(s, true);

  Get();
}

function RefreshGrdMainWithArgs(refresh) {
    var args = refresh;

    GrdMain.PerformCallback(args);
}

function RefreshGrdProductsWithArgs(refresh) {
  var args = refresh + '~' + GetRowID(GrdMain);

  GrdProducts.PerformCallback(args);
}

function TabChanged(s, e) {
  window.onresize();

  let tab = null;

  switch (e.tab.index) {
    case 0:
      tab = '.tab-vendors';
      break;

    case 1:
      tab = '.tab-products';

      RefreshGrdProductsWithArgs(true);
      break;
  }

  $('.tab-button').addClass('hide');
  $(tab).removeClass('hide');
}

function Get() {
  const tab = tabMain.tabs[1];

  var id = GetRowID(GrdMain);
  if (id == null) {
    tab.SetEnabled(false);
    return;
  }

  $.ajax({
    cache: false,
    url: '/Products/ERPIntegrations/Detail',
    data: { PageID: GetPageID(), ID: id },
    success: function (json) {
      $('#VendorID').val(json.VendorID);
      VendorName.SetValue(json.VendorName);
      MagentoID.SetValue(json.MagentoID);
      MagentoEnabled.SetChecked(json.MagentoEnabled);
      NetSuiteInternalID.SetValue(json.NetSuiteInternalID);
      NetSuiteEntityID.SetValue(json.NetSuiteEntityID);
      NetSuiteCode.SetValue(json.NetSuiteCode);
      NetSuiteName.SetValue(json.NetSuiteName);
      NetSuiteEnabled.SetChecked(json.NetSuiteEnabled);

      let enabled = json.MagentoID == null;

      MagentoID.SetEnabled(enabled);

      enabled = json.NetSuiteInternalID == null;

      NetSuiteInternalID.SetEnabled(enabled);
      NetSuiteEntityID.SetEnabled(enabled);
      NetSuiteCode.SetEnabled(enabled);
      NetSuiteName.SetEnabled(enabled);

      enabled = json.MagentoID != null && json.NetSuiteInternalID != null;

      tab.SetEnabled(enabled);
    }
  });
}

function Save() {
  if (!ASPxClientEdit.ValidateGroup()) return;

  ShowLoadingPanel("Saving..");

  $.ajax({
    cache: false,
    url: '/Products/ERPIntegrations/Save',
    data: {
      PageID: GetPageID(),
      VendorID: $('#VendorID').val(),
      MagentoID: MagentoID.GetValue(),
      MagentoEnabled: MagentoEnabled.GetChecked(),
      NetSuiteInternalID: NetSuiteInternalID.GetValue(),
      NetSuiteEntityID: NetSuiteEntityID.GetValue(),
      NetSuiteCode: NetSuiteCode.GetValue(),
      NetSuiteName: NetSuiteName.GetValue(),
      NetSuiteEnabled: NetSuiteEnabled.GetChecked()
    },
    success: function (message) {
      HideLoadingPanel();
      if (message) { alert(message); return; }
      RefreshGrdMainWithArgs(false);
    }
  });
}

function Send() {

  var list = GrdProducts.GetSelectedKeysOnPage();
  if (list.length === 0) return;

  ShowLoadingPanel("Flagging for integration updates..");

  $.ajax({
    cache: false,
    url: '/Products/ERPIntegrations/Send',
    data: {
      PageID: GetPageID(),
      ProductIDs: list.join()
    },
    success: function (msg) {
      HideLoadingPanel();
      if (msg) { alert(msg); return; }

      RefreshGrdProductsWithArgs(false);
    }
  });
}

