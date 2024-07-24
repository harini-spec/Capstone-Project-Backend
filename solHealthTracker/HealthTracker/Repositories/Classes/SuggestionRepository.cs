using ATMApplication.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class SuggestionRepository : AbstractRepository<int, Suggestion>
    {
        public SuggestionRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
