using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace LeadingEdge.Curator.Core
{
    public static class SecretManager
    {
        private static readonly Lazy<SecretClient> _client = new Lazy<SecretClient>(SetupClient);

        private static SecretClient Client => _client.Value;

        private static SecretClient SetupClient()
        {
            var endpoint = !string.IsNullOrWhiteSpace(App.KeyVaultEndpoint) ? App.KeyVaultEndpoint : Environment.GetEnvironmentVariable("KEY_VAULT_ENDPOINT");

            var uri = new Uri(endpoint);

            return new SecretClient(uri, new DefaultAzureCredential());
        }

        public static string GetSecret(string key)
        {
            try
            {
                var secret = Client.GetSecret(key);

                return secret?.Value?.Value;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static void SaveSecret(string key, string value)
        {
            Client.SetSecret(key, value);
        }
    }
}