using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class Suggestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CoachId { get; set; }
        public User SuggestionsByCoach { get; set; }

        public int HealthLogId { get; set; }
        public HealthLog SuggestionForHealthLog { get; set; }

        public string Description { get; set; }
        public bool IsLiked { get; set; } = false;
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}
