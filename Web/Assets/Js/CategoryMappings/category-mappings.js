function RefreshGridWithArgs(grid)
{
	var args = FeedID.GetValue();

	grid.PerformCallback(args);
}

function Get()
{
	var id = GetRowID(GrdMain);
	if (!id) return;

	$.ajax({
		cache: false,
		url: '/Products/CategoryMappings/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
			if (!json) return;
			$('#CategoryMappingID').val(json.CategoryMappingID);
			ManufacturerName.SetValue(json.ManufacturerName);
			Category1.SetText(json.Category1);
			Category2.SetText(json.Category2);
			Category3.SetText(json.Category3);
			CategoryID.SetValue(json.CategoryID);
		}
	});
}

function Check()
{
	ppConfirm.PerformCallback();
}

function ppConfirm_BeginCallback(s, e)
{
	e.customArgs['id'] = GetRowID(GrdMain);

	ShowLoadingPanel();
}

function ppConfirm_EndCallback()
{
	HideLoadingPanel();

	ppConfirm.Show();
}

function Save()
{
	var id = GetRowID(GrdMain);
	if (!id) return;

	ShowLoadingPanel("Saving...");

	$.ajax({
		cache: false,
		async: false,
		url: '/Products/CategoryMappings/Update',
		data: {
			CategoryMappingID: id,
			CategoryID: CategoryID.GetValue()
		},
		success: function (error)
		{
			HideLoadingPanel();

			if (error)
			{
				alert(error);
				return;
			}

			ppConfirm.Hide();

			RefreshGridWithArgs(GrdMain);
		}
	});
}