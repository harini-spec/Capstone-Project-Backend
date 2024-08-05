using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HealthTracker.Services.Interfaces;
using Azure.AI.OpenAI;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;

namespace HealthTracker.Services.Classes
{
    public class OpenAIService : IOpenAIService
    {

        public async Task<string> GetOpenAIEndpoint()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("AzureOpenAIEndpoint");
            var secret = jwt_secret.Value.Value;
            return secret;
        }

        public async Task<string> GetOpenAISecretKey()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("AzureOpenAIKey");
            var secret = jwt_secret.Value.Value;
            return secret;
        }


        public async Task<string> GetCompletionAsync(string prompt)
        {
            var endpoint = await GetOpenAIEndpoint();
            var apiKey = await GetOpenAISecretKey();

            OpenAIClient openAIClient = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.System, "You are a helpful AI assistant who is allowed to answer questions related to Health, Fitness and Medical topics"),
                    new ChatMessage(ChatRole.User, "Hello"),
                    new ChatMessage(ChatRole.Assistant, "Hello"),
                    new ChatMessage(ChatRole.User, prompt)
                },
                MaxTokens = 150
            };

            Response<ChatCompletions> response = await openAIClient.GetChatCompletionsAsync(deploymentOrModelName: "HealthSync", chatCompletionsOptions);

            var botResponse = response.Value.Choices.First().Message.Content;

            return botResponse;

        }
    }
}
