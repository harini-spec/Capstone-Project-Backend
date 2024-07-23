using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class IdealData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MetricId { get; set; }
        [ForeignKey("MetricId")]
        public Metric Metric { get; set; }

        public HealthStatusEnum.HealthStatus HealthStatus { get; set; }

        public float MinVal { get; set; }
        public float MaxVal { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
