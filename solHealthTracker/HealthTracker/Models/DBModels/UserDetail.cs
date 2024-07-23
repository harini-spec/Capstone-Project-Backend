using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HealthTracker.Models.ENUMs;

namespace HealthTracker.Models.DBModels
{
    public class UserDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte[] PasswordEncrypted { get; set; }
        public byte[] PasswordHashKey { get; set; }
        public UserStatusEnum.UserStatus Status { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
