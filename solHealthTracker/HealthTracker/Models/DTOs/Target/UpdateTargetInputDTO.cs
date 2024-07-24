using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.Target
{
    public class UpdateTargetInputDTO
    {
        [Required]
        public int TargetId { get; set; }

        [Required]
        public string MetricType { get; set; }

        [Required]
        public float TargetMinValue { get; set; }

        [Required]
        public float TargetMaxValue { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }
    }
}
