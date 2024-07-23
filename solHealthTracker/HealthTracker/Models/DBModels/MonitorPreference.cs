using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DBModels
{
    public class MonitorPreference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CoachId { get; set; }
        public User MonitorPreferenceForCoach { get; set; }

        public int MetricId { get; set; }
        [ForeignKey("MetricId")]
        public Metric Metric { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
