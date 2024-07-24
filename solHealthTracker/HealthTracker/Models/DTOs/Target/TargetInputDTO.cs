using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthTracker.Models.DTOs.Target
{
    public class TargetInputDTO
    {
        public string MetricType { get; set; }
        public float TargetMinValue { get; set; }
        public float TargetMaxValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
