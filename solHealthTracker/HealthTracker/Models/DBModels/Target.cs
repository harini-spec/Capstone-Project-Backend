using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class Target
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int PreferenceId { get; set; }
        public UserPreference TargetForUserPreference { get; set; }

        public float TargetMinValue { get; set; }
        public float TargetMaxValue { get; set; }
        public TargetStatusEnum.TargetStatus TargetStatus { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

    }
}
