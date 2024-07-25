using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;

namespace HealthTracker.Models.DTOs.Target
{
    public class TargetOutputDTO
    {
        public int Id { get; set; }
        public int PreferenceId { get; set; }
        public float TargetMinValue { get; set; }
        public float TargetMaxValue { get; set; }
        public string TargetStatus { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
