using System;
using System.Collections.Generic;
using DevExpress.Web.Mvc;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Web.Admin.Models
{
    public class StoreListEdit : BaseModel
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreUrl { get; set; }
        public bool Live { get; set; }
        public string StoreApiKey { get; set; }
        public string StorePassword { get; set; }
        public string StoreSharedSecret { get; set; }
        public long ShopifyID { get; set; }
        public byte[] Logo { get; set; }
        public List<StoreInfo> Stores { get; set; }

        public UploadControlSettings StoreLogoUploadControlSettings { get; set; }

        public StoreListEdit()
        {
            Stores = new List<StoreInfo>();
            StoreLogoUploadControlSettings = new UploadControlSettings();
        }
    }
}