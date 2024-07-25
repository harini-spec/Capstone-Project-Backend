namespace HealthTracker.Models.DTOs.Graph
{
    public class GraphDataOutputDTO
    {
        public int LogId { get; set; }
        public DateTime LogDate { get; set; }
        public float Value { get; set; }
        public string HealthStatus { get; set; }
    }
}