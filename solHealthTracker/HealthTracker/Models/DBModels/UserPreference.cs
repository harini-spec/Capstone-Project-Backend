using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class UserPreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User PreferenceForUser { get; set; }

        public int MetricId { get; set; }
        [ForeignKey("MetricId")]
        public Metric Metric { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }


        public List<Target> TargetsForUserPreference { get; set; }
        public List<HealthLog> healthLogsOfUser { get; set; }
    }
}
