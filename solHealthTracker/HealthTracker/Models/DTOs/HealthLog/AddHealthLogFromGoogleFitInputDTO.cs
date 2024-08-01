using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.HealthLog
{
    public class AddHealthLogFromGoogleFitInputDTO
    {
        [Required]
        public string MetricType { get; set; }

        [Required]
        public float Value { get; set; }
    }
}
