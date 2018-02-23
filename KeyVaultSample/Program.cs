using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultSample
{
    static class Program
    {
        const string CLIENT_ID = "ef408b79-2afc-4204-8dad-b422f17e15ee";
        const string CLIENT_SECRET = "SQvyGbWQh7iAoVi3aHCOlxdbaOwVXunp70r9eaSypEk=";

        const string CERTIFICATE_IDENTIFIER = "https://key-vault-just-for-test.vault.azure.net/certificates/TestCertificate/ee3c947cdb5241c2a7f711f9770b669e";

        const string KEY_VAULT_IDENTIFIER = "https://key-vault-just-for-test.vault.azure.net/";
        const string CERTIFICATE_NAME = "TestCertificate";

        static void Main(string[] args)
        {
            var Client = GetClient();
            var Certificate = Client.GetCertificateAsync(CERTIFICATE_IDENTIFIER).GetAwaiter().GetResult();
            Console.WriteLine(Certificate.X509Thumbprint.ToHexString());

            var Certificate2 = Client.GetCertificateAsync(KEY_VAULT_IDENTIFIER, CERTIFICATE_NAME).GetAwaiter().GetResult();
            Console.WriteLine(Certificate2.X509Thumbprint.ToHexString());
        }

        static KeyVaultClient GetClient() => new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (string authority, string resource, string scope) =>
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            ClientCredential clientCred = new ClientCredential(CLIENT_ID, CLIENT_SECRET);
            var authResult = await context.AcquireTokenAsync(resource, clientCred);
            return authResult.AccessToken;
        }));

        static string ToHexString(this byte[] hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return string.Empty;

            var s = new StringBuilder();
            foreach (byte b in hex)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }

    }
}
