using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.Target
{
    public class TargetInputDTO
    {
        [Required]
        public int PreferenceId { get; set; }

        [Required]
        public float TargetMinValue { get; set; }

        [Required]
        public float TargetMaxValue { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }
    }
}
