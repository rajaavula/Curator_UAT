using System;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public sealed class ApiCredentialInfo
    {
        public int UserKey { get; set; }
        public Guid UserID { get; set; }
        public string EntityID { get; set; }
        public string DisplayName { get; set; }

        public ApiCredentialInfo()
        {
        }

        public ApiCredentialInfo(DataRow dr)
        {
            UserKey = Utils.FromDBValue<int>(dr["UserKey"]);
            UserID = Utils.FromDBValue<Guid>(dr["UserId"]);
            EntityID = Utils.FromDBValue<string>(dr["EntityId"]);
            DisplayName = Utils.FromDBValue<string>(dr["DisplayName"]);
        }
    }
}