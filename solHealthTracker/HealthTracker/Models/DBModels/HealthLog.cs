using HealthTracker.Models.ENUMs;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DBModels
{
    public class HealthLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PreferenceId { get; set; }
        public UserPreference HealthLogForPreference { get; set; }

        public float value { get; set; }
        public HealthStatusEnum.HealthStatus HealthStatus { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }


        public List<Suggestion> SuggestionsForLog { get; set; }
    }
}
