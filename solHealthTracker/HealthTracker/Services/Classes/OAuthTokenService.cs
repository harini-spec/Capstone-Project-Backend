using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.GoogleFit;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace HealthTracker.Services.Classes
{
    public class OAuthTokenService : IOAuthTokenService
    {
        private readonly IRepository<int, OAuthAccessTokenModel> _OAuthTokenRepository;
        private static readonly HttpClient HttpClient = new HttpClient();

        public OAuthTokenService(IRepository<int, OAuthAccessTokenModel> oAuthTokenRepository)
        {
            _OAuthTokenRepository = oAuthTokenRepository;
        }

        public async Task<string> GetSecretKey()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var client_id = await client.GetSecretAsync("GoogleClientSecret");
            var secret = client_id.Value.Value;
            return secret;
        }

        public async Task<string> GetClientID()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var client_secret = await client.GetSecretAsync("GoogleClientId");
            var secret = client_secret.Value.Value;
            return secret;
        }

        public async Task<string> AddOrUpdateAccessTokenToDB(OAuthAccessTokenDTO accessToken, int UserId)
        {
            try
            {
                try
                {
                    var token = await _OAuthTokenRepository.GetById(UserId);
                    token.AccessToken = accessToken.AccessToken;
                    token.Updated_at = DateTime.Now;
                    await _OAuthTokenRepository.Update(token);
                }
                catch(EntityNotFoundException)
                {
                    OAuthAccessTokenModel oAuthAccessTokenModel = new OAuthAccessTokenModel();
                    oAuthAccessTokenModel.UserId = UserId;
                    oAuthAccessTokenModel.AccessToken = accessToken.AccessToken;
                    oAuthAccessTokenModel.Created_at = DateTime.Now;
                    oAuthAccessTokenModel.Updated_at = DateTime.Now;
                    await _OAuthTokenRepository.Add(oAuthAccessTokenModel);
                }
                return "Successfully Updated!";
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> IsTokenExpiredAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/tokeninfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await HttpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenInfo = JObject.Parse(content);

                // Example of checking the expiration time
                var exp = tokenInfo["exp"]?.ToObject<long>() ?? 0;
                var expDate = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;
                return expDate < DateTime.UtcNow;
            }
            else
            {
                Console.WriteLine("Error validating token: " + response.ReasonPhrase);
                return true; // Assume expired if the token cannot be validated
            }
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id", await GetClientID() },
                        { "client_secret", await GetSecretKey() },
                        { "refresh_token", refreshToken },
                        { "grant_type", "refresh_token" }
                    })
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                return tokenResponse.AccessToken;
            }
        }

        public async Task<OAuthAccessTokenDTO> GetAccessTokenDTO(int UserId)
        {
            try
            {
                var token = await _OAuthTokenRepository.GetById(UserId);
                if (await IsTokenExpiredAsync(token.AccessToken))
                {
                    token.AccessToken = await RefreshTokenAsync(token.AccessToken);
                }
                OAuthAccessTokenDTO oAuthAccessTokenDTO = new OAuthAccessTokenDTO();
                oAuthAccessTokenDTO.AccessToken = token.AccessToken;
                return oAuthAccessTokenDTO;
            }
            catch
            {
                throw new EntityNotFoundException("No Access Token found!");
            }
        }

    }
}
