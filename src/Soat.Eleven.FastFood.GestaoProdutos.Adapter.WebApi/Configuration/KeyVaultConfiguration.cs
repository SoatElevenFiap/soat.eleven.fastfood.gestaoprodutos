using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.Configuration
{
    public static class KeyVaultConfiguration
    {
        public static WebApplicationBuilder ConfigureKeyVault(this WebApplicationBuilder builder)
        {
            var keyVaultName = builder.Configuration["KeyVault:Name"];

            var isDevelopmentEnvironment = builder.Environment.IsDevelopment();
            TokenCredential credential;

            if (isDevelopmentEnvironment)
            {
                Console.WriteLine("🔧 Ambiente de desenvolvimento detectado - usando configuração local");

                credential = new ChainedTokenCredential(
                    new AzureCliCredential(),
                    new EnvironmentCredential(),
                    new ManagedIdentityCredential()
                );
            }
            else
            {
                Console.WriteLine("🔧 Ambiente de producao detectado - usando configuração de Produção");

                credential = new DefaultAzureCredential();
            }

            if (!string.IsNullOrEmpty(keyVaultName))
            {
                SetKeyVault(builder, keyVaultName, credential);
                Console.WriteLine("Configuração do Key Vault para SecretKey carregada com sucesso.");
            }

            return builder;
        }

        private static void SetKeyVault(WebApplicationBuilder builder, string keyVaultName, TokenCredential credential)
        {
            try
            {
                builder.Configuration.AddAzureKeyVault(
                    new Uri($"https://{keyVaultName}.vault.azure.net/"),
                    credential,
                    new KeyVaultSecretManager());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO : ao configurar o Key Vault: {ex.Message}");
                throw;
            }
        }
    }
}
