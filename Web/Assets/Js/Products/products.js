
function ChangeFeed() {

    BrandKey.PerformCallback();
}

function Brand_BeginCallback(s, e) {

	e.customArgs['PageID'] = GetPageID();
	e.customArgs['FeedKey'] = FeedKey.GetValue();
}

function Brand_EndCallback(s, e) {

	s.SetSelectedIndex(1);
}

function RefreshGridWithArgs(grid) {

    var args = FeedKey.GetValue() + '~' + BrandKey.GetValue();

    grid.PerformCallback(args);
}

function Get()
{
	var id = GetRowID(GrdMain);	
	if (!id) return;

	$.ajax({
		cache: false,
		url: '/Products/Products/ListPreview',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
			if (!json) return;
			$("#LongDescription").text(json.LongDescription);
		}
	});
	$.ajax({
		cache: false,
		url: '/Products/Products/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json) {
			if (!json) return;
			$('#ProductID').val(json.ProductID);
			CategoryKey.SetValue(json.CategoryKey);
			RecommendedRetailPrice.SetValue(json.RecommendedRetailPrice);
			MemberRecommendedRetailPrice.SetValue(json.MemberRecommendedRetailPrice);
			BuyPrice.SetValue(json.BuyPrice);
			Markup.SetValue(json.Markup);
			StockOnHand.SetValue(json.StockOnHand);
			IncludeZeroStock.SetValue(json.IncludeZeroStock);
		}
	});
}

function Save() {
	
	ShowLoadingPanel("Saving..");

	$.ajax({
		cache: false,
		url: '/Products/Products/Save',
		data: {
			pageID: GetPageID(),
			ProductID: $('#ProductID').val(),
			CategoryKey: CategoryKey.GetValue(),
			RecommendedRetailPrice: RecommendedRetailPrice.GetValue(),
			MemberRecommendedRetailPrice: MemberRecommendedRetailPrice.GetValue(),
			BuyPrice: BuyPrice.GetValue(),
			Markup: Markup.GetValue(),
			StockOnHand: StockOnHand.GetText(),
			IncludeZeroStock: IncludeZeroStock.GetChecked()
		},
		success: function (msg) {
			HideLoadingPanel();
			if (msg) { alert(msg); return; }
			GrdMain.PerformCallback();
		}
	});
}

