using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace OrdlyBackend.Utilities
{
    public static class AzureUtils
    {
        static Uri keyVaultEndpoint = new Uri("https://ordlysecrets.vault.azure.net/");
        //static Uri keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
        static SecretClient client = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
        public static string GetSecretFromVault(string key)
        {
            return client.GetSecret(key).Value.Value;
        }

        public static Uri GetUri()
        {
            return keyVaultEndpoint;
        }
    }
}
