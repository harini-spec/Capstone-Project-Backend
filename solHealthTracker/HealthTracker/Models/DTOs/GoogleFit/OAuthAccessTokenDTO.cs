using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.GoogleFit
{
    public class OAuthAccessTokenDTO
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
