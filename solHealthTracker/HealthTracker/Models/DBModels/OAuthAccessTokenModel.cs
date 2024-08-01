using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class OAuthAccessTokenModel
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDetail UserDetailsForUser { get; set; }

        public string AccessToken { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
