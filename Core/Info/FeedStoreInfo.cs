using System.Data;
using System.Linq;
using System.Reflection;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class FeedStoreInfo
    {
        public string FeedStoreID { get; set; }
        public int FeedID { get; set; }
        public string FeedName { get; set; }
        public bool IsEDISupplier { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public bool CredentialsProvided { get; set; }
        public bool CredentialsInvalid { get; set; }
        public string PushToSupplierEmail { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string CustomerNumber { get; set; }
        public string CountryCode { get; set; }
        public string SenderID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SourceIdentifier { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public bool CredentialsLoaded { get; set; }

        public FeedStoreInfo() { }

        public FeedStoreInfo(DataRow dr)
        {
            FeedStoreID = Utils.FromDBValue<string>(dr["FeedStoreID"]);
            FeedID = Utils.FromDBValue<int>(dr["FeedID"]);
            FeedName = Utils.FromDBValue<string>(dr["FeedName"]);
            IsEDISupplier = Utils.FromDBValue<bool>(dr["IsEDISupplier"]);
            StoreID = Utils.FromDBValue<int>(dr["StoreID"]);
            StoreName = Utils.FromDBValue<string>(dr["StoreName"]);
            CredentialsProvided = Utils.FromDBValue<bool>(dr["CredentialsProvided"]);
            CredentialsInvalid = Utils.FromDBValue<bool>(dr["CredentialsInvalid"]);
            PushToSupplierEmail = Utils.FromDBValue<string>(dr["PushToSupplierEmail"]);
        }

        public FeedStoreInfo RemoveSecrets()
        {
            var info = new FeedStoreInfo();

            foreach (PropertyInfo property in typeof(FeedStoreInfo).GetProperties().Where(p => p.CanWrite))
            {
                property.SetValue(info, property.GetValue(this, null), null);
            }

            info.ClientSecret = null;
            info.Password = null;
            info.ConsumerSecret = null;

            return info;
        }
    }
}