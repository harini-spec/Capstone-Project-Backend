namespace HealthTracker.Models.DTOs.Suggestions
{
    public class ProblemOutputDTO
    {
        public int UserId { get; set; }
        public List<string> MetricsWithProblem { get; set; }
    }
}
