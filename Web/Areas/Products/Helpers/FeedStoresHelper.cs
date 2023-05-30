using DevExpress.Utils.About;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Products.Models;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LeadingEdge.Curator.Web.Products.Helpers
{
    public class FeedStoresHelper
    {
        public static FeedStoresListEdit CreateModel()
        {
            var vm = new FeedStoresListEdit();

            vm.Grids.Add("GrdMain", GetGridView("GrdMain", false, vm));

            return vm;
        }

        public static void UpdateModel(FeedStoresListEdit vm, FeedStoresListEdit cached, bool isExporting)
        {
            vm.Grids = cached.Grids;

            if (isExporting) return;
        }

        public static GridModel GetGridView(string name, bool exporting, FeedStoresListEdit vm)
        {
            GridModel grid = new GridModel();
            Setup.GridView(grid.Settings, name, "v1.01", vm.Name);
            grid.Settings.KeyFieldName = "FeedStoreID";
            grid.Settings.CallbackRouteValues = new { Area = "Products", Controller = "FeedStores", Action = "GrdMainCallback" };
            grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
            grid.Settings.ClientSideEvents.EndCallback = "function(s,e) { Get(); }";

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "FeedName";
                s.Caption = grid.Label(200966); // Feed
                s.Width = 200;
            });
            
            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "StoreName";
                s.Caption = grid.Label(300071); // Store
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.TextBox;
                s.FieldName = "PushToSupplierEmail";
                s.Caption = grid.Label(300176); // Push to supplier email
                s.Width = 250;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "IsEDISupplier";
                s.Caption = grid.Label(300180); // EDI supplier?
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "CredentialsProvided";
                s.Caption = grid.Label(300181); // EDI credentials provided?
                s.Width = 200;
            });

            grid.Settings.Columns.Add(s =>
            {
                s.ColumnType = MVCxGridViewColumnType.CheckBox;
                s.FieldName = "CredentialsInvalid";
                s.Caption = grid.Label(300200); // EDI credentials invalid?
                s.Width = 200;
            });

            return grid;
        }

        public static List<FeedStoreInfo> GetData()
        {
            return FeedStoreManager.GetAllFeedStores();
        }

        public static FeedStoreInfo GetDetail(FeedStoresListEdit vm, string id)
        {
            return ((List<FeedStoreInfo>)vm.Grids["GrdMain"].Data).Find(x => x.FeedStoreID == id);
        }

        public static FeedStoreInfo GetCredentials(FeedStoresListEdit vm, string id)
        {
            var detail = GetDetail(vm, id);
            if (!detail.CredentialsProvided || detail.CredentialsLoaded)
            {
                return detail;
            }

            switch (detail.FeedName)
            {
                case "INGRAM":
                    detail.ClientID = SecretManager.GetSecret($"Ingram--EdiCredentials--{detail.FeedStoreID}--ClientID");
                    detail.ClientSecret = SecretManager.GetSecret($"Ingram--EdiCredentials--{detail.FeedStoreID}--ClientSecret");
                    detail.CustomerNumber = SecretManager.GetSecret($"Ingram--EdiCredentials--{detail.FeedStoreID}--CustomerNumber");
                    detail.CountryCode = SecretManager.GetSecret($"Ingram--EdiCredentials--{detail.FeedStoreID}--CountryCode");
                    detail.CredentialsLoaded = true;
                    break;

                case "SYNNEX":
                    detail.Username = SecretManager.GetSecret($"Synnex--EdiCredentials--{detail.FeedStoreID}--Username");
                    detail.Password = SecretManager.GetSecret($"Synnex--EdiCredentials--{detail.FeedStoreID}--Password");
                    detail.SourceIdentifier = SecretManager.GetSecret($"Synnex--EdiCredentials--{detail.FeedStoreID}--SourceIdentifier");
                    detail.ConsumerKey = SecretManager.GetSecret($"Synnex--EdiCredentials--{detail.FeedStoreID}--ConsumerKey");
                    detail.ConsumerSecret = SecretManager.GetSecret($"Synnex--EdiCredentials--{detail.FeedStoreID}--ConsumerSecret");
                    detail.CredentialsLoaded = true;
                    break;
            }

            return detail;
        }

        public static string Save(FeedStoresListEdit vm, FeedStoreInfo info)
        {
            var detail = GetDetail(vm, info.FeedStoreID);
            if (detail == null) return "The selected feed store's details could not be found.";

            info.FeedID = detail.FeedID;
            info.StoreID = detail.StoreID;

            switch (detail.FeedName)
            {
                case "INGRAM":
                    if (info.ClientID != detail.ClientID)
                    {
                        SecretManager.SaveSecret($"Ingram--EdiCredentials--{info.FeedStoreID}--ClientID", info.ClientID);

                        detail.ClientID = info.ClientID;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(info.ClientSecret) && info.ClientSecret != detail.ClientSecret)
                    {
                        SecretManager.SaveSecret($"Ingram--EdiCredentials--{info.FeedStoreID}--ClientSecret", info.ClientSecret);

                        detail.ClientSecret = info.ClientSecret;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (info.CustomerNumber != detail.CustomerNumber)
                    {
                        SecretManager.SaveSecret($"Ingram--EdiCredentials--{info.FeedStoreID}--CustomerNumber", info.CustomerNumber);

                        detail.CustomerNumber = info.CustomerNumber;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (info.CountryCode != detail.CountryCode)
                    {
                        SecretManager.SaveSecret($"Ingram--EdiCredentials--{info.FeedStoreID}--CountryCode", info.CountryCode);

                        detail.CountryCode = info.CountryCode;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    info.PushToSupplierEmail = null;

                    break;

                case "SYNNEX":
                    if (info.Username != detail.Username)
                    {
                        SecretManager.SaveSecret($"Synnex--EdiCredentials--{info.FeedStoreID}--Username", info.Username);

                        detail.Username = info.Username;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(info.Password) && info.Password != detail.Password)
                    {
                        SecretManager.SaveSecret($"Synnex--EdiCredentials--{info.FeedStoreID}--Password", info.Password);

                        detail.Password = info.Password;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (info.SourceIdentifier != detail.SourceIdentifier)
                    {
                        SecretManager.SaveSecret($"Synnex--EdiCredentials--{info.FeedStoreID}--SourceIdentifier", info.SourceIdentifier);

                        detail.SourceIdentifier = info.SourceIdentifier;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (info.ConsumerKey != detail.ConsumerKey)
                    {
                        SecretManager.SaveSecret($"Synnex--EdiCredentials--{info.FeedStoreID}--ConsumerKey", info.ConsumerKey);

                        detail.ConsumerKey = info.ConsumerKey;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    if (!string.IsNullOrWhiteSpace(info.ConsumerSecret) && info.ConsumerSecret != detail.ConsumerSecret)
                    {
                        SecretManager.SaveSecret($"Synnex--EdiCredentials--{info.FeedStoreID}--ConsumerSecret", info.ConsumerSecret);

                        detail.ConsumerSecret = info.ConsumerSecret;

                        info.CredentialsProvided = true;

                        detail.CredentialsProvided = true;
                        detail.CredentialsLoaded = true;
                    }

                    info.PushToSupplierEmail = null;

                    break;

                default:
                    detail.PushToSupplierEmail = info.PushToSupplierEmail;
                    break;
            }

            var ex = FeedStoreManager.SaveFeedStore(info);
            if (ex != null) return "There was an unexpected error saving changes to the feed store.";

            return null;
        }
    }
}