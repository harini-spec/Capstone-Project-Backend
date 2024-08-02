using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthTracker.Services.Classes
{
    public class TokenService : ITokenService
    {
        public TokenService()
        {
        }

        public async Task<string> GetSecretKey()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("JWTToken");
            var secret = jwt_secret.Value.Value;
            return secret;
        }


        #region GenerateToken

        // Generate JWT token with Symmetric key
        public async Task<string> GenerateToken<T>(T user)
        {
            string secret_key = await GetSecretKey();
            SymmetricSecurityKey _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key));

            string token = string.Empty;
            var claims = new List<Claim>();

            // Add claims
            var idProperty = typeof(T).GetProperty("UserId");
            if (idProperty != null)
            {
                claims.Add(new Claim("ID", idProperty.GetValue(user).ToString()));
            }

            var nameProperty = typeof(T).GetProperty("Name");
            if (nameProperty != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, nameProperty.GetValue(user).ToString()));
            }

            var emailProperty = typeof(T).GetProperty("Email");
            if (emailProperty != null)
            {
                claims.Add(new Claim(ClaimTypes.Email, emailProperty.GetValue(user).ToString()));
            }

            var roleProperty = typeof(T).GetProperty("Role");
            if (roleProperty != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleProperty.GetValue(user).ToString()));
            }

            // Algorithm
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            // Generate token
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2), signingCredentials: credentials);

            // Convert token to string
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }

        #endregion


    }
}