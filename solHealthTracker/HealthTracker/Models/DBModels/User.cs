using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using HealthTracker.Models.ENUMs;

namespace HealthTracker.Models.DBModels
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDetail UserDetailsForUser { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public GenderEnum.Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public RolesEnum.Roles Role { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }


        public List<UserPreference>? UserPreferences { get; set; }
        public List<MonitorPreference>? MonitorPreferences { get; set; }
        public List<Suggestion>? Suggestions { get; set; }
    }
}
