namespace HealthTracker.Models.DTOs.Graph
{
    public class GraphDataRangeOutputDTO
    {
        public string MetricType { get; set; }
        public string MetricUnit { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
    }
}