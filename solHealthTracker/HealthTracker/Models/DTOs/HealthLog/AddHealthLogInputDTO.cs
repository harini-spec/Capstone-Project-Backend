using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.HealthLog
{
    public class AddHealthLogInputDTO
    {
        [Required]
        public int PreferenceId { get; set; }

        [Required]
        public float value { get; set; }
    }
}