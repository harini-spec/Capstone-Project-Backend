using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;

namespace HealthTracker.Models.DTOs.HealthLog
{
    public class GetHealthLogOutputDTO
    {
        public int Id { get; set; }
        public int PreferenceId { get; set; }
        public float value { get; set; }
        public string Unit { get; set; }
        public string HealthStatus { get; set; }
        public string? TargetStatus { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
