using HealthTracker.Models.DBModels;

namespace HealthTracker.Models.DTOs.Suggestions
{
    public class SuggestionOutputDTO
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
