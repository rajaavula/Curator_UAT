using System.Collections.Generic;

namespace LeadingEdge.Curator.Core.Configurations
{
    public sealed class OpenIdConnectSettings
    {
        public string Authority { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public IEnumerable<string> Scopes { get; private set; }

        public bool Bypass { get; private set; }
    }
}
