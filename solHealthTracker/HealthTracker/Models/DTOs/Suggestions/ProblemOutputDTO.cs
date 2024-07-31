namespace HealthTracker.Models.DTOs.Suggestions
{
    public class ProblemOutputDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public List<string> MetricsWithProblem { get; set; }
    }
}
