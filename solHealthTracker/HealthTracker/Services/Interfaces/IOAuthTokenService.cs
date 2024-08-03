using HealthTracker.Models.DTOs.GoogleFit;

namespace HealthTracker.Services.Interfaces
{
    public interface IOAuthTokenService
    {
        public Task<string> AddOrUpdateAccessTokenToDB(OAuthAccessTokenDTO accessToken, int UserId);
        public Task<OAuthAccessTokenDTO> GetAccessTokenDTO(int UserId);
        public Task<OAuthCredsResponseDTO> GetOAuthCreds();
    }
}
