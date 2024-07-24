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
        public TargetStatusEnum.TargetStatus TargetStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
