let FeedStoreType = null, CredentialsLoaded = false;

function RefreshGridWithArgs(grid, refresh) {
    var args = refresh;

    grid.PerformCallback(args);
}

function Get() {
	var id = GetRowID(GrdMain);
	if (!id) return;

	$.ajax({
		cache: false,
		url: '/Products/FeedStores/Detail',
		data: { pageID: GetPageID(), id: id },
		success: function (json)
		{
			if (!json) return;
			$('#FeedStoreID').val(json.FeedStoreID);
			FeedName.SetValue(json.FeedName);
			StoreName.SetValue(json.StoreName);
			PushToSupplierEmail.SetValue(json.PushToSupplierEmail);
			CredentialsProvided.SetChecked(json.CredentialsProvided);
			ClientID.SetValue(json.ClientID);
			ClientSecret.SetValue(null);
			CustomerNumber.SetValue(json.CustomerNumber);
			CountryCode.SetValue(json.CountryCode);
			Username.SetValue(json.Username);
			Password.SetValue(null);
			SourceIdentifier.SetValue(json.SourceIdentifier);
			ConsumerKey.SetValue(json.ConsumerKey);
			ConsumerSecret.SetValue(null);

			CredentialsLoaded = false;

			$('.credentials').addClass('d-none');

			switch (json.FeedName)
			{
				case 'INGRAM':
					FeedStoreType = json.FeedName;
					$('.edi-supplier').removeClass('d-none');
					$('.enter-credentials').removeClass('d-none');
					break;

				case 'SYNNEX':
					FeedStoreType = json.FeedName;
					$('.edi-supplier').removeClass('d-none');
					$('.enter-credentials').removeClass('d-none');
					break;

				default:
					FeedStoreType = 'EMAIL';
					$('.email-supplier').removeClass('d-none');
					$('.enter-credentials').addClass('d-none');
					break;
      }
		}
	});
}

function Credentials()
{
	var id = $('#FeedStoreID').val();
	if (!id) return;

	ShowLoadingPanel();

	$.ajax({
		cache: false,
		url: '/Products/FeedStores/Credentials',
		data: { pageID: GetPageID(), id: id },
		complete: function ()
		{
			HideLoadingPanel();
    },
		success: function (json)
		{
			if (!json) return;
			ClientID.SetValue(json.ClientID);
			ClientSecret.SetValue(null);
			CustomerNumber.SetValue(json.CustomerNumber);
			CountryCode.SetValue(json.CountryCode);
			Username.SetValue(json.Username);
			Password.SetValue(null);
			SourceIdentifier.SetValue(json.SourceIdentifier);
			ConsumerKey.SetValue(json.ConsumerKey);
			ConsumerSecret.SetValue(null);

			CredentialsLoaded = true;

			$('.credentials').addClass('d-none');
			$('.enter-credentials').addClass('d-none');

			switch (FeedStoreType)
			{
				case 'INGRAM':
					$('.ingram').removeClass('d-none');
					break;

				case 'SYNNEX':
					$('.synnex').removeClass('d-none');
					break;
			}
		}
	});
}

function SaveFeedStore() {
	var id = $('#FeedStoreID').val();
	if (!id) return;

	if (!ASPxClientEdit.ValidateGroup()) return;

	$.ajax({
		cache: false,
		url: '/Products/FeedStores/Update',
		data:
		{
			PageID: GetPageID(),
			FeedStoreID: id,
			PushToSupplierEmail: PushToSupplierEmail.GetValue(),
			ClientID: ClientID.GetValue(),
			ClientSecret: ClientSecret.GetValue(),
			CustomerNumber: CustomerNumber.GetValue(),
			CountryCode: CountryCode.GetValue(),
			Username: Username.GetValue(),
			Password: Password.GetValue(),
			SourceIdentifier: SourceIdentifier.GetValue(),
			ConsumerKey: ConsumerKey.GetValue(),
			ConsumerSecret: ConsumerSecret.GetValue()
		},
		complete: function ()
		{
			HideLoadingPanel();
    },
		success: function (error)
		{
			if (error)
			{
				alert(msg);
				return;
			}

			GrdMain.PerformCallback(false);
		}
	});
}

function Validate(s, e, type)
{
	if (FeedStoreType !== type)
	{
		e.isValid = true;
	}

	if (FeedStoreType !== 'EMAIL' && !CredentialsLoaded)
	{
		e.isValid = true;
  }
}